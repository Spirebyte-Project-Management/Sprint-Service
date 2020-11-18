using System;
using Spirebyte.Services.Sprints.Core.Exceptions;

namespace Spirebyte.Services.Sprints.Core.Entities
{
    public class Sprint
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EndedAt { get; set; }

        public Sprint(Guid id, string key, string title, string description, Guid projectId, DateTime createdAt, DateTime startedAt, DateTime startDate, DateTime endDate, DateTime endedAt)
        {
            if (projectId == Guid.Empty)
            {
                throw new InvalidProjectIdException(projectId);
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidKeyException(title);
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new InvalidTitleException(title);
            }

            Id = id;
            Key = key;
            Title = title;
            Description = description;
            ProjectId = projectId;
            CreatedAt = createdAt;
            StartedAt = startedAt;
            StartDate = startDate;
            EndDate = endDate;
            EndedAt = endedAt;
        }
    }
}