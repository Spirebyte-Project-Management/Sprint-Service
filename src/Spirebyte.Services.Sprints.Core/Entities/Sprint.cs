using System;
using System.Collections.Generic;
using Spirebyte.Services.Sprints.Core.Enums;
using Spirebyte.Services.Sprints.Core.Exceptions;

namespace Spirebyte.Services.Sprints.Core.Entities;

public class Sprint
{
    public Sprint(string id, string title, string description, string projectId, List<string> issueIds,
        DateTime createdAt,
        DateTime startedAt, DateTime startDate, DateTime endDate, DateTime endedAt, List<Change> changes,
        int remainingStoryPoints, int totalStoryPoints)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new InvalidIdException(id);

        if (string.IsNullOrWhiteSpace(projectId)) throw new InvalidProjectIdException(projectId);

        if (string.IsNullOrWhiteSpace(title)) throw new InvalidTitleException(title);

        Id = id;
        Title = title;
        Description = description;
        ProjectId = projectId;
        IssueIds = issueIds ?? new List<string>();
        CreatedAt = createdAt;
        StartedAt = startedAt;
        StartDate = startDate;
        EndDate = endDate;
        EndedAt = endedAt;

        Changes = changes ?? new List<Change>();

        RemainingStoryPoints = remainingStoryPoints;
        TotalStoryPoints = totalStoryPoints;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ProjectId { get; set; }
    public List<string> IssueIds { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime EndedAt { get; set; }

    public List<Change> Changes { get; set; }
    public int RemainingStoryPoints { get; set; }
    public int TotalStoryPoints { get; set; }

    public void Start()
    {
        if (StartedAt == DateTime.MinValue)
        {
            StartedAt = DateTime.Now;

            var issueChanges = IssueIds.ConvertAll(id => new IssueChange(id));
            var newChange = new Change(StartedAt, EventType.SprintStart, "", issueChanges, RemainingStoryPoints);
            Changes.Add(newChange);
        }
    }

    public void End()
    {
        if (StartedAt != DateTime.MinValue && EndedAt == DateTime.MinValue)
        {
            EndedAt = DateTime.Now;

            var issueChanges = IssueIds.ConvertAll(id => new IssueChange(id));
            var newChange = new Change(EndedAt, EventType.SprintEnded, "", issueChanges, RemainingStoryPoints);
            Changes.Add(newChange);
        }
    }

    public void AddIssue(Issue issue)
    {
        if (!IssueIds.Contains(issue.Id)) IssueIds.Add(issue.Id);

        if (StartedAt != DateTime.MinValue && EndedAt == DateTime.MinValue)
        {
            var issueChange = new IssueChange(issue.Id, issue.StoryPoints, 0);
            RemainingStoryPoints += issue.StoryPoints;
            TotalStoryPoints += issue.StoryPoints;

            var change = new Change(DateTime.Now, EventType.ScopeChange, $"Issue added to sprint",
                new List<IssueChange> { issueChange }, RemainingStoryPoints);

            Changes.Add(change);
        }
    }

    public void RemoveIssue(Issue issue)
    {
        if (IssueIds.Contains(issue.Id)) IssueIds.Remove(issue.Id);

        if (StartedAt != DateTime.MinValue && EndedAt == DateTime.MinValue)
        {
            var issueChange = new IssueChange(issue.Id, 0, issue.StoryPoints);
            RemainingStoryPoints -= issue.StoryPoints;
            TotalStoryPoints -= issue.StoryPoints;

            var change = new Change(DateTime.Now, EventType.ScopeChange, $"Issue removed from sprint",
                new List<IssueChange> { issueChange }, RemainingStoryPoints);

            Changes.Add(change);
        }
    }

    public void IssueChanged(Issue newIssue, Issue oldIssue)
    {
        var difference = newIssue.StoryPoints - oldIssue.StoryPoints;

        RemainingStoryPoints += difference;
        TotalStoryPoints += difference;

        if (StartedAt != DateTime.MinValue && EndedAt == DateTime.MinValue)
        {
            var increase = difference > 0 ? difference : 0;
            var decrease = difference < 0 ? difference : 0;

            var issueChange = new IssueChange(newIssue.Id, increase, decrease);

            var change = new Change(DateTime.Now, EventType.ScopeChange, $"Estimate Changed from {oldIssue.StoryPoints} to {newIssue.StoryPoints}",
                new List<IssueChange> { issueChange }, RemainingStoryPoints);

            Changes.Add(change);
        }
    }

    public void IssueCompleted(Issue issue)
    {
        var issueChange = new IssueChange(issue.Id, 0, issue.StoryPoints);
        RemainingStoryPoints -= issue.StoryPoints;

        var change = new Change(DateTime.Now, EventType.BurnDown, "Issue completed",
            new List<IssueChange> { issueChange }, RemainingStoryPoints);

        Changes.Add(change);
    }

    public void IssueReOpened(Issue issue)
    {
        var issueChange = new IssueChange(issue.Id, 0, issue.StoryPoints);
        RemainingStoryPoints += issue.StoryPoints;

        var change = new Change(DateTime.Now, EventType.BurnDown, "Issue reopened",
            new List<IssueChange> { issueChange }, RemainingStoryPoints);

        Changes.Add(change);
    }
}