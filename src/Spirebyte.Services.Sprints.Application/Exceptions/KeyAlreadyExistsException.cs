using System;
using Spirebyte.Services.Sprints.Application.Exceptions.Base;

namespace Spirebyte.Services.Sprints.Application.Exceptions
{
    public class KeyAlreadyExistsException : AppException
    {
        public override string Code { get; } = "key_already_exists";
        public string Key { get; }
        public Guid UserId { get; }


        public KeyAlreadyExistsException(string key, Guid userId)
            : base($"Project with key: {key} already exists.")
        {
            Key = key;
            UserId = userId;
        }
    }
}
