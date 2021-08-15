using System;
using System.Collections.Generic;
using Confluent.Kafka;
using Newtonsoft.Json;

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
                consumer.Subscribe(new List<string> {"quickstart-events", "person-events"});

                while (true)
                {
                    var consumeResult = consumer.Consume(5000);

                    if (consumeResult != null)
                    {
                        string message = "";
                        if (consumeResult.Topic == "person-events")
                        {
                            var person = JsonConvert.DeserializeObject<Person>(consumeResult.Message.Value);
                            message = person.ToString();
                        }
                        else
                        {
                            message = consumeResult.Message.Value;
                        }

                        Console.WriteLine($"New message on {consumeResult.Topic} at {consumeResult.Message.Timestamp.UtcDateTime.ToString("o")}: {message}");
                    }
                }
            }
        }
    }
}
