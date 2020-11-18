using System;
using System.Collections.Generic;
using System.Linq;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Sprints.Application.Commands
{
    [Contract]
    public class CreateSprint : ICommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectKey { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EndedAt { get; set; }

        public CreateSprint(Guid id, string title, string description, Guid projectId, string projectKey, DateTime startDate, DateTime endDate)
        {
            Id = id;
            Title = title;
            Description = description;
            ProjectId = projectId;
            ProjectKey = projectKey;
            StartDate = startDate;
            EndDate = endDate;
            CreatedAt = DateTime.Now;
        }
    }
}
