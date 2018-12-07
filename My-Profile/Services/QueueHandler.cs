using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace My_Profile.Services {
    public class QueueHandler {
        public QueueBuilder queues;
        private readonly IUserRepository _userRepository;

        public QueueHandler (QueueBuilder _queues, IUserRepository userRepository) {
            queues = _queues;
            _userRepository = userRepository;
            this.ListenForQuizResult ();
            Console.WriteLine ("------");
        }
        public void ListenForQuizResult () {
            var channel = queues.connection.CreateModel ();
            var consumer = new AsyncEventingBasicConsumer (channel);
            consumer.Received += async (model, ea) => {
                Console.WriteLine ("Consuming from the queue");
                Console.WriteLine ("-----------------------------------------------------------------------");
                channel.BasicAck (ea.DeliveryTag, false);
                var body = ea.Body;
                var userData = (UserData) body.DeSerialize (typeof (UserData));
                await _userRepository.CreateQuizResultAndRelationships (userData);
                var routingKey = ea.RoutingKey;
                Console.WriteLine ("-----------------------------------------------------------------------");
                Console.WriteLine (" - Routing Key <{0}>", routingKey);
                await Task.Yield ();
            };
            Console.WriteLine ("Consuming from QuizEngine Knowledge Graph");
            channel.BasicConsume ("QuizEngine_Profile_QuizData", false, consumer);
        }

        

    }
}