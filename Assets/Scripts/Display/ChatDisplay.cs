using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine.UI;

public class ChatDisplay : MonoBehaviour
{
    [Header("General")]
    [SerializeField] VoteList VoteList;
    [SerializeField] BoolReference VotingsOpen;
    [SerializeField] AtomList Questions;
    [SerializeField] IntReference currentQuestionIndex;

    public enum DisplayType { Average, Barchart, Top4}
    [Header("Display")]
    public DisplayType displayType;
    private DisplayType oldDisplayType = DisplayType.Barchart;

    [Header("Average")]
    [SerializeField] GameObject AverageParent;
    [SerializeField] TextMeshProUGUI AverageValueText;

    [System.Serializable]
    public class Bar
    {
        public RectTransform rect;
        public TextMeshProUGUI value;
        public TextMeshProUGUI desc;
    }

    [Header("Bar Chart")]
    [SerializeField] GameObject BarChartParent;
    [SerializeField] float defaultWidth;
    [SerializeField] Bar[] bars;

    [Header("Top4")]
    [SerializeField] GameObject Top4Parent;
    [SerializeField] TextMeshProUGUI[] leaderboard;

    [Header("Points")]
    [SerializeField] TextMeshProUGUI ScoreText;

    private void Update()
    {
        if (VotingsOpen.Value)
            UpdateVotings();

        if (displayType != oldDisplayType)
            UpdateVotings();

        oldDisplayType = displayType;
    }

    public void ChangeDisplayType()
    {
        Question currentQuestion = (Question)Questions.Value[currentQuestionIndex.Value];
        displayType = currentQuestion.displayType;
    }

    public void UpdateVotings()
    {
        switch (displayType)
        {
            case DisplayType.Average: Average(); break;
            case DisplayType.Barchart: BarChart(); break;
            case DisplayType.Top4: Top4(); break;
        }

        AverageParent.SetActive(displayType == DisplayType.Average);
        BarChartParent.SetActive(displayType == DisplayType.Barchart);
        Top4Parent.SetActive(displayType == DisplayType.Top4);

    }

    void Average()
    {
        float sum = 0;
        float VoteAmount = 0;
        foreach(Vote vote in VoteList.VotesList)
        {
            VoteAmount += vote.amount;
            if (float.TryParse(vote.message, out float result))
                sum += result * vote.amount;
            else
            {
                AverageValueText.text = "NaN";
                return;
            }
        }
        float average;
        if (VoteAmount != 0)
            average = sum / VoteAmount;
        else
            average = 0;

        Question currentQuestion = (Question)Questions.Value[currentQuestionIndex.Value];
        if (currentQuestion.questionType == Question.QuestionType.FloatGuess)
            average = Mathf.Round(average * 100) / 100;
        else
            average = Mathf.RoundToInt(average);

        AverageValueText.text = average.ToString();

    }

    void BarChart()
    {
        float TotalVotes = 0;
        foreach (Vote vote in VoteList.VotesList)
        {
            TotalVotes += vote.amount;
        }

        for (int i = 0; i < Mathf.Min(VoteList.VotesList.Count, bars.Length); i++)
        {
            float percentage = 0;
            if (TotalVotes != 0)
                percentage = VoteList.VotesList[i].amount / TotalVotes;

            bars[i].rect.sizeDelta = new Vector2(percentage * defaultWidth, bars[i].rect.sizeDelta.y);
            percentage = Mathf.Round(percentage * 1000)/10;

            bars[i].value.text = $"{percentage}% ({VoteList.VotesList[i].amount})";
            bars[i].desc.text = $"{VoteList.VotesList[i].message}.:";
        }       
    }

    void Top4()
    {
        List<Vote> votes = VoteList.VotesList;
        votes.Sort(SortbyVotes);

        float TotalVotes = 0;
        foreach (Vote vote in VoteList.VotesList)
        {
            TotalVotes += vote.amount;
        }

        for (int i = 0; i < Mathf.Min(votes.Count, leaderboard.Length); i++) 
        {
            float percentage = votes[i].amount / TotalVotes * 100;
            percentage = Mathf.Round(percentage * 10)/10;

            leaderboard[i].text = $"{votes[i].message} {percentage}% ({votes[i].amount})";
        }
    }

    public void UpdatePoints(int _points)
    {
        ScoreText.text = $"{_points}";
    }
    public static int SortbyVotes(Vote vote1, Vote vote2)
    {
        return vote2.amount.CompareTo(vote1.amount);
    }
}
