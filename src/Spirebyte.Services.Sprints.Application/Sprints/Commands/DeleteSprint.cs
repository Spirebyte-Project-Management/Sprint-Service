﻿using Convey.CQRS.Commands;

namespace Spirebyte.Services.Sprints.Application.Sprints.Commands;

[Contract]
public class DeleteSprint : ICommand
{
    public DeleteSprint(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}