using System;
using System.Collections.Generic;
using Spirebyte.Framework.Shared.Types;
using Spirebyte.Services.Sprints.Core.Entities;

namespace Spirebyte.Services.Sprints.Infrastructure.Mongo.Documents;

public sealed class SprintDocument : IIdentifiable<string>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime EndedAt { get; set; }
    public IEnumerable<string> IssueIds { get; set; }

    public IEnumerable<Change> Changes { get; set; }
    public int RemainingStoryPoints { get; set; }
    public int TotalStoryPoints { get; set; }
    public string Id { get; set; }
}