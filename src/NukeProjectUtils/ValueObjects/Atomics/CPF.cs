using NukeProjectUtils.ContainerTypes;
using NukeProjectUtils.ContainerTypes.ErrorEnums;
using NukeProjectUtils.ExtensionMethods;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace NukeProjectUtils.ValueObjects.Atomics;

public partial record CPF
{
    [GeneratedRegex(@"(\d)\1{10}", RegexOptions.None, 100)]
    private static partial Regex RepeatedDigitsRegex();

    public string Value { get; init; }

    #region UFs
    private static readonly string[] Region0 = ["RS"];
    private static readonly string[] Region1 = ["DF", "GO", "MS", "MT", "TO"];
    private static readonly string[] Region2 = ["AC", "AM", "AP", "PA", "RO", "RR"];
    private static readonly string[] Region3 = ["CE", "MA", "PI"];
    private static readonly string[] Region4 = ["AL", "PB", "PE", "RN"];
    private static readonly string[] Region5 = ["BA", "SE"];
    private static readonly string[] Region6 = ["MG"];
    private static readonly string[] Region7 = ["ES", "RJ"];
    private static readonly string[] Region8 = ["SP"];
    private static readonly string[] Region9 = ["PR", "SC"];
    #endregion
    public char UF => Value[8];

    public string BaseNumbers => Value[..9];
    public string CheckDigits => Value[^2..];

    public IReadOnlyList<string> StateUnits => GetStatesByRegionalCode(UF);

    public string FormattedCpf => $"{Value[..3]}.{Value[3..6]}.{Value[6..9]}-{Value[9..11]}";

    public string MaskedCpf => $"{Value[..3]}.***.***-{Value[^2..]}";

    private CPF() { Value = null!; }

    private CPF(string cpf) => Value = cpf;

    public static Result<CPF, ErrorTrack> Create(string cpf)
    {
        string cleanCpf = cpf.Trim() ?? string.Empty;

        if (!cleanCpf.HasContent())
        {
            var error = ErrorTrack.Create(nameof(CPF),CpfCreateError.NullOrEmpty);
            return Result<CPF, ErrorTrack>.Fail(error);
        }

        if (!cleanCpf.IsOnlyLettersOrNumbers(CheckType.OnlyNumbers))
        {
            var error = ErrorTrack.Create(nameof(CPF), CpfCreateError.InvalidFormat);
            return Result<CPF, ErrorTrack>.Fail(error);
        }

        if (!cleanCpf.HasLength(11))
        {
            var error = ErrorTrack.Create(nameof(CPF), CpfCreateError.NotInTheRange);
            return Result<CPF, ErrorTrack>.Fail(error);
        }

        if (IsKnownInvalidCpf(cleanCpf))
        {
            var error = ErrorTrack.Create(nameof(CPF), CpfCreateError.KnowInvalidCpf);
            return Result<CPF, ErrorTrack>.Fail(error);
        }

        if(!IsValidCpf(cleanCpf))
        {
            var error = ErrorTrack.Create(nameof(CPF), CpfCreateError.MathematicallyInvalid);
            return Result<CPF, ErrorTrack>.Fail(error);
        }

        return Result<CPF, ErrorTrack>.Success(new(cleanCpf));
    }

    private static string[] GetStatesByRegionalCode(char digit)
    {
        return digit switch
        {
            '0' => Region0,
            '1' => Region1,
            '2' => Region2,
            '3' => Region3,
            '4' => Region4,
            '5' => Region5,
            '6' => Region6,
            '7' => Region7,
            '8' => Region8,
            '9' => Region9,
            _ => []
        };
    }
    private static bool IsValidCpf(string cpf)
    {
        var baseSpan = cpf.AsSpan();

        int firstDigit = CalculateCheckDigit(baseSpan[..9]);
        int secondDigit = CalculateCheckDigit(baseSpan[..10]);

        return cpf[9] - '0' == firstDigit && cpf[10] - '0' == secondDigit;
    }
    private static int CalculateCheckDigit(ReadOnlySpan<char> source)
    {
        int sum = 0;
        int multiplier = source.Length + 1;

        foreach (var c in source)
        {
            int digit = c - '0';
            sum += digit * multiplier--;
        }

        int remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }
    private static bool IsKnownInvalidCpf(string cpf) => RepeatedDigitsRegex().IsMatch(cpf);


    public static implicit operator string(CPF cpf) => cpf.Value;
}
z