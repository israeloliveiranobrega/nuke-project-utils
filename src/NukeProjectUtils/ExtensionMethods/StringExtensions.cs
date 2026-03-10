using System.Globalization;
using System.Text;

namespace NukeProjectUtils.ExtensionMethods;

/// <summary>
/// Provides extension methods for string manipulation and validation.
/// </summary>
public static class StringExtensions
{
    #region Verifiers

    /// <summary>
    /// Validates if the string exactly matches the specified length.
    /// </summary>
    /// <param name="str">The string to validate.</param>
    /// <param name="length">The exact required length.</param>
    /// <returns>True if the string meets the length requirement; otherwise, false.</returns>
    public static bool HasLength(this string str, int length)
    {
        if (str.Length != length)
            return false;

        return true;
    }

    /// <summary>
    /// Validates if the string length falls within the specified minimum and maximum boundaries.
    /// </summary>
    /// <param name="str">The string to validate.</param>
    /// <param name="minLength">The minimum allowed length.</param>
    /// <param name="maxLength">The maximum allowed length.</param>
    /// <returns>True if the string length is within range; otherwise, false.</returns>
    public static bool InLengthRange(this string str, int minLength, int maxLength)
    {
        if (str.Length < minLength || str.Length > maxLength)
            return false;

        return true;
    }

    /// <summary>
    /// Validates if the string meets a minimum length requirement.
    /// </summary>
    /// <param name="str">The string to validate.</param>
    /// <param name="minLength">The minimum required length.</param>
    /// <returns>True if the string meets or exceeds the minimum length; otherwise, false.</returns>
    public static bool HasMinLength(this string str, int minLength)
    {
        if (str.Length < minLength)
            return false;

        return true;
    }

    /// <summary>
    /// Validates if the string exceeds a maximum length constraint.
    /// </summary>
    /// <param name="str">The string to validate.</param>
    /// <param name="MaxLength">The maximum allowed length.</param>
    /// <returns>True if the string exceeds the maximum length; otherwise, false.</returns>
    public static bool ExceedsMaxLength(this string str, int MaxLength)
    {
        if (str.Length > MaxLength)
            return true;

        return false;
    }

    /// <summary>
    /// Validates if the string contains any non-whitespace characters.
    /// </summary>
    /// <param name="str">The string to validate.</param>
    /// <returns>True if the string has substantive content; otherwise, false.</returns>
    public static bool HasContent(this string str)
    {
        if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            return false;

        return true;
    }

    /// <summary>
    /// Validates if the string consists entirely of alphabetical characters and spaces.
    /// </summary>
    /// <param name="str">The string to validate.</param>
    /// <returns>True if only letters and spaces are present; otherwise, false.</returns>
    public static bool IsOnlyLetters(this string str)
    {
        return str.All(c => char.IsLetter(c) || c == ' ');
    }

    /// <summary>
    /// Validates if the string contains any accented characters.
    /// </summary>
    /// <param name="str">The string to validate.</param>
    /// <returns>True if accents are detected; otherwise, false.</returns>
    public static bool HasAccents(this string str)
    {
        string normalized = str.Normalize(NormalizationForm.FormD);

        bool result = normalized.Any(c => CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark);

        return result;
    }

    /// <summary>
    /// Validates if the string contains at least one numeric digit.
    /// </summary>
    /// <param name="str">The string to validate.</param>
    /// <returns>True if a number is present; otherwise, false.</returns>
    public static bool HasNumbers(this string str)
    {
        return str.Any(char.IsDigit);
    }

    /// <summary>
    /// Validates if the string consists entirely of numeric digits.
    /// </summary>
    /// <param name="str">The string to validate.</param>
    /// <returns>True if only digits are present; otherwise, false.</returns>
    public static bool IsOnlyNumbers(this string str)
    {
        return str.All(char.IsDigit);
    }

    /// <summary>
    /// Validates if the string contains any characters that are not letters, digits, or whitespace.
    /// </summary>
    /// <param name="str">The string to validate.</param>
    /// <returns>True if special characters are present; otherwise, false.</returns>
    public static bool HasSpecialCharacters(this string str)
    {
        foreach (var c in str)
        {
            if (!char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c))
                return true;
        }

        return false;
    }

    #endregion

    #region Extractors

    /// <summary>
    /// Extracts all numeric digits from the string.
    /// </summary>
    /// <param name="str">The source string.</param>
    /// <returns>A new string containing only the extracted digits.</returns>
    public static string ExtractNumbers(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        List<char> result = new List<char>();

        foreach (char c in str)
        {
            if (char.IsDigit(c))
            {
                result.Add(c);
            }
        }

        return new string(result.ToArray());
    }

    /// <summary>
    /// Extracts non-alphanumeric characters based on the existing algorithm.
    /// </summary>
    /// <param name="str">The source string.</param>
    /// <param name="removeSpaces">Indicates whether spaces should be excluded from the extraction.</param>
    /// <returns>A new string resulting from the extraction logic.</returns>
    public static string ExtractLetters(this string str, bool removeSpaces = false)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        List<char> result = new List<char>();

        foreach (char c in str)
        {
            if (!char.IsLetterOrDigit(c))
            {
                if (removeSpaces && c == ' ')
                    continue;

                result.Add(c);
            }
        }

        return new string(result.ToArray());
    }

    /// <summary>
    /// Extracts special characters from the string.
    /// </summary>
    /// <param name="str">The source string.</param>
    /// <param name="removeSpaces">Indicates whether spaces should be excluded from the extraction.</param>
    /// <returns>A new string containing the extracted characters.</returns>
    public static string ExtractSpecialCharacters(this string str, bool removeSpaces = false)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        List<char> result = new List<char>();

        foreach (char c in str)
        {
            if (!char.IsLetterOrDigit(c))
            {
                if (removeSpaces && c == ' ')
                    continue;

                result.Add(c);
            }
        }

        return new string(result.ToArray());
    }

    #endregion

    #region Removers

    /// <summary>
    /// Removes all occurrences of a specific substring from the current string.
    /// </summary>
    /// <param name="str">The source string.</param>
    /// <param name="targetRemove">The exact substring to remove.</param>
    /// <returns>A new string with the target sequence removed.</returns>
    public static string TargetRemove(this string str, string targetRemove)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        string result = str.Replace(targetRemove, "");

        return result;
    }

    /// <summary>
    /// Removes all numeric digits from the string.
    /// </summary>
    /// <param name="str">The source string.</param>
    /// <returns>A new string containing no numeric digits.</returns>
    public static string RemoveNumbers(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        List<char> result = new List<char>();

        foreach (char c in str)
        {
            if (!char.IsDigit(c))
            {
                result.Add(c);
            }
        }

        return new string(result.ToArray());
    }

    /// <summary>
    /// Removes all alphabetical characters from the string.
    /// </summary>
    /// <param name="str">The source string.</param>
    /// <returns>A new string containing no alphabetical letters.</returns>
    public static string RemoveLetters(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        List<char> result = new List<char>();

        foreach (char c in str)
        {
            if (!char.IsLetter(c))
            {
                result.Add(c);
            }
        }

        return new string(result.ToArray());
    }

    /// <summary>
    /// Removes special characters based on the existing exclusion algorithm.
    /// </summary>
    /// <param name="str">The source string.</param>
    /// <param name="removeSpaces">Indicates whether spaces should be excluded from the operation.</param>
    /// <returns>A new string processed by the removal logic.</returns>
    public static string RemoveSpecialCharacters(this string str, bool removeSpaces = false)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        List<char> result = new List<char>();

        foreach (char c in str)
        {
            if (!char.IsLetterOrDigit(c))
            {
                if (removeSpaces && c == ' ')
                    continue;

                result.Add(c);
            }
        }

        return new string(result.ToArray());
    }

    #endregion
}