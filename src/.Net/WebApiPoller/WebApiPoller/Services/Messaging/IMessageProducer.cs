using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiPoller.Services.Messaging
{
    public interface IMessageProducer
    {
        Task NotifyOnProductsUpsert(IEnumerable<string> productIds);
    }
}