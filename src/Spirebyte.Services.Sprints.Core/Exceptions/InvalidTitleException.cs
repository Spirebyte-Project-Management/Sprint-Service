using Spirebyte.Services.Sprints.Core.Exceptions.Base;

namespace Spirebyte.Services.Sprints.Core.Exceptions;

public class InvalidTitleException : DomainException
{
    public InvalidTitleException(string title) : base($"Invalid title: {title}.")
    {
    }

    public override string Code { get; } = "invalid_title";
}