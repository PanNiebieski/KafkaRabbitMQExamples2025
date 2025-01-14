# NuGet Packages Example Project

This project demonstrates the use of various NuGet packages in a .NET 9 application. The packages integrated into this project include:

- **Confluent.Kafka**: For working with Apache Kafka, providing a high-performance and reliable client.
- **KafkaFlow**: A library for simplifying Kafka-based event-driven architecture.
- **EasyNetQ**: An easy-to-use library for RabbitMQ integration.
- **RabbitMQ.Client**: The official RabbitMQ client library for .NET.
- **Rebus.RabbitMq**: Event-driven library for RabbitMQ added to this repository as Bonus

## Table of Contents

1. [Introduction](#introduction)
2. [Setup and Installation](#setup-and-installation)
3. [NuGet Packages Overview](#nuget-packages-overview)
4. [Usage Examples](#usage-examples)
5. [Contributing](#contributing)
6. [License](#license)

## Introduction

This repository is designed to serve as a reference for integrating various messaging libraries using NuGet packages in a .NET 9 application. It showcases how to:

- Produce and consume messages with Apache Kafka using `Confluent.Kafka` and `KafkaFlow`.
- Send and receive messages with RabbitMQ using `EasyNetQ` and `RabbitMQ.Client`.

## Setup and Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/nuget-packages-example.git
   ```

2. Navigate to the project directory:

   ```bash
   cd nuget-packages-example
   ```

3. Restore dependencies:

   ```bash
   dotnet restore
   ```

4. Build the project:

   ```bash
   dotnet build
   ```

## NuGet Packages Overview

### Confluent.Kafka
- **Purpose**: Kafka client library for .NET.
- **Use Case**: Producing and consuming Kafka messages.
- **Documentation**: [Confluent.Kafka](https://github.com/confluentinc/confluent-kafka-dotnet)

### KafkaFlow
- **Purpose**: A framework for creating Kafka-based event-driven applications.
- **Use Case**: Simplifying Kafka consumers and producers with pipelines and middlewares.
- **Documentation**: [KafkaFlow](https://github.com/Farfetch/kafkaflow)

### EasyNetQ
- **Purpose**: Simplified RabbitMQ client for .NET.
- **Use Case**: Abstracting RabbitMQ's AMQP model for straightforward usage.
- **Documentation**: [EasyNetQ](https://github.com/EasyNetQ/EasyNetQ)

### RabbitMQ.Client
- **Purpose**: Official RabbitMQ client library.
- **Use Case**: Advanced configurations and messaging workflows with RabbitMQ.
- **Documentation**: [RabbitMQ.Client](https://github.com/rabbitmq/rabbitmq-dotnet-client)

## Usage Examples

### Kafka Example (Using Confluent.Kafka)
```csharp
var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
using var producer = new ProducerBuilder<Null, string>(config).Build();
await producer.ProduceAsync("test-topic", new Message<Null, string> { Value = "Hello Kafka" });
```

### KafkaFlow Example
```csharp
services.AddKafkaFlow(kafka =>
    kafka.UseConsoleLog()
         .AddCluster(cluster => cluster
             .WithBrokers(new[] { "localhost:9092" })
             .AddProducer("producer-name", producer => producer
                 .DefaultTopic("test-topic"))));
```

### RabbitMQ Example (Using EasyNetQ)
```csharp
using var bus = RabbitHutch.CreateBus("host=localhost");
bus.PubSub.Publish(new MyMessage { Text = "Hello RabbitMQ" });
```

### RabbitMQ Example (Using RabbitMQ.Client)
```csharp
var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false);
channel.BasicPublish(exchange: "", routingKey: "hello", body: Encoding.UTF8.GetBytes("Hello RabbitMQ"));
```

## Contributing

Contributions are welcome! If you find any issues or want to add new examples, feel free to create a pull request or open an issue.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

