using MongoDB.Driver;
using Microsoft.Extensions.Options;
namespace My_Profile
{
    public class UserContext : IUserContext
    {
        private readonly IMongoDatabase _db;
        public UserContext(IOptions<Settings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _db = client.GetDatabase(options.Value.Database);
            Status.Indexes.CreateOne(new CreateIndexModel<Status>("{ UserId : 1 }"));
        }
        public IMongoCollection<User> Users => _db.GetCollection<User>("Users");
        public IMongoCollection<Status> Status => _db.GetCollection<Status>("status");
         public IMongoCollection<UserData> QuizResult => _db.GetCollection<UserData>("QuizResult");

        
    }
}
