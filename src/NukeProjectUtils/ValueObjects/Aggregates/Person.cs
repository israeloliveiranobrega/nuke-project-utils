using NukeProjectUtils.ContainerTypes;
using NukeProjectUtils.ContainerTypes.ErrorEnums;
using NukeProjectUtils.ValueObjects.Atomics;

namespace NukeProjectUtils.ValueObjects.Aggregates;

public record Person
{
    public Name Name { get; init; }
    public DateOnly BirthDate { get; init; }
    public CPF Cpf { get; init; }

    private Person() { }

    private Person(Name name, DateOnly birthDate, CPF cpf)
    {
        Name = name;
        Cpf = cpf;
        BirthDate = birthDate;
    }

    public static Result<Person, ErrorTrack> Create(string firstName, string lastName, DateOnly birthDate, string cpf)
    {
        var nameResult = Name.Create(firstName, lastName);

        if (!nameResult.IsSuccess)
        {
            return Result<Person, ErrorTrack>.Fail(nameResult.Failure.AddContext(nameof(Person)));
        }

        var cpfResult = CPF.Create(cpf);

        if (!cpfResult.IsSuccess)
        {
            return Result<Person, ErrorTrack>.Fail(cpfResult.Failure.AddContext(nameof(Person)));
        }

        if (!IsOfLegalAge(birthDate))
        {
            var error = ErrorTrack.Create(nameof(Person), PersonCreateError.InvalidAge);
            return Result<Person, ErrorTrack>.Fail(error);
        }

        return Result<Person, ErrorTrack>.Success(new Person(nameResult.Value, birthDate, cpfResult.Value));
    }

    private static bool IsOfLegalAge(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var age = today.Year - birthDate.Year;

        if (birthDate > today.AddYears(-age))
            age--;

        return age is >= 18 and <= 120;
    }
}