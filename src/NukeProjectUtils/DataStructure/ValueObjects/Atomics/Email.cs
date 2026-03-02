using NukeProjectUtils.ExtensionMethods;
using NukeProjectUtils.Patterns.ResultPattern;
using NukeProjectUtils.Patterns.ResultPattern.BaseErrorResorces;
using System.Text.RegularExpressions;

namespace NukeProjectUtils.DataStructure.ValueObjects.Atomics;

public partial record Email
{
    public string RawValue { get; init; }

    public string LocalPart => RawValue.Split('@')[0];
    public string Domain => RawValue.Split('@')[1];
    public string MaskedEmail => MaskEmail();

    //Construtor para o EF
    private Email() { RawValue = null!; }

    #region Email Creation

    private Email(string emailAddress) { RawValue = emailAddress; }

    public static Result<Email> Create(string emailAddress)
    {
        string cleanEmailAddress = emailAddress.Trim() ?? string.Empty;

        if (!cleanEmailAddress.HasContent())
        {
            var error = ErrorTrack.Create(EmailCreationError.NullOrEmpty.ToString());
            return Result<Email>.Failure(error);
        }

        if (cleanEmailAddress.ExceedsMaxLength(64))
        {
            var error = ErrorTrack.Create(EmailCreationError.MaxLengthExceeded.ToString());
            return Result<Email>.Failure(error);
        }

        if (!CheckEmailRules(cleanEmailAddress))
        {
            var error = ErrorTrack.Create(EmailCreationError.InvalidFormat.ToString());
            return Result<Email>.Failure(error);
        }

        return Result<Email>.Success(new(cleanEmailAddress));
    }

    #endregion

    #region Private Tools

    private string MaskEmail()
    {
        if (string.IsNullOrEmpty(LocalPart) || LocalPart.Length < 2)
            return RawValue;

        var asterisks = new string('*', LocalPart.Length - 1);
        return $"{LocalPart[0]}{asterisks}@{Domain}";
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+$", RegexOptions.None, 100)]
    private static partial Regex EmailRegex();

    private static bool CheckEmailRules(string email)
    {
        return EmailRegex().IsMatch(email);
    }

    #endregion
}