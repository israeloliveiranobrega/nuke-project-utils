using NukeProjectUtils.Exceptions.Base;

namespace NukeProjectUtils.Exceptions;

public class InvalidPhoneCountryCodeFormatExceptions : DomainException
{
    public InvalidPhoneCountryCodeFormatExceptions() : base ("O DDD deve conter apenas numeros.") { }
}
public class InvalidPhoneCountryCodeLengthExceptions : DomainException
{
    public InvalidPhoneCountryCodeLengthExceptions() : base("O DDD deve conter 2 digitos.") { }
}
public class InvalidPhoneNumberLengthExceptions : DomainException
{
    public InvalidPhoneNumberLengthExceptions() : base("O numero deve conter 9 digitos.") { }
}
public class InvalidPhoneNumberFormatExceptions : DomainException
{
    public InvalidPhoneNumberFormatExceptions() : base("O numero deve conter apenas numeros.") { }
}
