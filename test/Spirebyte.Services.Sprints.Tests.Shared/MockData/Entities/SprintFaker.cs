using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Bogus;
using Spirebyte.Services.Sprints.Core.Entities;

namespace Spirebyte.Services.Sprints.Tests.Shared.MockData.Entities;

public sealed class SprintFaker : Faker<Sprint>
{
    private SprintFaker()
    {
        CustomInstantiator(_ => FormatterServices.GetUninitializedObject(typeof(Sprint)) as Sprint);
        RuleFor(r => r.Id, f => f.Random.Hash(7));
        RuleFor(r => r.Title, f => f.Random.Word());
        RuleFor(r => r.Description, f => f.Random.Words());
        RuleFor(r => r.IssueIds, f => new List<string>());
        RuleFor(r => r.Changes, f => new List<Change>());
        RuleFor(r => r.Description, f => f.Random.Words());
        RuleFor(r => r.ProjectId, f => f.Random.Word());
        RuleFor(r => r.CreatedAt, f => f.Date.Recent(6, DateTime.UtcNow));
        RuleFor(r => r.StartDate, f => f.Date.Recent(1, DateTime.UtcNow));
        RuleFor(r => r.EndDate, f => f.Date.Soon(1, DateTime.UtcNow));
    }

    public static SprintFaker Instance => new();
}