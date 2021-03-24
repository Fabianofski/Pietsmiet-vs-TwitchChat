using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityAtoms.BaseAtoms;
using UnityAtoms;
using TMPro;

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
    [SerializeField] BoolEventReference ReveilAnswerEvent;
    [SerializeField] TMP_InputField InputFieldAnswer;

    [Header("Score")]
    [SerializeField] IntEventReference UpdateChatScoreEvent;
    [SerializeField] IntEventReference UpdatePSScoreEvent;

    private void Awake()
    {
        if(!photonView.Owner.IsMasterClient)
            SetParentEvent.Event.Raise(gameObject.transform.parent.gameObject);
    }

    public void NextQuestion(bool _VotingsOpened)
    {
        if(_VotingsOpened && PhotonNetwork.IsMasterClient && photonView.IsMine)
            photonView.RPC("NextQuestionRPC", RpcTarget.Others);
    }

    public void ReveilAnswer(bool _showAnswer)
    {
        if (PhotonNetwork.IsMasterClient && photonView.IsMine)
            photonView.RPC("ReveilAnswerRPC", RpcTarget.Others, _showAnswer);
    }

    public void UpdatePSScore(int _score)
    {
        if (PhotonNetwork.IsMasterClient && photonView.IsMine)
            photonView.RPC("UpdatePSScoreRPC", RpcTarget.Others, _score);
    }

    public void UpdateChatScore(int _score)
    {
        if (PhotonNetwork.IsMasterClient && photonView.IsMine)
            photonView.RPC("UpdateChatScoreRPC", RpcTarget.Others, _score);
    }

    public void VotingEnded(bool _state)
    {
        if(!_state && !PhotonNetwork.IsMasterClient && photonView.IsMine)
            photonView.RPC("ReceiveAnswer", RpcTarget.MasterClient, InputFieldAnswer.text);
    }

    [PunRPC]
    void NextQuestionRPC()
    {
        Debug.LogError("Next");
        currentQuestionIndex.Value++;
        VotingsOpen.Value = true;
        VotingTimer.Value = VotingTime.Value;
    }

    [PunRPC]
    void ReveilAnswerRPC(bool _showAnswer)
    {
        ReveilAnswerEvent.Event.Raise(_showAnswer);
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

    [PunRPC]
    void ReceiveAnswer(string _answer)
    {
        Debug.Log("Received Vote: " + _answer);
        CountVote(PSVotes, _answer);
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
}
