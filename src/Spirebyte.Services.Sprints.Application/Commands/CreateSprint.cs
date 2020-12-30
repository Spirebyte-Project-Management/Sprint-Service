using Convey.CQRS.Commands;
using System;

namespace Spirebyte.Services.Sprints.Application.Commands
{
    [Contract]
    public class CreateSprint : ICommand
    {
        public string SprintId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EndedAt { get; set; }

        public CreateSprint(string id, string title, string description, string projectId, DateTime startDate, DateTime endDate)
        {
            SprintId = id;
            Title = title;
            Description = description;
            ProjectId = projectId;
            StartDate = startDate;
            EndDate = endDate;
            CreatedAt = DateTime.Now;
        }
    }
}
