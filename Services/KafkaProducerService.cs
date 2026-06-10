using System;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using ChineseRaffleApi.Dto;

namespace ChineseRaffleApi.Services
{
    public class KafkaProducerService : IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topicName;
        private bool _disposed;

        public KafkaProducerService(IConfiguration configuration)
        {
            var kafkaConfig = configuration.GetSection("KafkaConfig");
            
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = kafkaConfig["BootstrapServers"] ?? throw new InvalidOperationException("KafkaConfig:BootstrapServers is not configured"),
                
                // התיקון עבור עבודה מול KRaft במחשב המקומי:
                ApiVersionRequest = false,
                BrokerAddressFamily = BrokerAddressFamily.V4
            };

            _topicName = kafkaConfig["TopicName"] ?? throw new InvalidOperationException("KafkaConfig:TopicName is not configured");
            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public async Task PublishTransactionAsync(OrderTransactionMessage orderTransactionMessage)
        {
            if (orderTransactionMessage is null)
            {
                throw new ArgumentNullException(nameof(orderTransactionMessage));
            }

            var messageValue = JsonSerializer.Serialize(orderTransactionMessage);
            var message = new Message<Null, string> { Value = messageValue };

            await _producer.ProduceAsync(_topicName, message).ConfigureAwait(false);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
            _disposed = true;
        }
    }
}