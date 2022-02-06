using System;
using System.Collections.Generic;
using Spirebyte.Services.Sprints.Core.Enums;

namespace Spirebyte.Services.Sprints.Core.Entities;

public class Change
{
    public Change(DateTime date, EventType eventType, string eventDescription, List<IssueChange> issueChanges,
        int remaining)
    {
        Date = date;
        EventType = eventType;
        EventDescription = eventDescription;
        IssueChanges = issueChanges;
        Remaining = remaining;
    }

    public DateTime Date { get; set; }

    public EventType EventType { get; set; }

    public string EventDescription { get; set; }

    public List<IssueChange> IssueChanges { get; set; }

    public int Remaining { get; set; }
}