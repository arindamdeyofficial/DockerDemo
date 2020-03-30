using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Helpers
{
    public class RabbitMqHelper
    {
        public Lazy<RabbitMqHelper> _rabitMq = new Lazy<RabbitMqHelper>();
        public static string _queueName;
        public static ConnectionFactory _connectionFactory;
        public static IConnection _rabbitConnection;
        public static IModel _channel;
        public static EventingBasicConsumer _consumer;
        static RabbitMqHelper()
        {
            _queueName = "testqueue";
            try
            { 
            _connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672,
                RequestedConnectionTimeout = 3000, // milliseconds
            };
            _rabbitConnection = _connectionFactory.CreateConnection();
            _channel = _rabbitConnection.CreateModel();
            // Declaring a queue is idempotent 
            _channel.QueueDeclare(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _consumer = new EventingBasicConsumer(_channel);            
            }
            catch (Exception ex)
            {
                _rabbitConnection.Close();
            }
        }
        public static async Task RabbitMqPutMessageAsync()
        {
            try
            {
                string body = String.Format("message");
                _channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: _queueName,
                    basicProperties: null,
                    body: Encoding.UTF8.GetBytes(body));
                Console.WriteLine("Message written...{0}", body);
            }
            catch (Exception ex)
            {
            }   
        }
        public static async Task RabbitMqReadMessageAsync()
        {
            const string _queueName = "testqueue";

            try
            {
                Console.WriteLine("reading Started...");
                _consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}...Press [enter] to exit.", message);
                };
                _channel.BasicConsume(queue: _queueName,
                                     autoAck: true,
                                     consumer: _consumer);
            }
            catch (Exception ex)
            {
            }
        }
        public static async Task RabbitMqCloseAsync()
        {
            try
            {
                _rabbitConnection.Close();
                Console.WriteLine("RabbitMq Connection Closed");
                Console.Read();
            }
            catch (Exception ex)
            {
            }
            Console.Read();
        }
    }
}
