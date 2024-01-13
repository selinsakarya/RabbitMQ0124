using System.Text;
using RabbitMQ.Client;

ConnectionFactory factory = new ConnectionFactory
{
    Uri = new Uri("amqp://guest:guest@localhost:5672/")
};

using (IConnection connection = factory.CreateConnection())
{
    using (IModel channel = connection.CreateModel())
    {
        channel.QueueDeclare("BasicTest", false, false, false);

        string message = "selin";

        byte[] body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(
            exchange: string.Empty,
            routingKey: "BasicTest",
            basicProperties: null,
            body: body);

        Console.WriteLine($"Sent message {message}...");
    }
}

Console.ReadLine();
