using Spirebyte.Services.Sprints.Core.Exceptions.Base;

namespace Spirebyte.Services.Sprints.Core.Exceptions;

public class InvalidAggregateIdException : DomainException
{
    public InvalidAggregateIdException() : base("Invalid aggregate id.")
    {
    }

    public override string Code { get; } = "invalid_aggregate_id";
}