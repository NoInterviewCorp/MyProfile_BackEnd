using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
namespace My_Profile
{
    public class User
    {
        [BsonId]
        public MongoDB.Bson.ObjectId UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }


    }
    public class Settings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
    public class Resource
    {
        public string ResourceId { get; set; }
        public bool isCheck { get; set; }


    }
    public class Status
    {
        [BsonIgnoreIfDefault]
        public ObjectId _id { get; set; }
        public List<Resource> Resources { get; set; }
        public string UserId { get; set; }
    }
}