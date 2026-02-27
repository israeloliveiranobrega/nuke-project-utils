using NukeProjectUtils.ContainerTypes;
using NukeProjectUtils.ContainerTypes.ErrorEnums;

namespace NukeProjectUtils.ValueObjects.Atomics;

public record Person
{
    public Name Name { get; init; }
    public DateOnly BirthDate { get; init; }
    public CPF Cpf { get; init; }
    public int Age => FindAge(BirthDate);

    //Construtor para o EF
    private Person() { Name = null!; Cpf = null!; }

    #region Perason Creations

    private Person(Name name, DateOnly birthDate, CPF cpf)
    {
        Name = name;
        Cpf = cpf;
        BirthDate = birthDate;
    }

    public static Result<Person> Create(string firstName, string lastName, DateOnly birthDate, string cpf)
    {
        var nameResult = Name.Create(firstName, lastName);

        if (nameResult.IsFailure)
        {
            return Result<Person>.Failure(nameResult.ErrorTrack.AddContext());
        }

        var cpfResult = CPF.Create(cpf);

        if (cpfResult.IsFailure)
        {
            return Result<Person>.Failure(cpfResult.ErrorTrack.AddContext());
        }

        if (!IsOfLegalAge(birthDate))
        {
            var error = ErrorTrack.Create(PersonCreateError.InvalidAge.ToString());
            return Result<Person>.Failure(error);
        }

        return Result<Person>.Success(new Person(nameResult.Value, birthDate, cpfResult.Value));
    }

    #endregion

    #region Private Tools

    private static int FindAge(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var age = today.Year - birthDate.Year;

        if (birthDate > today.AddYears(-age))
            age--;

        return age;
    }

    private static bool IsOfLegalAge(DateOnly birthDate)
    {
        return FindAge(birthDate) is >= 18 and <= 120;
    }

    #endregion
}