using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Sprints.Application.Commands;

[Contract]
public class CreateSprint : ICommand
{
    public CreateSprint(string title, string description, string projectId, DateTime startDate,
        DateTime endDate)
    {
        Title = title;
        Description = description;
        ProjectId = projectId;
        StartDate = startDate;
        EndDate = endDate;
        CreatedAt = DateTime.Now;
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public string ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}