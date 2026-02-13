using System.Text.RegularExpressions;

namespace NukeProjectUtils.ValueObjects.Base;

public record CPF
{
    public string Numbers { get; init; }
    public string Validators { get; init; }  
    public string UnformattedCpf { get; init; }
    public string FormattedCpf => $"{UnformattedCpf[..3]}.{UnformattedCpf[3..6]}.{UnformattedCpf[6..9]}-{UnformattedCpf[9..11]}";
    public string MaskedCpf => $"{FormattedCpf.Split('.')[0]}.XXX.XXX-{FormattedCpf.Split('-')[1]}";

    public bool IsVerified { get; init; }
    private CPF() { }

    public CPF(string cpf, bool isVerified)
    {
        CheckCpfInput(cpf);
        ValidCpf(cpf);

        Numbers = cpf[..9];
        Validators = cpf[^2..];

        UnformattedCpf = $"{Numbers}{Validators}";

        IsVerified = isVerified;
    }

    #region Validator
    private static void CheckCpfInput(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentNullException(nameof(cpf),"O Cpf não pode ser vazio.");

        if (!CheckIsNumbers(cpf) || !CheckLength(cpf))
            throw new InvalidCpfFormatException();

        if (CheckKnowWrongCpfs(cpf))
            throw new KnowInvalidCpfException();
    }
    private static void ValidCpf(string cpf)
    {
        int[] firstValidation = ConvertCpfToIntArray(cpf[..9]);

        int firstResult = CalculeValidator(firstValidation);

        int[] secondValidation = ConvertCpfToIntArray($"{cpf[..9]}{firstResult}");

        int secondResult = CalculeValidator(secondValidation);

        string result = $"{firstResult}{secondResult}";

        if (!(result == cpf[^2..]))
            throw new InvalidCpfException();
    }
    #endregion

    #region Tools
    private static int[] ConvertCpfToIntArray(string cpf) => [.. cpf.Select(c => int.Parse(c.ToString()))];
    private static int CalculeValidator(int[] numbers)
    {
        int sum = 0;

        int multiplier = numbers.Length + 1;

        for (int i = 0; i < numbers.Length; i++)
            sum += numbers[i] * (multiplier - i);

        var result = (sum * 10) % 11;

        if (result >= 10)
            return 0;

        return result;
    }
    private static bool CheckKnowWrongCpfs(string cpf)
    {
        string regex = @"(\d)\1{10}";

        if (Regex.IsMatch(cpf, regex))
            return true;

        return false;
    }
    private static bool CheckIsNumbers(string numbers) => numbers.All(char.IsAsciiDigit);
    private static bool CheckLength(string numbers) => numbers.Length == 11;

    #endregion
}
