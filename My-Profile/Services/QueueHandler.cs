using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace My_Profile.Services
{
    public class QueueHandler
    {
        public QueueBuilder queues;
        

        public QueueHandler(QueueBuilder _queues)
        {
            queues = _queues;
            // this.HandleLearningPlanFromQueue();
            // this.HandleResourceFromQueue();
            // this.ListenForUser();
            // this.ListenForLeaningPlanRating();
            // this.ListenForResourceFeedBack();
            // this.ListenForLeaningPlanSubscriber();
            // this.ListenForLeaningPlanUnSubscriber();
            // this.ListenForQuestionFeedBack();
            // this.QuestionBatchRequestHandler();
            Console.WriteLine("------");
            //  this.QuizEngineQueueHandler();
        }

       
    }
}