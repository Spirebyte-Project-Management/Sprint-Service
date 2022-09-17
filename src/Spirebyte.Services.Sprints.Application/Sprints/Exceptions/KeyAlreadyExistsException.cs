using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Sprints.Application.Sprints.Exceptions;

public class KeyAlreadyExistsException : AppException
{
    public KeyAlreadyExistsException(string sprintId)
        : base($"Sprint with id: {sprintId} already exists.")
    {
        SprintId = sprintId;
    }

    public string Code { get; } = "key_already_exists";
    public string SprintId { get; }
}