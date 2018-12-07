using System.Collections.Generic;
using System.Threading.Tasks;
namespace My_Profile {
    public interface IUserRepository {
        Task<List<User>> GetAllUsers ();
        Task<User> GetUser (string id);
        Task<bool> GetStatus (string id, string resourceId);
        Task<bool> PostNote (User user);
        Task<bool> FindNote (string id);
        Task UserResFind (Status status);
        Task Create (string id, User user);
        Task<bool> Update (string id, User user);
        Task<bool> Delete (string id);
        Task CreateQuizResultAndRelationships (UserData userData);
        Task<UserData> GetQuizResult (string id);
    }
}