using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Sprints.Application.Sprints.Exceptions;

public class SprintNotStartedException : AppException
{
    public SprintNotStartedException(string sprintId) : base($"Sprint with id: '{sprintId}' is not yet started.")
    {
        SprintId = sprintId;
    }

    public string Code { get; } = "sprint_not_started";
    public string SprintId { get; }
}