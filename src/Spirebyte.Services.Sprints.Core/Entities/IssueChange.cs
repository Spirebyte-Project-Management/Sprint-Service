namespace Spirebyte.Services.Sprints.Core.Entities;

public class IssueChange
{
    public IssueChange(string issueId, int increase = 0, int decrease = 0)
    {
        IssueId = issueId;
        Increase = increase;
        Decrease = decrease;
    }

    public string IssueId { get; set; }
    public int Increase { get; set; }
    public int Decrease { get; set; }
}