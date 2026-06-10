using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChineseRaffleApi.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly string _topicName;
        private readonly ILogger<KafkaConsumerService> _logger;

        public KafkaConsumerService(IConfiguration configuration, ILogger<KafkaConsumerService> logger)
        {
            _logger = logger;

            var kafkaConfig = configuration.GetSection("KafkaConfig");
            var bootstrapServers = kafkaConfig["BootstrapServers"] ?? throw new InvalidOperationException("KafkaConfig:BootstrapServers is not configured");
            _topicName = kafkaConfig["TopicName"] ?? throw new InvalidOperationException("KafkaConfig:TopicName is not configured");

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = "chinese-raffle-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                
                // התיקון עבור עבודה מול KRaft במחשב המקומי:
                ApiVersionRequest = false,
                BrokerAddressFamily = BrokerAddressFamily.V4
            };

            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
        }

        private void StartConsumerLoop(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_topicName);
            _logger.LogInformation($"[Kafka Consumer] Started listening to topic: {_topicName}");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);
                    _logger.LogInformation($"[Kafka Consumer] New Message Received: {consumeResult.Message.Value}");
                }
                catch (ConsumeException e)
                {
                    _logger.LogError($"[Kafka Consumer] Error occurred: {e.Error.Reason}");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        public override void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
            base.Dispose();
        }
    }
}