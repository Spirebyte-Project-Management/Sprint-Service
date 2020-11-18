using System;
using Convey.MessageBrokers.RabbitMQ;
using Spirebyte.Services.Sprints.Application.Events.Rejected;
using Spirebyte.Services.Sprints.Application.Exceptions;

namespace Spirebyte.Services.Sprints.Infrastructure.Exceptions
{
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch

            {
                UserNotFoundException ex => new SprintCreatedRejected(ex.UserId, ex.Message, ex.Code),
                KeyAlreadyExistsException ex => new SprintCreatedRejected(ex.UserId, ex.Message, ex.Code),
                _ => null
            };
    }
}
