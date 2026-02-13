namespace NukeProjectUtils.ExtensionMethods;

public static class StringExtensionHelper
{
    public enum CheckType
    {
        OnlyLetters,
        OnlyNumbers,
        LettersOrNumbers
    }
    public static bool IsOnlyLettersOrNumbers(this string stringToCheck, CheckType checkType)
    {
        return checkType switch
        {
            CheckType.OnlyLetters => stringToCheck.All(c => char.IsLetter(c) || c == ' '),
            CheckType.OnlyNumbers => stringToCheck.All(char.IsDigit),
            CheckType.LettersOrNumbers => stringToCheck.All(char.IsLetterOrDigit),
            _ => false,
        };
    }

    public static bool HasContent(this string stringToCheck)
    {
        if(string.IsNullOrEmpty(stringToCheck))
            return false;
        
        return true;
    }

    public static bool HasLength(this string stringToCheck, int length)
    {
        if(stringToCheck.Length != length)
            return false;

        return true;
    }
    public static bool HasMinLength(this string stringToCheck, int minLength)
    {
        if(stringToCheck.Length < minLength)
            return false;

        return true;
    }
    public static bool InLengthRange(this string stringToCheck, int minLength, int maxLength)
    {
        if(stringToCheck.Length < minLength || stringToCheck.Length > maxLength)
            return false;

        return true;
    }
}
