using Confluent.Kafka;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPoller.Services.Messaging
{
    public class MessageProducer : IMessageProducer
    {
        private string _topicName = "Products batch Upsert";
        private IProducer<Null, IEnumerable<string>> _producer;

        public MessageProducer(IProducer<Null, IEnumerable<string>> producer)
        {
            _producer = producer;
        }

        public async Task NotifyOnProductsUpsert(IEnumerable<string> productIds)
        {
            var message = new Message<Null, IEnumerable<string>>()
            {
                Value = productIds.ToList()
            };

            var dr = await _producer.ProduceAsync(_topicName, message);
            // AK TODO logg when the message is delivered
            //   .ContinueWith();

            return;
        }
    }
}
