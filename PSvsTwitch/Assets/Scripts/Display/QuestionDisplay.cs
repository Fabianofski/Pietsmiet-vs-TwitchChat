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
        Answer.text = _currentQuestion.Answer;
    }

}
