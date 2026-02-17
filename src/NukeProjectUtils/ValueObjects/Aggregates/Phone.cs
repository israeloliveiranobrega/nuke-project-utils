using NukeProjectUtils.ContainerTypes;
using NukeProjectUtils.Exceptions;
using NukeProjectUtils.ExtensionMethods;
using NukeProjectUtils.Internal;
using NukeProjectUtils.ValueObjects.Atomics;
using PhoneNumbers;
using static NukeProjectUtils.ExtensionMethods.StringExtensionHelper;

namespace NukeProjectUtils.ValueObjects.Aggregates;

public record Phone
{
    private readonly PhonesServices _phonesServices = PhonesServices.Instance;
    private PhoneNumber Number {  get; init; }

    public string PhoneNumber { get; init; }

    public bool IsVerified { get; private set; }
    private ValidationToken? Verification { get; set; }

    public string CountryCode => $"{Number.CountryCode}";
    public string AreaCode
    {
        get
        {
            string nationalSignificantNumber = _phonesServices._phoneUtil.GetNationalSignificantNumber(Number);
            int lengthOfAreaCode = _phonesServices._phoneUtil.GetLengthOfNationalDestinationCode(Number);

            return nationalSignificantNumber[lengthOfAreaCode..];
        }
    }
    public string SubscriberNumber => $"{Number.NationalNumber}";


    private Phone() { }

    public Phone(string phoneNumber, ValidationToken? verification)
    {
        ValidNumber(phoneNumber);

        PhoneNumber = phoneNumber;

        Number = _phonesServices._phoneUtil.Parse(phoneNumber,"");

        Verification = verification;
    }
    private static void ValidNumber(string number)
    {
        if (!number.HasContent())
            throw new ArgumentNullException(nameof(number));

        if (!number.IsOnlyLettersOrNumbers(CheckType.OnlyNumbers))
            throw new InvalidPhoneCountryCodeFormatExceptions();

        if (!number.HasMinLength(16))
            throw new InvalidPhoneCountryCodeLengthExceptions();
    }
    /// <summary>
    /// Generates a new verification token.
    /// Logic: You can only generate a new token if the previous one has expired or does not exist.
    /// </summary>
    /// <param name="tokenType">The type of token to generate (OTP, Alpha, etc).</param>
    /// <exception cref="EmailVerificationCodeAlreadyExistsException">Thrown if a valid (non-expired) token already exists.</exception>
    public void GenerateVerificationToken(VerificationTokenType tokenType)
    {
        if (Verification != null)
        {
            var now = DateTime.UtcNow;
            if (Verification.ExpiresOn > now)
            {
                throw new EmailVerificationCodeAlreadyExistsException();
            }
        }

        Verification = ValidationToken.Create(tokenType);
        IsVerified = false;
    }

    /// <summary>
    /// Attempts to verify the email using the provided code.
    /// </summary>
    /// <param name="code">The code provided by the user.</param>
    /// <exception cref="EmailVerificationCodeNotFoundException">Thrown if verification has not been initiated.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the provided code is null.</exception>
    /// <exception cref="EmailVerificationCodeNotMatchException">Thrown if the code does not match.</exception>
    /// <exception cref="EmailVerificationCodeExpiredException">Thrown if the token has expired.</exception>
    public void VerifyEmail(string code)
    {
        if (Verification is null)
            throw new EmailVerificationCodeNotFoundException();

        var response = Verification.VerifyToken(code);

        if (!response.IsSuccess)
        {
            IsVerified = false;

            throw response.Failure switch
            {
                VerificationFailure.CodeIsNull => new ArgumentNullException(nameof(code), "Verification code cannot be null."),
                VerificationFailure.CodeNotMatch => new EmailVerificationCodeMismatchException(),
                VerificationFailure.CodeExpired => new EmailVerificationCodeExpiredException(),
                _ => new InvalidOperationException($"Unknown verification failure: {response.Failure}"),
            };
        }

        IsVerified = true;
    }
}

