using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Spirebyte.Services.Sprints.Application.Events.External
{
    [Message("issues")]
    public class IssueCreated : IEvent
    {
        public Guid IssueId { get; }
        public string IssueKey { get; }

        public IssueCreated(Guid issueId, string issueKey)
        {
            IssueId = issueId;
            IssueKey = issueKey;
        }
    }
}
