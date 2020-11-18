using System;
using System.Collections.Generic;
using System.Linq;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Sprints.Application.Commands
{
    [Contract]
    public class UpdateSprint : ICommand
    {
        public Guid ProjectId { get; }
        public IEnumerable<Guid> ProjectUserIds { get; }
        public IEnumerable<Guid> InvitedUserIds { get; }
        public string Key { get; set; }
        public string Pic { get; }
        public string File { get; }
        public string Title { get; }
        public string Description { get; }

        public UpdateSprint(Guid projectId, string key, IEnumerable<Guid> projectUserIds, IEnumerable<Guid> invitedUserIds, string pic, string file, string title,
            string description)
        {
            ProjectId = projectId;
            Key = key;
            ProjectUserIds = projectUserIds ?? Enumerable.Empty<Guid>();
            InvitedUserIds = invitedUserIds ?? Enumerable.Empty<Guid>();
            Pic = pic;
            File = file;
            Title = title;
            Description = description;
        }
    }
}
