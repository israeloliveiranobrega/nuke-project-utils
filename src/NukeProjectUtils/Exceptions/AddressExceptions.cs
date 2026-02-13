using NukeProjectUtils.Exceptions.Base;

namespace NukeProjectUtils.Exceptions;


public class InvalidPostalCodeExceptions : DomainException
{
    public InvalidPostalCodeExceptions() : base("O CEP informado é inválido.") { }
}
public class InvalidPostalCodeFormatException : DomainException
{
    public InvalidPostalCodeFormatException() : base("O CEP deve conter apenas números e ter 8 digitos.") { }
}
public class InvalidNumberException : DomainException
{
    public InvalidNumberException() : base("O número informado é inválido.") { }
}
public class InvalidNumberFormatException : DomainException
{
    public InvalidNumberFormatException() : base("O número deve conter apenas números e ser acima de 0.") { }
}
