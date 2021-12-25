using Spirebyte.Services.Sprints.Core.Exceptions.Base;

namespace Spirebyte.Services.Sprints.Core.Exceptions;

public class InvalidIdException : DomainException
{
    public InvalidIdException(string key) : base($"Invalid key: {key}.")
    {
    }

    public override string Code { get; } = "invalid_key";
}