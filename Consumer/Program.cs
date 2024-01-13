using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory = new ConnectionFactory
{
    Uri = new Uri("amqp://guest:guest@localhost:5672/")
};

using (IConnection connection = factory.CreateConnection())
{
    using (IModel channel = connection.CreateModel())
    {
        channel.QueueDeclare("BasicTest", false, false, false, null);

        EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

        consumer.Received += (_, eventArgs) =>
        {
            byte[] body = eventArgs.Body.ToArray();

            string message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"Received message: {message}");
        };

        channel.BasicConsume("BasicTest", autoAck: true, consumer);
    }
}

Console.ReadLine();