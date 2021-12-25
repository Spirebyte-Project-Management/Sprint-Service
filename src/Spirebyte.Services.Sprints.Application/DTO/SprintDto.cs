using System;
using Spirebyte.Services.Sprints.Core.Entities;

namespace Spirebyte.Services.Sprints.Application.DTO;

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
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string[] IssueIds { get; set; }
    public string ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime EndedAt { get; set; }
}