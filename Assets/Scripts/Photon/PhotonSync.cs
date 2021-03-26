using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityAtoms.BaseAtoms;
using UnityAtoms;
using TMPro;
using Photon.Realtime;

public class PhotonSync : MonoBehaviourPun
{

    [SerializeField] GameObjectEventReference SetParentEvent;

    [Header("Timer")]
    [SerializeField] FloatConstant VotingTime;
    [SerializeField] FloatReference VotingTimer;
    [SerializeField] BoolReference VotingsOpen;

    [Header("Questions")]
    [SerializeField] AtomList Questions;
    [SerializeField] IntReference currentQuestionIndex;

    [Header("Answers")]
    [SerializeField] VoteList PSVotes;
    [SerializeField] VoteList ChatVotes;
    [SerializeField] BoolEventReference ReveilAnswerEvent;
    [SerializeField] TMP_InputField InputFieldAnswer;

    [Header("Score")]
    [SerializeField] IntEventReference UpdateChatScoreEvent;
    [SerializeField] IntEventReference UpdatePSScoreEvent;
    [SerializeField] VoidBaseEventReference UpdateDisplayEvent;

    private void Awake()
    {
        if (!photonView.Owner.IsMasterClient)
        {
            InputFieldAnswer.placeholder.GetComponent<TextMeshProUGUI>().text = photonView.Owner.NickName;
            SetParentEvent.Event.Raise(gameObject.transform.parent.gameObject);
            InputFieldAnswer.interactable = photonView.IsMine;
        }
    }

    #region Sync Timer

    public void StartVoting()
    {
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();

        foreach (PhotonView photonView in photonViews)
            if (!photonView.Owner.IsMasterClient)
                photonView.RPC("StartVotingRPC", photonView.Owner);
    }

    [PunRPC]
    void StartVotingRPC()
    {
        VotingsOpen.Value = true;
        VotingTimer.Value = VotingTime.Value;
    }

    #endregion

    #region Sync Votes
    public void UpdateVotes()
    {
        SendCalltoClient();
        SendChatVotesToClient();
    }

    #region PSVotesToMaster
    public void SendCalltoClient()
    {
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();

        foreach (PhotonView photonView in photonViews)
            if (!photonView.Owner.IsMasterClient)
                photonView.RPC("SendVotesToMasterRPC", photonView.Owner);
    }

    [PunRPC]
    public void SendVotesToMasterRPC()
    {
        string _message = InputFieldAnswer.text;
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();

        photonView.RPC("UpdateInputFieldRPC", RpcTarget.All, _message);

        foreach (PhotonView photonView in photonViews)
            photonView.RPC("ReceiveAnswerRPC", RpcTarget.All, _message);
    }

    [PunRPC]
    void UpdateInputFieldRPC(string _answer)
    {
        InputFieldAnswer.text = _answer;
    }

    [PunRPC]
    void ReceiveAnswerRPC(string _answer)
    {
        if (!photonView.IsMine) return;

        Debug.Log($"Received Vote: { _answer}");
        CountVote(PSVotes, _answer);
        UpdateDisplay();
    }
    #endregion

    #region ChatVotesToClient
    public void SendChatVotesToClient()
    {
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();

        foreach (PhotonView photonView in photonViews)
            if (!photonView.Owner.IsMasterClient)
                foreach (Vote vote in ChatVotes.VotesList)
                    photonView.RPC("ClientReceiveAnswers", photonView.Owner, vote.message, vote.amount);
    }

    [PunRPC]
    void ClientReceiveAnswers(string _answer, int _amount)
    {
        if (!photonView.IsMine) return;

        Debug.Log("Received Vote: " + _answer + " " + _amount);
        Vote vote = new Vote() { message = _answer, amount = _amount };
        ChatVotes.VotesList.Add(vote);
        UpdateDisplay();
    }

    #endregion
    void UpdateDisplay()
    {
        UpdateDisplayEvent.Event.Raise();
    }
    public void CountVote(VoteList _list, string _answer)
    {
        // Return if Answer is not valid
        Question currentQuestion = (Question)Questions.Value[currentQuestionIndex.Value];
        if (!CheckForValidAnswer.CheckIfValid(_answer, currentQuestion))
        {
            Debug.Log($"Not a valid answer {_answer}");
            return;
        }

        // Add Vote to Dictionary
        foreach (Vote vote in _list.VotesList)
            if (vote.message == _answer)
            {
                vote.amount++;
                return;
            }
        Vote NewVote = new Vote() { message = _answer, amount = 1 };
        _list.VotesList.Add(NewVote);
    }
    #endregion

    #region Sync Answer
    public void ReveilAnswer(bool _showAnswer)
    {
        photonView.RPC("ReveilAnswerRPC", RpcTarget.All, _showAnswer);
    }

    [PunRPC]
    void ReveilAnswerRPC(bool _showAnswer)
    {
        ReveilAnswerEvent.Event.Raise(_showAnswer);
    }
    #endregion

    #region Sync Score
    public void UpdateScore(int _psScore, int _chatScore)
    {
        UpdatePSScore(_psScore);
        UpdateChatScore(_chatScore);
    }

    void UpdatePSScore(int _score)
    {
        photonView.RPC("UpdatePSScoreRPC", RpcTarget.Others, _score);
    }

    void UpdateChatScore(int _score)
    {
        photonView.RPC("UpdateChatScoreRPC", RpcTarget.Others, _score);
    }

    [PunRPC]
    void UpdatePSScoreRPC(int _score)
    {
        UpdatePSScoreEvent.Event.Raise(_score);
    }

    [PunRPC]
    void UpdateChatScoreRPC(int _score)
    {
        UpdateChatScoreEvent.Event.Raise(_score);
    }
    #endregion

    #region Sync Questions
    public void NextQuestion(int _index)
    {
        photonView.RPC("NextQuestionRPC", RpcTarget.All, _index);
    }

    [PunRPC]
    void NextQuestionRPC(int _index)
    {
        ChatVotes.VotesList.Clear();
        PSVotes.VotesList.Clear();
        currentQuestionIndex.Value = _index;
    }
    #endregion

    public void EnableInputField(bool _state)
    {
        InputFieldAnswer.interactable = _state && photonView.IsMine;
    }
}
