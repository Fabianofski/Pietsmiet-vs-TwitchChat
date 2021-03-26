using UnityEngine;
using UnityAtoms.BaseAtoms;
using System.Collections.Generic;

public class TwitchChat : MonoBehaviour
{
    private TwitchIRC IRC;

    [SerializeField] VoteList Votes;
    [SerializeField] BoolReference VotingsOpen;
    [SerializeField] AtomList Questions;
    [SerializeField] IntReference currentQuestionIndex;
    Question currentQuestion;

    private void Start()
    {
        IRC = GameObject.Find("TwitchIRC").GetComponent<TwitchIRC>();
        Votes.VotesList.Clear();
        IRC.newChatMessageEvent.AddListener(NewMessage);
    }

    public void StartNewVotingRound(bool _state)
    {
        if (_state)
        {
            // Clear all Votes
            Votes.VotesList.Clear();
            currentQuestion = (Question)Questions.Value[currentQuestionIndex.Value];
            IRC.SendChatMessage("Voting started");
        }
        else
            IRC.SendChatMessage("Voting ended");

    }

    public void NewMessage(Chatter _chatter)
    {
        CountVote(Votes, _chatter.message);
    }

    public void CountVote(VoteList _list, string _answer)
    {
        // Return if Votings aren't open
        if (!VotingsOpen.Value)
            return;

        // Return if Answer is not valid
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

    private void OnApplicationQuit()
    {
        Votes.VotesList.Clear();
        IRC.IRC_Disconnect();
    }
}
