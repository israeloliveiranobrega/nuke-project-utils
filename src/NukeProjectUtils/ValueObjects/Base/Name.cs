using NukeAuthentication.Shared.Exceptions;
using NukeProjectUtils.ExtensionMethods;
using static NukeProjectUtils.ExtensionMethods.StringExtensionHelper;

namespace NukeProjectUtils.ValueObjects.Base;

public record Name
{
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public string FullName => $"{FirstName} {LastName}";

    private Name() { }

    public Name(string firstName, string lastName)
    {
        ValidateNamePart(firstName);
        ValidateNamePart(lastName);

        FirstName = firstName;
        LastName = lastName;
    }

    private static void ValidateNamePart(string namePart)
    {
        if (!namePart.HasContent() || !namePart.HasMinLength(3))
            throw new InvalidNameLengthExceptions(nameof(namePart));

        if (!namePart.IsOnlyLettersOrNumbers(CheckType.OnlyLetters))
            throw new InvalidNameCharacterExceptions(nameof(namePart));
    }
}
