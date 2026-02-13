using Microsoft.AspNetCore.Mvc.Infrastructure;
using NukeAuthentication.Shared.Exceptions;
using NukeProjectUtils.ExtensionMethods;
using static NukeProjectUtils.ExtensionMethods.StringExtensionHelper;

namespace NukeProjectUtils.ValueObjects.Base;

public record Phone
{
    public string RegionCode { get; init; }
    public string Number { get; init; }

    public ulong FullPhone => ulong.Parse($"{RegionCode}{Number}");
    public string UnformattedNumber => $"{RegionCode}{Number}";
    public string FormattedNumber => $"({RegionCode}) {UnformattedNumber[2]} {UnformattedNumber[3..7]}-{UnformattedNumber[7..11]}";
    public string MaskedPhone => $"({RegionCode}) * ****-**{FormattedNumber[^2..]}";

    public VerificationToken VerificationCode { get; init; }

    public bool PhoneVerified => VerificationCode.IsVerified;
    public string? PhoneVerificationCode => VerificationCode.Code;
    public DateTime? PhoneCodeExpiresOn => VerificationCode.ExpiresOn;

    private Phone() { }

    public Phone(string regionCode, string number, VerificationToken verificationCode)
    {
        ValidCountryCode(regionCode);
        ValidNumber(number);

        RegionCode = regionCode;
        Number = number;

        VerificationCode = verificationCode;
    }

    private static void ValidCountryCode(string countryCode)
    {
        if (!countryCode.HasContent())
            throw new ArgumentNullException(nameof(countryCode));

        if (!countryCode.IsOnlyLettersOrNumbers(CheckType.OnlyNumbers))
            throw new InvalidPhoneCountryCodeFormatExceptions();

        if (!countryCode.HasLength(2))
            throw new InvalidPhoneCountryCodeLengthExceptions();
    }
    private static void ValidNumber(string number)
    {
        if (!number.HasContent())
            throw new ArgumentNullException(nameof(number));

        if (!number.IsOnlyLettersOrNumbers(CheckType.OnlyNumbers))
            throw new InvalidPhoneCountryCodeFormatExceptions();

        if (!number.HasLength(9))
            throw new InvalidPhoneCountryCodeLengthExceptions();
    }
}
