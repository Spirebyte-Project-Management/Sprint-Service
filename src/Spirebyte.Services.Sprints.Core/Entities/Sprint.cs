using Spirebyte.Services.Sprints.Core.Exceptions;
using System;

namespace Spirebyte.Services.Sprints.Core.Entities
{
    public class Sprint
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProjectId { get; set; }
        public string[] IssueIds { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EndedAt { get; set; }

        public Sprint(string id, string title, string description, string projectId, string[] issueIds, DateTime createdAt, DateTime startedAt, DateTime startDate, DateTime endDate, DateTime endedAt)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new InvalidIdException(id);
            }

            if (string.IsNullOrWhiteSpace(projectId))
            {
                throw new InvalidProjectIdException(projectId);
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new InvalidTitleException(title);
            }

            Id = id;
            Title = title;
            Description = description;
            ProjectId = projectId;
            IssueIds = issueIds ?? new string[] { };
            CreatedAt = createdAt;
            StartedAt = startedAt;
            StartDate = startDate;
            EndDate = endDate;
            EndedAt = endedAt;
        }

        public void Start()
        {
            StartedAt = DateTime.Now;
        }
    }
}