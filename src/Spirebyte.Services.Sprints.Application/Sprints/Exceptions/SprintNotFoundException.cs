﻿using Spirebyte.Services.Sprints.Application.Exceptions.Base;

namespace Spirebyte.Services.Sprints.Application.Sprints.Exceptions;

public class SprintNotFoundException : AppException
{
    public SprintNotFoundException(string key) : base($"Sprint with Key: '{key}' was not found.")
    {
        Key = key;
    }

    public override string Code { get; } = "sprint_not_found";
    public string Key { get; }
}