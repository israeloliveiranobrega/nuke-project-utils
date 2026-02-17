using NukeProjectUtils.ContainerTypes;
using NukeProjectUtils.ContainerTypes.ErrorEnums;
using NukeProjectUtils.ExtensionMethods;
using System.Text.RegularExpressions;

namespace NukeProjectUtils.ValueObjects.Atomics;

public partial record Email
{
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+$", RegexOptions.None, 100)]
    private static partial Regex EmailRegex();

    public string Value { get; init; }

    public string MaskedEmail
    {
        get
        {
            if (string.IsNullOrEmpty(LocalPart) || LocalPart.Length < 2)
                return Value;

            var asterisks = new string('*', LocalPart.Length - 1);
            return $"{LocalPart[0]}{asterisks}@{Domain}";
        }
    }
    public string LocalPart => Value.Split('@')[0];
    public string Domain => Value.Split('@')[1];

    private Email() { Value = null!; }

    private Email(string emailAddress) => Value = emailAddress;

    public static Result<Email, ErrorTrack> Create(string emailAddress)
    {
        string cleanEmailAddress = emailAddress.Trim() ?? string.Empty;

        if (!cleanEmailAddress.HasContent())
        {
            var error = ErrorTrack.Create(nameof(Email), EmailCreationError.NullOrEmpty, nameof(Create));
            return Result<Email, ErrorTrack>.Fail(error);
        }

        if (cleanEmailAddress.ExceedsMaxLength(64))
        {
            var error = ErrorTrack.Create(nameof(Email), EmailCreationError.MaxLengthExceeded, nameof(Create));
            return Result<Email, ErrorTrack>.Fail(error);
        }

        if (!CheckEmailRules(cleanEmailAddress))
        {
            var error = ErrorTrack.Create(nameof(Email), EmailCreationError.InvalidFormat, nameof(Create));
            return Result<Email, ErrorTrack>.Fail(error);
        }

        return Result<Email, ErrorTrack>.Success(new(cleanEmailAddress));
    }
    private static bool CheckEmailRules(string email) => EmailRegex().IsMatch(email);

    public static implicit operator string(Email email) => email.Value;
}