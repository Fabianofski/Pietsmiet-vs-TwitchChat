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
                // message is a valid vote, if message is a float and in allowed Range or in Valid Answers
                if (question.ValidVotes.Count > 0)
                    return question.ValidVotes.Contains(_message);

                if (float.TryParse(_message, out float result))
                {
                    if (result >= question.ValidRange.x && result <= question.ValidRange.y)
                        return true;
                    else if (question.ValidRange.x == 0 && question.ValidRange.y == 0)
                        return true;
                }
                return false;

            case Question.QuestionType.IntegerGuess:
                // message is a valid vote, if message is an integer and in allowed Range or in Valid Answers
                if (question.ValidVotes.Count > 0)
                    return question.ValidVotes.Contains(_message);

                if (int.TryParse(_message, out int _result))
                {
                    if (_result >= question.ValidRange.x && _result <= question.ValidRange.y)
                        return true;
                    else if (question.ValidRange.x == 0 && question.ValidRange.y == 0)
                        return true;
                }
                return false;

            case Question.QuestionType.StringGuess:
                if (question.ValidVotes.Count > 0)
                    return question.ValidVotes.Contains(_message);
                else if (question.ValidRange.x == 0 && question.ValidRange.y == 0)
                    return true;
                else if(_message.Length <= question.ValidRange.y && _message.Length >= question.ValidRange.x)
                    return true;

                return false;
        }
        return false;
    }

}
