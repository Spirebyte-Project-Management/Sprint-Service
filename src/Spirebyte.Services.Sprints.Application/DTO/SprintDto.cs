using System;
using System.Collections.Generic;
using Spirebyte.Services.Sprints.Core.Entities;

namespace Spirebyte.Services.Sprints.Application.DTO
{
    public class SprintDto
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid[] IssueIds { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EndedAt { get; set; }

        public SprintDto()
        {
        }

        public SprintDto(Sprint sprint)
        {
            Id = sprint.Id;
            Key = sprint.Key;
            Title = sprint.Title;
            Description = sprint.Description;
            ProjectId = sprint.ProjectId;
            CreatedAt = sprint.CreatedAt;
            StartedAt = sprint.StartedAt;
            StartDate = sprint.StartDate;
            EndDate = sprint.EndDate;
            EndedAt = sprint.EndedAt;
        }
    }
}
