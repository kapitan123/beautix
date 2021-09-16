using Confluent.Kafka;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiPoller.Services.Messaging
{
    public class MessageProducer : IMessageProducer
    {
        private string _topicName = "Products batch Upsert";
        private IProducer<Null, List<string>> _producer;

        public MessageProducer(IProducer<Null, List<string>> producer)
        {
            _producer = producer;
        }

        public async Task NotifyOnProductsUpsert(List<string> productIds)
        {
            var message = new Message<Null, List<string>>()
            {
                Value = productIds
            };

            var dr = await _producer.ProduceAsync(_topicName, message);
            // AK TODO logg when the message is delivered
            //   .ContinueWith();

            return;
        }
    }
}
