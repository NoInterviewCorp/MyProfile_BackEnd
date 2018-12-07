using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using My_Profile.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace My_Profile.Services
{
    public class QueueBuilder
    {
        private ConnectionFactory Factory;
        public IConnection connection { get; set; }
        public IModel Model { get; set; }
        public string ExchangeName
        {
            get { return "KnowledgeGraphExchange"; }
        }
        private AsyncEventingBasicConsumer consumer;
        private IBasicProperties properties;
        private BlockingCollection<UserReport> responseQueue = new BlockingCollection<UserReport>();
        private string replyQueueName = "UserReport_Response";

        public QueueBuilder()
        {

            Factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "achausername",
                Password = "strongpassword",
                DispatchConsumersAsync = true
            };

            connection = Factory.CreateConnection();
            Model = connection.CreateModel();
            Model.ExchangeDeclare("KnowledgeGraphExchange", "topic");
            Model.QueueDeclare("QuizEngine_Profile_QuizData", false, false, false, null);
            Model.QueueBind("QuizEngine_Profile_QuizData", ExchangeName, "User.QuizData");
        }
        public UserReport GetUserReport(string UserId)
        {
            // Initializing the connection
            consumer = new AsyncEventingBasicConsumer(Model);
            properties = Model.CreateBasicProperties();
            var correlationId = "1"; // Guid.NewGuid().ToString("N");
            properties.CorrelationId = correlationId;
            properties.ReplyTo = "Response.Report";

            // Initialising the reciever
            consumer.Received += async (model, ea) =>
            {
                Console.WriteLine("Response Report Recieved");
                try
                {
                    var body = ea.Body;
                    var response = (UserReport)body.DeSerialize(typeof(UserReport));
                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        Console.WriteLine($"Adding Report for user -> {response.UserId} to the queue");
                        responseQueue.Add(response);
                    }
                }
                catch (Exception e)
                {
                    ConsoleWriter.ConsoleAnException(e);
                }
                await Task.Yield();
            };

            // Preparing message and publishing it
            Console.WriteLine($"Sending Report Generation Request for ID: " + UserId);
            var messageBytes = UserId.Serialize();
            Model.BasicPublish(
                exchange: ExchangeName,
                routingKey: "Request.Report",
                basicProperties: properties,
                body: messageBytes);

            // Starting to listen for the response
            Model.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);
            Console.Write("Taking from BlockingCollection with count " + responseQueue.Count);
            return responseQueue.Take();
        }
        public void Close()
        {
            connection.Close();
            responseQueue.Dispose();
        }
    }
}
