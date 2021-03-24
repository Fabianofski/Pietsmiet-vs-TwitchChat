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

    [Header("Answer")]
    public Vector2 ValidRange;
    public string Answer;
}
