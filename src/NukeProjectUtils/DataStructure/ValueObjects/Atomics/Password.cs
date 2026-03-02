using NukeProjectUtils.Patterns.ResultPattern;
using NukeProjectUtils.Patterns.ResultPattern.BaseErrorResorces;
using System.Text.RegularExpressions;

namespace NukeProjectUtils.DataStructure.ValueObjects.Atomics;

public partial record Password
{
    public string RawValue { get; init; }

    //Construtor para o EF
    private Password() { RawValue = null!; }

    #region Password Creation

    private Password(string password) { RawValue = password; }

    public static Result<Password> Create(string password)
    {
        string cleanPassword = password.Trim() ?? string.Empty;

        if (!CheckPasswordRules(cleanPassword))
        {
            var error = ErrorTrack.Create(PasswordCreateError.DoesNotMeetTheRequirements.ToString());
            return Result<Password>.Failure(error);
        }

        string hash = BCrypt.Net.BCrypt.HashPassword(cleanPassword);

        return Result<Password>.Success(new(hash));
    }

    #endregion

    #region Public Tools

    public Result<bool> Verify(string password)
    {
        string cleanPassword = password.Trim() ?? string.Empty;

        if (!BCrypt.Net.BCrypt.Verify(cleanPassword, RawValue))
        {
            var error = ErrorTrack.Create(PasswordCreateError.ThePasswordDoesntMatch.ToString());
            return Result<bool>.Failure(error);
        }

        return Result<bool>.Success(true);
    }

    #endregion

    #region Private Tools

    [GeneratedRegex(@"^(?=.*[A-Z])(?=.*\d).{15,}$", RegexOptions.None, 100)]
    private static partial Regex PasswordRegex();
    private static bool CheckPasswordRules(string password)
    {
        return PasswordRegex().IsMatch(password);
    }

    #endregion
}
