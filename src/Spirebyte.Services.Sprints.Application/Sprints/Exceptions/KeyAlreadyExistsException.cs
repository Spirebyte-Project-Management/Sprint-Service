using Spirebyte.Services.Sprints.Application.Exceptions.Base;

namespace Spirebyte.Services.Sprints.Application.Sprints.Exceptions;

public class KeyAlreadyExistsException : AppException
{
    public KeyAlreadyExistsException(string sprintId)
        : base($"Sprint with id: {sprintId} already exists.")
    {
        SprintId = sprintId;
    }

    public override string Code { get; } = "key_already_exists";
    public string SprintId { get; }
}