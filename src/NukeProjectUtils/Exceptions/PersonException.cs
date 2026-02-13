using NukeProjectUtils.Exceptions.Base;

namespace NukeProjectUtils.Exceptions;

public class UnderageException : DomainException
{
    public UnderageException() : base("O usuário deve ser maior de idade.") { }
}
public class OveragedException : DomainException
{
    public OveragedException() : base("A idade do usuário é inválida.") { }
}
