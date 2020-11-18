using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Spirebyte.Services.Sprints.Application.Services.Interfaces
{
    public interface IMessageBroker
    {
        Task PublishAsync(params IEvent[] events);
    }
}
