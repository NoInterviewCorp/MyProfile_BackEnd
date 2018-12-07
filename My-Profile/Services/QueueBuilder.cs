using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
// using KnowledgeGraph.Services;

namespace My_Profile.Services {
    public class QueueBuilder {
        private ConnectionFactory Factory;
        public IConnection connection { get; set; }
        public IModel Model { get; set; }
        public string ExchangeNme {
            get { return "KnowledgeGraphExchange"; }
        }
        // private UserContext dbConnection;

        public QueueBuilder () {
            // this.dbConnection = dbConnection;

            Factory = new ConnectionFactory {
                // HostName = "172.23.238.173",
                HostName = "rabbitmq",
                // Port = 8080,
                UserName = "achausername",
                Password = "strongpassword",
                DispatchConsumersAsync = true
            };

            connection = Factory.CreateConnection ();
            Model = connection.CreateModel ();
            Model.ExchangeDeclare ("KnowledgeGraphExchange", "topic");

            Model.QueueDeclare ("Profile_KnowledgeGraph_User", false, false, false, null);
            Model.QueueDeclare ("Profile_KnowledgeGraph_LearningPlanRatingWrapper", false, false, false, null);
            Model.QueueDeclare ("Profile_KnowledgeGraph_LearningPlanSubscriptionWrapper", false, false, false, null);
            // Model.QueueDeclare("QuizEngine_KnowledgeGraph_Result", false, false, false, null);
            Model.QueueDeclare ("QuizEngine_Profile_QuizData", false, false, false, null);

            // Model.QueueDeclare("Profile_KnowledgeGraph_ResourceFeedBack", false, false, false, null);
            // Model.QueueDeclare("Profile_KnowledgeGraph_QuestionFeedBack", false, false, false, null);

            Model.QueueBind ("Profile_KnowledgeGraph_User", "KnowledgeGraphExchange", "Users");
            Model.QueueBind ("Profile_KnowledgeGraph_LearningPlanRatingWrapper", ExchangeNme, "Send.LearningPlanRating");
            Model.QueueBind ("Profile_KnowledgeGraph_LearningPlanSubscriptionWrapper", ExchangeNme, "Send.LearningPlanSubscription");
            // Model.QueueBind("QuizEngine_KnowledgeGraph_Result", ExchangeNme, "Result.Update");
            Model.QueueBind ("QuizEngine_Profile_QuizData", ExchangeNme, "User.QuizData");
            // Model.QueueBind("Profile_KnowledgeGraph_LearningPlanFeedBack", ExchangeNme, "Send.LearningPlanFeedBack");
            // Model.QueueBind("Profile_KnowledgeGraph_ResourceFeedBack", ExchangeNme, "Send.ResourceFeedBack");
            // Model.QueueBind("Profile_KnowledgeGraph_QuestionFeedBack", ExchangeNme, "Send.QuestionFeedBack");
            // ListenForUser();
        }

    }
}