using NukeProjectUtils.Exceptions.Base;

namespace NukeProjectUtils.Exceptions;

public class InvalidNameLengthExceptions : DomainException
{
    public InvalidNameLengthExceptions(string str) : base ($"O {str} deve conter no mínimo 3 caracteres.") { }
}
public class InvalidNameCharacterExceptions : DomainException
{
    public InvalidNameCharacterExceptions(string str) : base ($"O {str} deve conter apenas letras.") { }
}
