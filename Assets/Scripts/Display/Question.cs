using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Question", menuName = "Quiz/new Question")]
public class Question : UnityAtoms.AtomBaseVariable<object>
{

    public enum QuestionType{ IntegerGuess, FloatGuess, StringGuess}
    public enum AnswerType { Exact, Closest}

    [Header("Question Settings")]
    public QuestionType questionType;
    public ChatDisplay.DisplayType displayType;
    public AnswerType answerType;

    [Header("Question")]
    public string Title;

    [Header("Voting Options")]
    public List<string> StringOptions;
    public List<Sprite> ImgOptions;

    [Header("Voting")]
    [Tooltip("Limits Integer/ Float Votes in this Range or the character Length of a Vote, If x,y = 0 Range is disabled, ")]
    public Vector2 ValidRange;
    [Tooltip("Only allow certain Answers. Leave empty if many Answers")]
    public List<string> ValidVotes;

    [Header("Answer")]
    [Tooltip("Amount of Answers that should be displayed. Displays the first - AnswerDisplayAmount Answers")]
    [Range(1, 5)]
    public int AnswerDisplayAmount = 1;
    public List<string> Answer;

}
