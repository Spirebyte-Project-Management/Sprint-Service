using Spirebyte.Services.Sprints.Application.Exceptions.Base;

namespace Spirebyte.Services.Sprints.Application.Exceptions
{
    public class SprintNotStartedException : AppException
    {
        public override string Code { get; } = "sprint_not_started";
        public string SprintId { get; }

        public SprintNotStartedException(string sprintId) : base($"Sprint with id: '{sprintId}' is not yet started.")
        {
            SprintId = sprintId;
        }
    }
}
