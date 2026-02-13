using NukeProjectUtils.ContainerTypes;
using System.Security.Cryptography;

namespace NukeProjectUtils.ValueObjects.Base;

/// <summary>
/// Defines the specific format strategy for the verification token.
/// </summary>
public enum VerificationTokenType
{
    OneTimePassword = 0,
    AlphanumericCode = 1,
    UrlSafe = 2,
}

/// <summary>
/// Represents a Value Object that encapsulates the lifecycle, generation strategy, and validation logic 
/// of a verification token. It guarantees immutability and self-validation.
/// </summary>
public record VerificationToken
{
    public string Code { get; init; }
    public DateTime ExpiresOn { get; init; }
    public VerificationTokenType VerificationType { get; init; }

    /// <summary>
    /// Private constructor for ORM (Entity Framework) serialization purposes.
    /// </summary>
    private VerificationToken() { }

    private VerificationToken(string code, DateTime expiresOn, VerificationTokenType type)
    {
        Code = code;
        ExpiresOn = expiresOn;
        VerificationType = type;
    }

    /// <summary>
    /// Factory Method: Creates a valid <see cref="VerificationToken"/> instance based on the requested strategy.
    /// </summary>
    /// <param name="type">The desired format of the token (OTP, Alphanumeric, or URL Safe).</param>
    /// <returns>A fully hydrated <see cref="VerificationToken"/> with a generated code and a 6-minute expiration window.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the provided token type is not supported.</exception>
    public static VerificationToken Create(VerificationTokenType type)
    {
        return type switch
        {
            VerificationTokenType.OneTimePassword => GenerateOtp(),
            VerificationTokenType.AlphanumericCode => GenerateAlphanumeric(),
            VerificationTokenType.UrlSafe => GenerateUrlSafe(),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }

    /// <summary>
    /// Performs domain-specific validation of the input code against the stored token.
    /// </summary>
    /// <param name="inputCode">The raw string provided by the user for verification.</param>
    /// <returns>
    /// A <see cref="VerificationResult{Boolean}"/> that contains:
    /// <list type="bullet">
    /// <item>Success(true) if the code matches and is valid.</item>
    /// <item>Failure(CodeIsNull, CodeNotMatch, or CodeExpired) depending on the violation.</item>
    /// </list>
    /// </returns>
    public VerificationResult<bool> VerifyCode(string inputCode)
    {
        if (string.IsNullOrWhiteSpace(inputCode))
            return VerificationResult<bool>.Failure(VerificationFailure.CodeIsNull);

        if (!Code.Equals(inputCode, StringComparison.Ordinal))
            return VerificationResult<bool>.Failure(VerificationFailure.CodeNotMatch);

        var now = DateTime.UtcNow;

        if (ExpiresOn <= now)
            return VerificationResult<bool>.Failure(VerificationFailure.CodeExpired);

        return VerificationResult<bool>.Success(true);
    }

    private static VerificationToken GenerateOtp()
    {
        var number = RandomNumberGenerator.GetInt32(11111111, 99999999);
        return new VerificationToken(number.ToString(), DateTime.UtcNow.AddMinutes(6), VerificationTokenType.OneTimePassword);
    }

    private static VerificationToken GenerateAlphanumeric()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        var token = RandomNumberGenerator.GetItems<char>(chars.ToCharArray(), 8);
        return new VerificationToken(new string(token), DateTime.UtcNow.AddMinutes(6), VerificationTokenType.AlphanumericCode);
    }

    private static VerificationToken GenerateUrlSafe()
    {
        // Uses the "N" format (32 digits) to create a clean string without hyphens or braces.
        return new VerificationToken(Guid.NewGuid().ToString("N"), DateTime.UtcNow.AddMinutes(6), VerificationTokenType.UrlSafe);
    }
}
// as vezes eu penso..."será que eu to fazendo a coisa certa?", mas logo em seguida que vejo seu soriso, eu sei que estou fazendo <3