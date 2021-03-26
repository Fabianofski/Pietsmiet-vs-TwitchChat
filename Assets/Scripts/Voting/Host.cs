using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using UnityAtoms.FSM;
using Photon.Pun;

public class Host : MonoBehaviour
{
    [SerializeField] FiniteStateMachine HostStateMachine;
    PhotonSync photonSync;

    [Header("Timer")]
    [SerializeField] FloatConstant VotingTime;
    [SerializeField] FloatReference VotingTimer;
    [SerializeField] BoolReference VotingsOpen;

    [Header("Questions")]
    [SerializeField] AtomList Questions;
    [SerializeField] IntReference currentQuestionIndex;
    [SerializeField] BoolEventReference ReveilAnswerEvent;

    [Header("Score")]
    [SerializeField] IntReference PSPoints;
    [SerializeField] VoteList PSVotes;
    [SerializeField] IntReference ChatPoints;
    [SerializeField] VoteList ChatVotes;

    [Header("Voting")]
    [SerializeField] GameObject TwitchChatIRC;

    private void Start()
    {
        GetComponent<PlayerInput>().enabled = PhotonNetwork.IsMasterClient;
        TwitchChatIRC.SetActive(PhotonNetwork.IsMasterClient);
        photonSync = GetComponentInChildren<PhotonSync>();

        NextQuestion();
    }
    public void NextState(InputAction.CallbackContext _context)
    {
        if (!_context.performed) return;

        switch (HostStateMachine.Value)
        {
            case "QUESTION_STARTED": StartVoting();  break;
            case "VOTING_STARTED":
                if (!VotingsOpen.Value)
                    ReveilVoting();
                break;
            case "VOTING_REVEILED": ReveilAnswer(); break;
            case "ANSWER_REVEILED":  NextQuestion();  break;
        }
    }
    private void StartVoting()
    {
        HostStateMachine.Dispatch("START_VOTING");
        Debug.Log("START VOTING");

        photonSync.StartVoting();

        VotingsOpen.Value = true;
        VotingTimer.Value = VotingTime.Value;
    }
    public void ReveilVoting()
    {
        HostStateMachine.Dispatch("REVEIL_VOTING");
        Debug.Log("REVEIL VOTINGS");

        photonSync.UpdateVotes();
    }
    public void ReveilAnswer()
    {
        HostStateMachine.Dispatch("REVEIL_ANSWER");
        Debug.Log("REVEIL ANSWER");

        DistributePoints();

        photonSync.UpdateScore(PSPoints.Value, ChatPoints.Value);
        photonSync.ReveilAnswer(true);
    }
    #region Calculating Points
    private void DistributePoints()
    {
        Question currentQuestion = (Question)Questions.Value[currentQuestionIndex];
        switch (currentQuestion.answerType)
        {
            case Question.AnswerType.Exact:
                if (CheckIfAnswerIsCorrect(PSVotes))
                { 
                    PSPoints.Value++;
                    Debug.Log("Pietsmiet was correct");
                }
                if (CheckIfAnswerIsCorrect(ChatVotes))
                {
                    ChatPoints.Value++;
                    Debug.Log("Chat was correct");
                }
                break;
            case Question.AnswerType.Closest:
                float PSDistance = CheckDistanceToAnswer(PSVotes);
                float ChatDistance = CheckDistanceToAnswer(ChatVotes);
                Debug.Log($"Pietsmiet was {PSDistance} away");
                Debug.Log($"Chat was {ChatDistance} away");

                if (PSDistance < ChatDistance)
                    PSPoints.Value++;
                else if (ChatDistance < PSDistance)
                    ChatPoints.Value++;
                else if(ChatDistance == PSDistance)
                {
                    ChatPoints.Value++;
                    PSPoints.Value++;
                }
                break;
        }
    }

    bool CheckIfAnswerIsCorrect(VoteList _list)
    {
        _list.VotesList.Sort(ChatDisplay.SortbyVotes);
        Question currentQuestion = (Question)Questions.Value[currentQuestionIndex];

        if (_list.VotesList.Count < 1)
            return false;

        return currentQuestion.Answer.Contains(_list.VotesList[0].message);
    }

    float CheckDistanceToAnswer(VoteList _list)
    {
        _list.VotesList.Sort(ChatDisplay.SortbyVotes);
        if (_list.VotesList.Count < 1)
            return Mathf.Infinity;

        Question currentQuestion = (Question)Questions.Value[currentQuestionIndex];
        if (float.TryParse(currentQuestion.Answer[0], out float answer) && float.TryParse(_list.VotesList[0].message, out float msg))
        {
            return answer - msg;
        }
        return Mathf.Infinity;
    }
    #endregion
    public void NextQuestion()
    {
        HostStateMachine.Dispatch("NEXT_QUESTION");
        Debug.Log("NEXT QUESTION");

        photonSync.ReveilAnswer(false);
        photonSync.NextQuestion(currentQuestionIndex.Value + 1);
    }
}
