using System;

namespace Spirebyte.Services.Sprints.Application.Exceptions.Base
{
    public abstract class AppException : Exception
    {
        public virtual string Code { get; }

        protected AppException(string message) : base(message)
        {
        }
    }
}
