using System;
using System.Collections.Generic;
using Convey.CQRS.Events;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Shared.Changes;
using Change = Spirebyte.Shared.Changes.ValueObjects.Change;

namespace Spirebyte.Services.Sprints.Application.Sprints.Events;

[Contract]
internal class SprintUpdated : IEvent
{
    public SprintUpdated(string id, string title, string description, IEnumerable<string> issueIds, string projectId,
        DateTime createdAt, DateTime startedAt, DateTime startDate, DateTime endDate, DateTime endedAt,
        int remainingStoryPoints, int totalStoryPoints)
    {
        Id = id;
        Title = title;
        Description = description;
        IssueIds = issueIds;
        ProjectId = projectId;
        CreatedAt = createdAt;
        StartedAt = startedAt;
        StartDate = startDate;
        EndDate = endDate;
        EndedAt = endedAt;
        RemainingStoryPoints = remainingStoryPoints;
        TotalStoryPoints = totalStoryPoints;
    }

    public SprintUpdated(Sprint sprint, Sprint old)
    {
        Id = sprint.Id;
        Title = sprint.Title;
        Description = sprint.Description;
        ProjectId = sprint.ProjectId;
        CreatedAt = sprint.CreatedAt;
        StartedAt = sprint.StartedAt;
        StartDate = sprint.StartDate;
        EndDate = sprint.EndDate;
        EndedAt = sprint.EndedAt;
        RemainingStoryPoints = sprint.RemainingStoryPoints;
        TotalStoryPoints = sprint.TotalStoryPoints;

        Changes = ChangedFieldsHelper.GetChanges(old, sprint);
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IEnumerable<string> IssueIds { get; set; }
    public string ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime EndedAt { get; set; }
    public int RemainingStoryPoints { get; set; }
    public int TotalStoryPoints { get; set; }

    public Change[] Changes { get; set; }
}