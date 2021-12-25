﻿using Convey.CQRS.Commands;

namespace Spirebyte.Services.Sprints.Application.Commands;

[Contract]
public class EndSprint : ICommand
{
    public EndSprint(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}