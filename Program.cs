using System;
using Confluent.Kafka;

namespace KafkaConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "foo",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe("quickstart-events");

                while (true)
                {
                    var consumeResult = consumer.Consume(5000);

                    if (consumeResult != null)
                    {
                        Console.WriteLine($"New message on {consumeResult.Topic}: {consumeResult.Message.Value}");
                    }
                }
            }
        }
    }
}
