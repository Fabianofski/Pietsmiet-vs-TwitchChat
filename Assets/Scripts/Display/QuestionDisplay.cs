using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using TMPro;

public class QuestionDisplay : MonoBehaviour
{


    [SerializeField] AtomList Questions;
    [SerializeField] TextMeshProUGUI Title;
    [SerializeField] TextMeshProUGUI Answer;

    public void DisplayQuestion(int _currentQuestionIndex)
    {
        Question _currentQuestion = (Question)Questions.Value[_currentQuestionIndex];
        Title.text = _currentQuestion.Title;

        string _answer = "";
        for (int i = 0; i < _currentQuestion.AnswerDisplayAmount; i++)
            _answer += _currentQuestion.Answer[i] + ", ";
        _answer = _answer.Substring(0, _answer.Length - 2);

        Answer.text = _answer;
    }

}
