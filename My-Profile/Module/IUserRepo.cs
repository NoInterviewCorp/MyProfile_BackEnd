using System.Collections.Generic;
using System.Threading.Tasks;
namespace My_Profile
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUser(MongoDB.Bson.ObjectId id);
        Task<bool> GetStatus(MongoDB.Bson.ObjectId id, string resourceId);
        Task<bool> PostNote(User user);
        Task<bool> FindNote(MongoDB.Bson.ObjectId id);
        Task UserResFind(Status status);
        Task Create(User user);
        Task<bool> Update(User user);
        Task<bool> Delete(MongoDB.Bson.ObjectId id);
    }
}