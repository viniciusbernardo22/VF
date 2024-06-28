using System.Runtime.Serialization;

namespace VF.Catalog.Domain.Exceptions;

public class EntityValidationException : Exception
{
    public EntityValidationException(string? message) : base(message)
    {
        
    }

}