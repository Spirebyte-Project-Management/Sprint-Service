using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Sprints.Application;

public interface IIdentityContext
{
    Guid Id { get; }
    string Role { get; }
    bool IsAuthenticated { get; }
    bool IsAdmin { get; }
    IDictionary<string, string> Claims { get; }
}