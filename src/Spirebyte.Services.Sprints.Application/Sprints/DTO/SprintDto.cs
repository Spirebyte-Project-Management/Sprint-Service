using System;
using System.Collections.Generic;
using Spirebyte.Services.Sprints.Core.Entities;

namespace Spirebyte.Services.Sprints.Application.Sprints.DTO;

public class SprintDto
{
    public SprintDto()
    {
    }

    public SprintDto(Sprint sprint)
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
        Changes = sprint.Changes;
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
    public IEnumerable<Change> Changes { get; set; }
    public int RemainingStoryPoints { get; set; }
    public int TotalStoryPoints { get; set; }
}