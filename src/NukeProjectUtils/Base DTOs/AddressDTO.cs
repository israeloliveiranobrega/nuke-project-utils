namespace NukeAuthentication.Src.Shared.Base_DTOs;

public record AddressDTO(string ZipCode, string Region, string State, string City, string Neighborhood, string Street, string? Number, string? Complement);
