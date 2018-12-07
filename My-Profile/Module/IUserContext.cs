using MongoDB.Driver;
namespace My_Profile {
    public interface IUserContext {
        IMongoCollection<User> Users { get; }
        IMongoCollection<Status> Status { get; }
        IMongoCollection<UserData> QuizResult { get; }
    }
}