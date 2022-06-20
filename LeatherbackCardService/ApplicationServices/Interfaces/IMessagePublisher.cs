using System.Threading;
using System.Threading.Tasks;

namespace ApplicationServices.Interfaces
{
    public interface IMessagePublisher
    {
        Task Publish<T>(T message);
        Task SendToQueueAsync<T>(T message, string queue, CancellationToken cancellationToken = default);
    }
}