namespace NukeProjectUtils.ExtensionMethods;

public enum CheckType
{
    OnlyLetters,
    OnlyNumbers,
    LettersOrNumbers
}

public static class StringExtensions
{
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
        if(string.IsNullOrEmpty(stringToCheck) || string.IsNullOrWhiteSpace(stringToCheck))
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
    public static bool ExceedsMaxLength(this string stringToCheck, int MaxLength)
    {
        if(stringToCheck.Length > MaxLength)
            return true;

        return false;
    }
    public static bool InLengthRange(this string stringToCheck, int minLength, int maxLength)
    {
        if(stringToCheck.Length < minLength || stringToCheck.Length > maxLength)
            return false;

        return true;
    }
}
