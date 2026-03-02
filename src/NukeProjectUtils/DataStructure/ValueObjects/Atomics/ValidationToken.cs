using NukeProjectUtils.Patterns.ResultPattern;
using NukeProjectUtils.Patterns.ResultPattern.BaseErrorResorces;
using System.Security.Cryptography;
using System.Text;

namespace NukeProjectUtils.DataStructure.ValueObjects.Atomics;

public record ValidationToken
{
    public string Value { get; init; }
    public DateTime ExpiresOn { get; init; }
    public ValidationTokenType VerificationType { get; init; }

    //Construtor para o EF
    private ValidationToken() { Value = null!;}

    #region Token Creation

    private ValidationToken(string token, DateTime expiresOn, ValidationTokenType type)
    {
        Value = token;
        ExpiresOn = expiresOn;
        VerificationType = type;
    }

    public static Result<ValidationToken> Create(ValidationTokenType type, DateTime? expiresOn = null)
    {
        DateTime expirationDate = expiresOn ?? GetDefaultExpirationByType(type);

        return type switch
        {
            ValidationTokenType.OneTimePassword => Result<ValidationToken>.Success(GenerateOtp(expirationDate)),
            ValidationTokenType.AlphanumericCode => Result<ValidationToken>.Success(GenerateAlphanumeric(expirationDate)),
            ValidationTokenType.UrlSafe => Result<ValidationToken>.Success(GenerateUrlSafe(expirationDate)),
            ValidationTokenType.RefreshToken => Result<ValidationToken>.Success(GenerateRefreshToken(expirationDate)),
            _ => Result<ValidationToken>.Failure(ErrorTrack.Create(VerificationTokenError.InvalidOption.ToString()))
        };
    }

    #endregion

    #region Public Tools

    public Result<bool> VerifyToken(string tokenToVerify)
    {
        if (string.IsNullOrWhiteSpace(tokenToVerify))
        {
            return Result<bool>.Failure(ErrorTrack.Create(VerificationTokenError.TokenIsNull.ToString()));
        }

        if (ExpiresOn <= DateTime.UtcNow)
        {
            return Result<bool>.Failure(ErrorTrack.Create(VerificationTokenError.TokenExpired.ToString()));
        }

        var tokenBytes = Encoding.UTF8.GetBytes(Value);
        var inputBytes = Encoding.UTF8.GetBytes(tokenToVerify);

        if (!CryptographicOperations.FixedTimeEquals(tokenBytes, inputBytes))
        {
            return Result<bool>.Failure(ErrorTrack.Create(VerificationTokenError.TokenNotMatch.ToString()));
        }

        return Result<bool>.Success(true);
    }

    #endregion

    #region Private Tools

    private static DateTime GetDefaultExpirationByType(ValidationTokenType type)
    {
        var now = DateTime.UtcNow;

        return type switch
        {
            ValidationTokenType.OneTimePassword => now.AddMinutes(6),
            ValidationTokenType.AlphanumericCode => now.AddMinutes(6),
            ValidationTokenType.UrlSafe => now.AddHours(6),
            ValidationTokenType.RefreshToken => now.AddDays(7),
            _ => now.AddMinutes(10)
        };
    }

    private static ValidationToken GenerateOtp(DateTime expiresOn)
    {
        var token = RandomNumberGenerator.GetInt32(10000000, 100000000);
        return new ValidationToken(token.ToString(), expiresOn, ValidationTokenType.OneTimePassword);
    }

    private static ValidationToken GenerateAlphanumeric(DateTime expiresOn)
    {
        var token = RandomNumberGenerator.GetItems<char>("ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray(), 8);
        return new ValidationToken(new string(token), expiresOn, ValidationTokenType.AlphanumericCode);
    }

    private static ValidationToken GenerateUrlSafe(DateTime expiresOn)
    {
        var bytes = new byte[32];
        RandomNumberGenerator.Fill(bytes);

        var token = Convert.ToBase64String(bytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");

        return new ValidationToken(token, expiresOn, ValidationTokenType.UrlSafe);
    }

    private static ValidationToken GenerateRefreshToken(DateTime expiresOn)
    {
        var randomNumber = new byte[32];
        RandomNumberGenerator.Fill(randomNumber);
        var token = Convert.ToBase64String(randomNumber);
        return new ValidationToken(token, expiresOn, ValidationTokenType.RefreshToken);
    }

    #endregion
}