using NukeProjectUtils.ContainerTypes;
using NukeProjectUtils.ContainerTypes.ErrorEnums;
using NukeProjectUtils.ExtensionMethods;
using System.Text.RegularExpressions;

namespace NukeProjectUtils.ValueObjects.Atomics;

public partial record Name
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string FullName => $"{FirstName} {LastName}";

    //Construtor para o EF
    private Name() { FirstName = null!; LastName = null!; }

    #region Name Creation 

    private Name(string firstName, string lastName) 
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static Result<Name> Create(string firstName, string lastName)
    {
        string cleanFirstName = firstName.Trim() ?? string.Empty;
        string cleanLastName = lastName.Trim() ?? string.Empty;

        var firstResult = IsValidName(cleanFirstName, 30);

        if (!firstResult.IsFailure)
            return Result<Name>.Failure(firstResult.ErrorTrack);

        var secondResult = IsValidName(cleanLastName, 70);

        if (!secondResult.IsFailure)
            return Result<Name>.Failure(secondResult.ErrorTrack);

        return Result<Name>.Success(new (cleanFirstName, cleanLastName));
    }

    #endregion

    #region Private Tools

    private static Result<bool> IsValidName(string namePart, int MaxLength)
    {
        if (!namePart.HasContent())
        {
            var error = ErrorTrack.Create(NameCreationError.NullOrEmpty.ToString());
            return Result<bool>.Failure(error);
        }

        if (!namePart.HasMinLength(3))
        {
            var error = ErrorTrack.Create(NameCreationError.NameTooShort.ToString());
            return Result<bool>.Failure(error);
        }

        if (namePart.ExceedsMaxLength(MaxLength))
        {
            var error = ErrorTrack.Create(NameCreationError.NameExedesLength.ToString());
            return Result<bool>.Failure(error);
        }

        if (!CheckNameRules(namePart))
        {
            var error = ErrorTrack.Create(NameCreationError.InvalidCharacters.ToString());
            return Result<bool>.Failure(error);
        }

        return Result<bool>.Success(true);
    }

    [GeneratedRegex(@"^[\p{L}'-]+( [\p{L}'-]+)*$", RegexOptions.None, 100)]
    private static partial Regex NameRegex();

    private static bool CheckNameRules(string name)
    {
        return NameRegex().IsMatch(name);
    }

    #endregion
}
