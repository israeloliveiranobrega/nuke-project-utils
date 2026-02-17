using NukeProjectUtils.ContainerTypes;
using NukeProjectUtils.ContainerTypes.ErrorEnums;
using NukeProjectUtils.ExtensionMethods;
using System.Text.RegularExpressions;

namespace NukeProjectUtils.ValueObjects.Atomics;

public partial record Name
{
    [GeneratedRegex(@"^[\p{L}'-]+( [\p{L}'-]+)*$", RegexOptions.None, 100)]
    private static partial Regex NameRegex();


    public string FirstName { get; init; }
    public string LastName { get; init; }

    public string FullName => $"{FirstName} {LastName}";

    private Name() { FirstName = null!; LastName = null!; }

    private Name(string firstName, string lastName) 
    {
        FirstName = firstName;
        LastName = lastName;
    }

    private static Result<bool, ErrorTrack> IsValidName(string namePart, int MaxLength)
    {
        if (!namePart.HasContent())
        {
            var error = ErrorTrack.Create(nameof(Name), NameCreationError.NullOrEmpty);
            return Result<bool, ErrorTrack>.Fail(error);
        }

        if (!namePart.HasMinLength(3))
        {
            var error = ErrorTrack.Create(nameof(Name), NameCreationError.NameTooShort);
            return Result<bool, ErrorTrack>.Fail(error);
        }

        if (namePart.ExceedsMaxLength(MaxLength))
        {
            var error = ErrorTrack.Create(nameof(Name), NameCreationError.NameExedesLength);
            return Result<bool, ErrorTrack>.Fail(error);
        }

        if (!CheckNameRules(namePart))
        {
            var error = ErrorTrack.Create(nameof(Name), NameCreationError.InvalidCharacters);
            return Result<bool, ErrorTrack>.Fail(error);
        }

        return Result<bool, ErrorTrack>.Success(true);
    }

    public static Result<Name,ErrorTrack> Create(string firstName, string lastName)
    {
        string cleanFirstName = firstName.Trim() ?? string.Empty;
        string cleanLastName = lastName.Trim() ?? string.Empty;

        var firstResult = IsValidName(cleanFirstName, 30);

        if (!firstResult.IsSuccess)
            return Result<Name, ErrorTrack>.Fail(firstResult.Failure);

        var secondResult = IsValidName(cleanLastName, 70);

        if (!secondResult.IsSuccess)
            return Result<Name, ErrorTrack>.Fail(secondResult.Failure);

        return Result<Name, ErrorTrack>.Success(new (cleanFirstName, cleanLastName));
    }

    private static bool CheckNameRules(string name) => NameRegex().IsMatch(name);
}
