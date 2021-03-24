using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheckForValidAnswer
{
    
    public static bool CheckIfValid(string _message, Question question)
    {
        switch (question.questionType)
        {
            case Question.QuestionType.FloatGuess:
                // message is a valid vote, if message is a float and in allowed Range
                if (float.TryParse(_message, out float result))
                {
                    if (result >= question.ValidRange.x && result <= question.ValidRange.y)
                        return true;
                    return false;
                }
                return false;

            case Question.QuestionType.IntegerGuess:
                // message is a valid vote, if message is an integer and in allowed Range
                if (int.TryParse(_message, out int _result))
                {
                    if (_result >= question.ValidRange.x && _result <= question.ValidRange.y)
                        return true;
                    return false;
                }
                return false;

            case Question.QuestionType.StringGuess:
                // message always valid
                return true;
        }
        return false;
    }

}
