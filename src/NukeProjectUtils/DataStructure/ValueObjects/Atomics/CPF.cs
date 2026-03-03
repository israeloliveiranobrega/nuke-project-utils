using NukeProjectUtils.DataStructure.ValueObjects.Atomics.ErrorResources.Cpf;
using NukeProjectUtils.ExtensionMethods;
using NukeProjectUtils.Patterns.ResultPattern;
using System.Text.RegularExpressions;

namespace NukeProjectUtils.DataStructure.ValueObjects.Atomics;

public partial record Cpf
{
    public string RawValue { get; init; }

    public string BaseNumbers => RawValue[..9];
    public string CheckDigits => RawValue[^2..];
    public int RegionalCode => GetRegionalCode();
    public IEnumerable<string> TaxRegions => GetRegionsByRegionalCode();
    public string FormattedValue => FormatCpf();
    public string MaskedValue => MaskCpf();
     
    //Construtor para o EF
    private Cpf() { RawValue = null!; }

    #region CPF Creations

    private Cpf(string cpf) { RawValue = cpf; }

    public static Result<Cpf> Create(string cpf)
    {
        string cleanCpf = cpf.Trim() ?? string.Empty;

        if (!cleanCpf.HasContent())
        {
            var error = ErrorTrack.Create(CpfErros.CPF1001);
            return Result<Cpf>.Failure(error);
        }

        if (!cleanCpf.IsOnlyLettersOrNumbers(CheckType.OnlyNumbers))
        {
            var error = ErrorTrack.Create(CpfErros.CPF1002);
            return Result<Cpf>.Failure(error);
        }

        if (!cleanCpf.HasLength(11))
        {
            var error = ErrorTrack.Create(CpfErros.CPF1003);
            return Result<Cpf>.Failure(error);
        }

        if (IsKnownInvalidCpf(cleanCpf))
        {
            var error = ErrorTrack.Create(CpfErros.CPF2001);
            return Result<Cpf>.Failure(error);
        }

        if(!IsMathematicallyValidCpf(cleanCpf))
        {
            var error = ErrorTrack.Create(CpfErros.CPF2002);
            return Result<Cpf>.Failure(error);
        }

        return Result<Cpf>.Success(new(cleanCpf));
    }

    #endregion

    #region CPF formats

    private int GetRegionalCode()
    {
        int regionCode = (int)char.GetNumericValue(RawValue[8]);
        return regionCode;
    }

    //o 9 digito representa a região fiscal de onde o cpf reside, não representa de forma demografica, e o nono digito representa essa região
    private IEnumerable<string> GetRegionsByRegionalCode()
    {
        string[][] regions =
        [
            ["RS"],
            ["DF", "GO", "MS", "MT", "TO"],
            ["AC", "AM", "AP", "PA", "RO", "RR"],
            ["CE", "MA", "PI"],
            ["AL", "PB", "PE", "RN"],
            ["BA", "SE"],
            ["MG"],
            ["ES", "RJ"],
            ["SP"],
            ["PR", "SC"]
        ];

        IEnumerable<string> result = regions[RegionalCode];

        return result;
    }

    private string FormatCpf()
    {
        string formated = $"{RawValue[..3]}.{RawValue[3..6]}.{RawValue[6..9]}-{RawValue[9..11]}";
        return formated;
    }

    private string MaskCpf()
    {
        string masked = $"{RawValue[..3]}.***.***-{RawValue[^2..]}";
        return masked;
    }

    #endregion

    #region Private Tools

    //um cpf pode ser matematicamente valido porém ainda sim não ser de niguem
    private static bool IsMathematicallyValidCpf(string cpf)
    {
        ReadOnlySpan<char> baseSpan = cpf.AsSpan();

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

    [GeneratedRegex(@"(\d)\1{10}", RegexOptions.None, 100)]
    private static partial Regex RepeatedDigitsRegex();

    //cpfs podem ser matematicamente vaidos mas não aceitos, esses cpfs são os sequenciais, como ex: 11111111111, é matematicamente valido porem n é aceito como cpf
    private static bool IsKnownInvalidCpf(string cpf)
    {
        return RepeatedDigitsRegex().IsMatch(cpf);
    }

    #endregion
}