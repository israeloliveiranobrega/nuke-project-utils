using NukeProjectUtils.ContainerTypes;
using NukeProjectUtils.ContainerTypes.ErrorEnums;
using System.Security.Cryptography;
using System.Text;

namespace NukeProjectUtils.ValueObjects.Atomics;

public record ValidationToken
{
    private static readonly char[] AlphanumericChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();

    public string Value { get; init; }
    public DateTime ExpiresOn { get; init; }
    public ValidationTokenType VerificationType { get; init; }

    private ValidationToken() { Value = null!;}

    private ValidationToken(string token, DateTime expiresOn, ValidationTokenType type)
    {
        Value = token;
        ExpiresOn = expiresOn;
        VerificationType = type;
    }

    public static Result<ValidationToken, ErrorTrack> Create(ValidationTokenType type, DateTime? expiresOn = null)
    {
        DateTime expirationDate = expiresOn ?? GetDefaultExpirationByType(type);

        return type switch
        {
            ValidationTokenType.OneTimePassword => Result<ValidationToken, ErrorTrack>.Success(GenerateOtp(expirationDate)),
            ValidationTokenType.AlphanumericCode => Result<ValidationToken, ErrorTrack>.Success(GenerateAlphanumeric(expirationDate)),
            ValidationTokenType.UrlSafe => Result<ValidationToken, ErrorTrack>.Success(GenerateUrlSafe(expirationDate)),
            ValidationTokenType.RefreshToken => Result<ValidationToken, ErrorTrack>.Success(GenerateRefreshToken(expirationDate)),
            _ => Result<ValidationToken, ErrorTrack>.Fail(ErrorTrack.Create(nameof(ValidationToken), VerificationTokenError.InvalidOption))
        };
    }

    public Result<bool, ErrorTrack> VerifyToken(string tokenToVerify)
    {
        if (string.IsNullOrWhiteSpace(tokenToVerify))
        {
            return Result<bool, ErrorTrack>.Fail(ErrorTrack.Create(nameof(ValidationToken), VerificationTokenError.TokenIsNull));
        }

        if (ExpiresOn <= DateTime.UtcNow)
        {
            return Result<bool, ErrorTrack>.Fail(ErrorTrack.Create(nameof(ValidationToken), VerificationTokenError.TokenExpired));
        }

        var tokenBytes = Encoding.UTF8.GetBytes(Value);
        var inputBytes = Encoding.UTF8.GetBytes(tokenToVerify);

        if (!CryptographicOperations.FixedTimeEquals(tokenBytes, inputBytes))
        {
            return Result<bool, ErrorTrack>.Fail(ErrorTrack.Create(nameof(ValidationToken), VerificationTokenError.TokenNotMatch));
        }

        return Result<bool, ErrorTrack>.Success(true);
    }

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
        var token = RandomNumberGenerator.GetItems<char>(AlphanumericChars, 8);
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
}