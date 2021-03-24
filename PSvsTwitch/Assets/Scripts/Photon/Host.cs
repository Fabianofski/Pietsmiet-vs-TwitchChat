using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityAtoms.BaseAtoms;
using Photon.Pun;

public class Host : MonoBehaviour
{

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

    private void Start()
    {
        GetComponent<PlayerInput>().enabled = PhotonNetwork.IsMasterClient;
        this.enabled = PhotonNetwork.IsMasterClient;
    }

    private void StartTimer()
    {
        VotingsOpen.Value = true;
        VotingTimer.Value = VotingTime.Value;
    }

    public void NextQuestion(InputAction.CallbackContext _context)
    {
        if (!_context.performed || VotingsOpen.Value) return;

        ChatVotes.VotesList.Clear();
        PSVotes.VotesList.Clear();

        ReveilAnswerEvent.Event.Raise(false);
        currentQuestionIndex.Value++;
        StartTimer();
    }

    public void ReveilAnswer(InputAction.CallbackContext _context)
    {
        if (!_context.performed || VotingTimer.Value > 0) return;
        DistributePoints();
        ReveilAnswerEvent.Event.Raise(true);
    }

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

        return _list.VotesList[0].message == currentQuestion.Answer;
    }

    float CheckDistanceToAnswer(VoteList _list)
    {
        _list.VotesList.Sort(ChatDisplay.SortbyVotes);
        if (_list.VotesList.Count < 1)
            return Mathf.Infinity;

        Question currentQuestion = (Question)Questions.Value[currentQuestionIndex];
        if (float.TryParse(currentQuestion.Answer, out float answer) && float.TryParse(_list.VotesList[0].message, out float msg))
        {
            return answer - msg;
        }
        return Mathf.Infinity;
    }

}
