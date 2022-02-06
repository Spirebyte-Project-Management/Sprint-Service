using System;
using Convey.MessageBrokers.RabbitMQ;
using Spirebyte.Services.Sprints.Application.Projects.Exceptions;
using Spirebyte.Services.Sprints.Application.Sprints.Events.Rejected;
using Spirebyte.Services.Sprints.Application.Sprints.Exceptions;

namespace Spirebyte.Services.Sprints.Infrastructure.Exceptions;

internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
{
    public object Map(Exception exception, object message)
    {
        return exception switch

        {
            ProjectNotFoundException ex => new SprintCreatedRejected(ex.ProjectId, ex.Message, ex.Code),
            KeyAlreadyExistsException ex => new SprintCreatedRejected(ex.SprintId, ex.Message, ex.Code),
            _ => null
        };
    }
}