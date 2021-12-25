using System;

namespace Spirebyte.Services.Sprints.Application.Exceptions.Base;

public abstract class AppException : Exception
{
    protected AppException(string message) : base(message)
    {
    }

    public virtual string Code { get; }
}