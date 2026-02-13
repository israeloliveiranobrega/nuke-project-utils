using NukeProjectUtils.ContainerTypes;
using NukeProjectUtils.Exceptions;
using System.Text.RegularExpressions;

namespace NukeProjectUtils.ValueObjects.Base;

/// <summary>
/// Represents an Email value object that encapsulates validation, verification logic, and masking capabilities.
/// </summary>
public record Email
{
    /// <summary>
    /// Gets the raw email address string.
    /// </summary>
    public string EmailAddress { get; init; }

    /// <summary>
    /// Gets a value indicating whether this email address has been successfully verified.
    /// </summary>
    public bool IsVerified { get; private set; }

    /// <summary>
    /// Internal verification token object used for the verification process.
    /// </summary>
    private VerificationToken? Verification { get; set; }

    /// <summary>
    /// Gets the local part of the email (the characters before the '@').
    /// </summary>
    public string LocalPart => EmailAddress.Split('@')[0];

    /// <summary>
    /// Gets the domain part of the email (the characters after the '@').
    /// </summary>
    public string Domain => EmailAddress.Split('@')[1];

    /// <summary>
    /// Gets a masked version of the email address for privacy display (e.g., "j***@domain.com").
    /// </summary>
    public string MaskedEmail
    {
        get
        {
            if (string.IsNullOrEmpty(LocalPart) || LocalPart.Length < 2)
                return EmailAddress;

            var asterisks = new string('*', LocalPart.Length - 1);
            return $"{LocalPart[0]}{asterisks}@{Domain}";
        }
    }

    /// <summary>
    /// Private constructor for ORM (Entity Framework) serialization purposes.
    /// </summary>
    private Email() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Email"/> class.
    /// </summary>
    /// <param name="emailAddress">The raw email address string.</param>
    /// <param name="verificationCode">Optional verification token. Defaults to null.</param>
    /// <exception cref="InvalidEmailFormatException">Thrown when the email format is invalid.</exception>
    public Email(string emailAddress, VerificationToken? verificationCode)
    {
        ValidateFormatting(emailAddress);

        EmailAddress = emailAddress;
        Verification = verificationCode;
    }

    /// <summary>
    /// Validates the format of the provided email string using Regex and structural checks.
    /// </summary>
    /// <param name="emailAddress">The email address to validate.</param>
    /// <exception cref="InvalidEmailFormatException">Thrown if the email structure is invalid.</exception>
    private static void ValidateFormatting(string emailAddress)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
            throw new InvalidEmailFormatException();

        string regexPattern = @"^[^@\s]+@[^@\s]+$";

        if (!Regex.IsMatch(emailAddress, regexPattern))
            throw new InvalidEmailFormatException();

        var parts = emailAddress.Split('@');

        if (parts.Length != 2 || string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]))
            throw new InvalidEmailFormatException();
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

        Verification = VerificationToken.Create(tokenType);
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

        var response = Verification.VerifyCode(code);

        if (!response.IsSuccess)
        {
            IsVerified = false;

            throw response.FailureType switch
            {
                VerificationFailure.CodeIsNull => new ArgumentNullException(nameof(code), "Verification code cannot be null."),
                VerificationFailure.CodeNotMatch => new EmailVerificationCodeMismatchException(),
                VerificationFailure.CodeExpired => new EmailVerificationCodeExpiredException(),
                _ => new InvalidOperationException($"Unknown verification failure: {response.FailureType}"),
            };
        }

        IsVerified = true;
    }
}