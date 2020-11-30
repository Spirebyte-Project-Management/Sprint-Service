using Convey.MessageBrokers.RabbitMQ;
using Spirebyte.Services.Sprints.Application.Events.Rejected;
using Spirebyte.Services.Sprints.Application.Exceptions;
using System;

namespace Spirebyte.Services.Sprints.Infrastructure.Exceptions
{
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch

            {
                ProjectNotFoundException ex => new SprintCreatedRejected(ex.ProjectId, ex.Message, ex.Code),
                KeyAlreadyExistsException ex => new SprintCreatedRejected(ex.ProjectId, ex.Message, ex.Code),
                _ => null
            };
    }
}
