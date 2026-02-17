using NukeProjectUtils.ContainerTypes;
using NukeProjectUtils.ContainerTypes.ErrorEnums;
using System.Text.RegularExpressions;

namespace NukeProjectUtils.ValueObjects.Atomics;

public partial record Password
{
    [GeneratedRegex(@"^(?=.*[A-Z])(?=.*\d).{15,}$", RegexOptions.None, 100)]
    private static partial Regex PasswordRegex();

    public string Value { get; init; }

    private Password() { Value = null!; }

    private Password(string password) => Value = password;

    public Result<bool, ErrorTrack> Verify(string password)
    {
        string cleanPassword = password.Trim() ?? string.Empty;

        if (!BCrypt.Net.BCrypt.Verify(cleanPassword, Value))
        {
            var error = ErrorTrack.Create(nameof(Password), PasswordCreateError.ThePasswordDoesntMatch);
            return Result<bool, ErrorTrack>.Fail(error);
        }

        return Result<bool, ErrorTrack>.Success(true);
    }

    public static Result<Password, ErrorTrack> Create(string password)
    {
        string cleanPassword = password.Trim() ?? string.Empty;

        if (!CheckPasswordRules(cleanPassword))
        {
            var error = ErrorTrack.Create(nameof(Password),PasswordCreateError.DoesNotMeetTheRequirements);
            return Result<Password, ErrorTrack>.Fail(error);
        }

        string hash = BCrypt.Net.BCrypt.HashPassword(cleanPassword);

        return Result<Password, ErrorTrack>.Success(new(hash));
    }

    private static bool CheckPasswordRules(string password) => PasswordRegex().IsMatch(password);

    public static implicit operator string(Password password) => password.Value;
}
