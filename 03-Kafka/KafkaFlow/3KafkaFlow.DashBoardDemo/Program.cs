using _2.KafkaFlow.ASPNETCoreExample;
using KafkaFlow;
using KafkaFlow.Admin.Dashboard;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KafkaConfig>(builder.Configuration.GetSection("KafkaConfig"));
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Services.AddLogging(configure => configure.AddConsole());

var server = builder.Configuration["KafkaConfig:BootstrapServers"];
var topic = builder.Configuration["KafkaConfig:Topic"];
var groupid = builder.Configuration["KafkaConfig:GroupId"];


builder.Services
    .AddKafka(kafka => kafka
        .AddCluster(cluster => cluster
                .WithBrokers(new[] { server })
                .AddConsumer(consumer => consumer
                    .Topic(topic)
                    .WithGroupId(groupid)
                    .WithWorkersCount(1)
                    .WithBufferSize(10)
                )
                .EnableTelemetry("kafka-flow.admin") // you can use the same topic used in EnableAdminMessages, if need it
                .EnableAdminMessages(
                "kafka-flow.admin" // the admin topic
            )
        ))
    .AddControllers();

builder.Services
    .AddSwaggerGen(
        c =>
        {
            c.SwaggerDoc(
                "kafka-flow",
                new OpenApiInfo
                {
                    Title = "KafkaFlow Admin",
                    Version = "kafka-flow",
                });
        });

var app = builder.Build();

app.MapControllers();
app.UseKafkaFlowDashboard();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/kafka-flow/swagger.json", "KafkaFlow Admin");
});

var kafkaBus = app.Services.CreateKafkaBus();
await kafkaBus.StartAsync();
app.MapGet("/", () => $"https://localhost:7195/kafkaflow/ <br />  https://localhost:7195/kafka-flow/ <br /> https://localhost:7195/swagger/");

await app.RunAsync();