using UnityEngine;

public static class NumberFormatter
{
    public static string ClearFormattingFromString(string _message)
    {
        string _clearedMessage = _message.ToUpper();

        // Formatting
        _clearedMessage = _clearedMessage.Replace(" ", "");
        _clearedMessage = _clearedMessage.Replace(".", "");

        // Tausend
        _clearedMessage = _clearedMessage.Replace("TAUSEND", "e3");
        _clearedMessage = _clearedMessage.Replace("MILLE", "e3");
        _clearedMessage = _clearedMessage.Replace("TSD", "e3");
        _clearedMessage = _clearedMessage.Replace("K", "e3");

        // Million
        _clearedMessage = _clearedMessage.Replace("MILLIONEN", "e6");
        _clearedMessage = _clearedMessage.Replace("MILLION", "e6");
        _clearedMessage = _clearedMessage.Replace("MIO", "e6");

        // Milliarde
        _clearedMessage = _clearedMessage.Replace("MILLIARDEN", "e9");
        _clearedMessage = _clearedMessage.Replace("MILLIARDE", "e9");
        _clearedMessage = _clearedMessage.Replace("MILL", "e9");
        _clearedMessage = _clearedMessage.Replace("MRD", "e9");

        // Billion 
        _clearedMessage = _clearedMessage.Replace("BILLIONEN", "e12");
        _clearedMessage = _clearedMessage.Replace("BILLION", "e12");
        _clearedMessage = _clearedMessage.Replace("BILL", "e12");
        _clearedMessage = _clearedMessage.Replace("BIO", "e12");

        // Convert Float to Format, which Integers support
        _clearedMessage = float.Parse(_clearedMessage) + "";

        return _clearedMessage;
    }

    public static string AddFormattingToNumber(int _number)
    {
        string _message = _number.ToString("n2");
        _message = AddFormattingToString(_message);

        return _message;
    }
    
    public static string AddFormattingToNumber(float _number)
    {
        string _message = _number.ToString("n2");
        _message = AddFormattingToString(_message);

        return _message;
    }

    public static string AddFormattingToNumber(string _number)
    {
        if (float.TryParse(_number, out float _parsedFloatNumber))
            return AddFormattingToNumber(_parsedFloatNumber);
        else if (int.TryParse(_number, out int _parsedIntNumber))
            return AddFormattingToNumber(_parsedIntNumber);
        else
            return _number;
    }

    private static string AddFormattingToString(string _message)
    {
        float _parsed = float.Parse(_message);

        if (_message.Length >= 19) // 5 (.000.000.000.000,00) -> 19 
        {
            _parsed = _parsed / 1e12f;
            _message = _parsed.ToString("0.##") + " bio.";
            return _message;
        }
        else if (_message.Length >= 15) // 5 (.000.000.000,00) -> 15 
        {
            _parsed = _parsed / 1e9f;
            _message = _parsed.ToString("0.##") + " mill.";
            return _message;
        }
        else if (_message.Length >= 11) // 5 (.000.000,00) -> 11 
        {
            _parsed = _parsed / 1e6f;
            _message = _parsed.ToString("0.##") + " mio.";
            return _message;
        }
        else if (_message.Length >= 7) // 5 (.000,00) -> 7 
        {
            _parsed = _parsed / 1e3f;
            _message = _parsed.ToString("0.##") + " tsd.";
            return _message;
        }
        else
            return _message;
    }

}
