using System.Globalization;
using System.Text;

namespace NukeProjectUtils.ExtensionMethods;

public static class StringExtensions
{
    public static bool HasLength(this string str, int length)
    {
        if(str.Length != length)
            return false;

        return true;
    }

    public static bool HasMinLength(this string str, int minLength)
    {
        if(str.Length < minLength)
            return false;

        return true;
    }

    public static bool ExceedsMaxLength(this string str, int MaxLength)
    {
        if(str.Length > MaxLength)
            return true;

        return false;
    }

    public static bool InLengthRange(this string str, int minLength, int maxLength)
    {
        if(str.Length < minLength || str.Length > maxLength)
            return false;

        return true;
    }

    #region Verificadores

    //verifica se não é nulo ou vazio
    public static bool HasContent(this string stringToCheck)
    {
        if (string.IsNullOrEmpty(stringToCheck) || string.IsNullOrWhiteSpace(stringToCheck))
            return false;

        return true;
    }

    //verifica se tudo é letras 
    public static bool IsOnlyLetters(this string stringToCheck)
    {
        return stringToCheck.All(c => char.IsLetter(c) || c == ' ');
    }

    //verifica se tem letras com acentos
    public static bool HasAccents(this string str)
    {
        string normalized = str.Normalize(NormalizationForm.FormD);

        bool reult = normalized.Any(c => CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark);

        return reult;
    }

    //verifica de tem numeros
    public static bool HasNumbers(this string str)
    {
        return str.Any(char.IsDigit);
    }

    //verifica se tudo é numero
    public static bool IsOnlyNumbers(this string stringToCheck)
    {
        return stringToCheck.All(char.IsDigit);
    }

    //verifica se tem caracteres especiais
    public static bool HasSpecialCharacters(this string stringToCheck)
    {
        if (string.IsNullOrEmpty(stringToCheck) || string.IsNullOrWhiteSpace(stringToCheck))
            return false;

        return true;
    }

    #endregion

    #region Extratores

    //retorna apenas os numeros da string passada, se não tiver nenhum, retorna ""
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

    //retorna apenas as letras (escolher com e sem acentos) da string passada, se não tiver nenhum, retorna ""
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

    //retorna apenas os caracteres especiais da string passada, se não tiver nenhum, retorna ""
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

    #region Removedores

    //remove o que for passado como parametro
    public static string TargedRemove(this string str, string targedRemove)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        string result = str.Replace(targedRemove, "");

        return result;
    }

    //remove todos os numeros
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

    //remove todas as letras
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

    //remove todos os caracteres especiais
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
