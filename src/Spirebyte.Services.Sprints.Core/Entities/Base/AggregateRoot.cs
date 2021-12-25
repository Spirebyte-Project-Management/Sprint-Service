﻿using System.Collections.Generic;
using Spirebyte.Services.Sprints.Core.Entities.Base.Interfaces;

namespace Spirebyte.Services.Sprints.Core.Entities.Base;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _events = new();
    public IEnumerable<IDomainEvent> Events => _events;
    public AggregateId Id { get; protected set; }
    public int Version { get; protected set; }

    protected void AddEvent(IDomainEvent @event)
    {
        _events.Add(@event);
    }

    public void ClearEvents()
    {
        _events.Clear();
    }
}