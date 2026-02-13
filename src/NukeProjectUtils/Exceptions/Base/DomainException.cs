namespace NukeProjectUtils.Exceptions.Base;
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}
