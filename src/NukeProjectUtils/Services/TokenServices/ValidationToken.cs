/*
O medo me consome a cada dia que passa, medo de ser medíocre, medo de perder quem amo,
medo de ser um fardo, mas olhar aquele sorriso, assim como um farol em uma noite nublado,
aquele sorriso ilumina minha vida
*/

using Microsoft.Extensions.Options;
using NukeProjectUtils.DataStructure.OptionsObjects;
using System.Security.Cryptography;
using System.Text;

namespace NukeProjectUtils.Services.TokenServices;

/// <summary>
/// Represents a temporal verification token utilized for multi-factor authentication, 
/// URL-safe links, or session renewal.
/// </summary>
public record ValidationToken
{
    private static VerificationTokenOptions Options = new();

    /// <summary>
    /// The raw cryptographic or alphanumeric value of the token.
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// The exact UTC moment when this token ceases to be valid.
    /// </summary>
    public DateTime ExpiresOn { get; init; }

    /// <summary>
    /// The business domain classification of the token.
    /// </summary>
    public ValidationTokenType VerificationType { get; init; }

    /// <summary>
    /// Injects global token expiration parameters.
    /// </summary>
    /// <param name="options">Configuration options defining token lifespans.</param>
    public static void Configure(IOptions<VerificationTokenOptions> options)
    {
        Options = options.Value;
    }

    /// <summary>
    /// Parameterless constructor required for Entity Framework Core materialization.
    /// </summary>
    private ValidationToken() { Value = null!; }

    #region Token Creation

    /// <summary>
    /// Instantiates a defined token with its calculated expiration and classification.
    /// </summary>
    /// <param name="token">The generated token value.</param>
    /// <param name="expiresOn">The UTC expiration timestamp.</param>
    /// <param name="type">The domain classification.</param>
    private ValidationToken(string token, DateTime expiresOn, ValidationTokenType type)
    {
        Value = token;
        ExpiresOn = expiresOn;
        VerificationType = type;
    }

    /// <summary>
    /// Factory method to generate a specific token type based on predefined strategies.
    /// </summary>
    /// <param name="type">The required token generation strategy.</param>
    /// <param name="expiresOn">Optional override for the token expiration.</param>
    /// <returns>A materialized token ready for distribution.</returns>
    /// <exception cref="InvalidOperationException">Thrown when an unsupported token type is requested.</exception>
    public static ValidationToken Create(ValidationTokenType type, DateTime? expiresOn = null)
    {
        return type switch
        {
            ValidationTokenType.OneTimePassword => GenerateOtp(),
            ValidationTokenType.AlphanumericCode => GenerateAlphanumeric(),
            ValidationTokenType.UrlSafe => GenerateUrlSafe(),
            ValidationTokenType.RefreshToken => GenerateRefreshToken(),
            _ => throw new InvalidOperationException(),
        };
    }

    #endregion

    #region Public Tools

    /// <summary>
    /// Validates an incoming string against the current token using a constant-time comparison to mitigate timing attacks.
    /// </summary>
    /// <param name="tokenToVerify">The raw string submitted by the client.</param>
    /// <returns>True if the token is mathematically identical and has not expired; otherwise, false.</returns>
    public bool VerifyToken(string tokenToVerify)
    {
        if (string.IsNullOrWhiteSpace(tokenToVerify))
        {
            return false;
        }

        if (ExpiresOn <= DateTime.UtcNow)
        {
            return false;
        }

        var tokenBytes = Encoding.UTF8.GetBytes(Value);
        var inputBytes = Encoding.UTF8.GetBytes(tokenToVerify);

        if (!CryptographicOperations.FixedTimeEquals(tokenBytes, inputBytes))
        {
            return false;
        }

        return true;
    }

    #endregion

    #region Private Tools

    /// <summary>
    /// Generates an 8-digit numeric One-Time Password.
    /// </summary>
    /// <returns>An OTP token instance.</returns>
    private static ValidationToken GenerateOtp()
    {
        DateTime expiresOn = DateTime.UtcNow.AddMinutes(Options.OneTimePasswordTokenExpireTime);

        var token = RandomNumberGenerator.GetInt32(10000000, 100000000);
        return new ValidationToken(token.ToString(), expiresOn, ValidationTokenType.OneTimePassword);
    }

    /// <summary>
    /// Generates an 8-character token excluding ambiguous characters.
    /// </summary>
    /// <returns>An alphanumeric token instance.</returns>
    private static ValidationToken GenerateAlphanumeric()
    {
        DateTime expiresOn = DateTime.UtcNow.AddMinutes(Options.AlphanumericTokenExpireTime);

        var token = RandomNumberGenerator.GetItems<char>("ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray(), 8);
        return new ValidationToken(new string(token), expiresOn, ValidationTokenType.AlphanumericCode);
    }

    /// <summary>
    /// Generates a 32-byte cryptographic token formatted for safe URL transmission.
    /// </summary>
    /// <returns>A URL-safe token instance.</returns>
    private static ValidationToken GenerateUrlSafe()
    {
        DateTime expiresOn = DateTime.UtcNow.AddMinutes(Options.UrlSafeTokenExpireTime);

        var bytes = new byte[32];
        RandomNumberGenerator.Fill(bytes);

        var token = Convert.ToBase64String(bytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");

        return new ValidationToken(token, expiresOn, ValidationTokenType.UrlSafe);
    }

    /// <summary>
    /// Generates a standard 32-byte cryptographic refresh token.
    /// </summary>
    /// <returns>A refresh token instance.</returns>
    private static ValidationToken GenerateRefreshToken()
    {
        DateTime expiresOn = DateTime.UtcNow.AddMinutes(Options.RefreshJwtTokenExpireTime);

        var randomNumber = new byte[32];
        RandomNumberGenerator.Fill(randomNumber);
        var token = Convert.ToBase64String(randomNumber);
        return new ValidationToken(token, expiresOn, ValidationTokenType.RefreshToken);
    }

    #endregion
}