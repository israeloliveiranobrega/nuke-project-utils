namespace NukeAuthentication.Src.Shared.Base_DTOs;

public record PersonDTO(string FirstName, string LastName, DateOnly BirthDate, string Cpf);