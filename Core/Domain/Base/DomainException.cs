using System.Runtime.Serialization;

namespace Domain.Base;

public class DomainException : Exception
{
    public DomainValidation DomainValidation { get; }
    
    public Dictionary<string, string> Errors { get; }
    
    public DomainException(DomainValidation validator)
    {
        this.DomainValidation = validator;
        this.Errors = DomainValidation.Fails;
    }

    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected DomainException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}