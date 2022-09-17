using System;
using System.Collections.Generic;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;
using Spirebyte.Services.Sprints.Core.Entities;

namespace Spirebyte.Services.Sprints.Application.Sprints.Events;

[Message("sprints", "sprint_created")]
internal class SprintCreated : IEvent
{
    public SprintCreated(string id, string title, string description, IEnumerable<string> issueIds, string projectId,
        DateTime createdAt, DateTime startedAt, DateTime startDate, DateTime endDate, DateTime endedAt,
        IEnumerable<Change> changes, int remainingStoryPoints, int totalStoryPoints)
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

    public SprintCreated(Sprint sprint)
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
}