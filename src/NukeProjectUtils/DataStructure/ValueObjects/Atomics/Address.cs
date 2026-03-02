using NukeProjectUtils.ExtensionMethods;

namespace NukeProjectUtils.DataStructure.ValueObjects.Atomics;
public record Address
{
    public string ZipCode { get; init; }
    public string Region { get; init; }
    public string State { get; init; }
    public string City { get; init; }
    public string Neighborhood { get; init; }
    public string Street { get; init; }
    public string? Number { get; init; }
    public string? Complement { get; init; }
    public string FullAddress
    {
        get
        {
            var fullAddress = $"{Street}";

            if (!string.IsNullOrEmpty(Number))
                fullAddress += $", {Number}";

            if (!string.IsNullOrEmpty(Complement))
                fullAddress += $", {Complement}";

            fullAddress += $", {Neighborhood}, {City} - {State}, CEP: {ZipCode}";
            return fullAddress;
        }
    }

    private Address() { }

    public Address(string zipCode, string region, string state, string city,
    string neighborhood, string street, string? number, string? complement)
    {
        ValidatePostalCode(zipCode);

        if (number != null)
            ValidateNumber(number);

        foreach (var str in new string[] { region, state, city, neighborhood, street })
        {
            ValidateAddressFormar(str);
        }

        ZipCode = zipCode;
        Region = region;
        State = state;
        City = city;
        Neighborhood = neighborhood;
        Street = street;
        Number = number;
        Complement = complement;
    }

    private static void ValidatePostalCode(string postalCode)
    {
        if (!postalCode.IsOnlyLettersOrNumbers(CheckType.OnlyNumbers) || !postalCode.HasLength(8))
        {

        }
            //throw new InvalidPostalCodeFormatException();
    }
    private static void ValidateNumber(string number)
    {
        if (!number.IsOnlyLettersOrNumbers(CheckType.OnlyNumbers) || int.Parse(number) <= 0)
        {
            
        }
            //throw new InvalidNumberFormatException();
    }
    private static void ValidateAddressFormar(string str)
    {
        if (!str.HasContent())
            throw new ArgumentException("Campo não preenchido", nameof(str));
    }
}
