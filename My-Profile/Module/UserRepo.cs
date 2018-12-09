using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace My_Profile
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserContext _context;
        public UserRepository(IUserContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllUsers()
        {
            return await _context
                .Users
                .Find(_ => true)
                .ToListAsync();
        }
        public Task<User> GetUser(string id)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(m => m.UserId, id);
            return _context
                .Users
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> GetStatus(string id, string resourceId)
        {
            // string x = "214";
            FilterDefinition<Status> filter = Builders<Status>.Filter.Eq(m => m.StatusId, id);
            var userStutus = await _context.Status.Find(filter).FirstAsync();
            //   Console.WriteLine("---"+stu.Resources[0].ResourceId);
            for (int i = 0; i < userStutus.Resources.Count; i++)
            {
                if (userStutus.Resources[i].ResourceId == resourceId)
                {

                    return true;

                }
            }
            return false;
        }

        public async Task Create(string id, User user)
        {
            user.UserId = id;
            await _context.Users.InsertOneAsync(user);
        }
        public async Task<bool> PostNote(User user)
        {
            //  FilterDefinition<User> filter = Builders<User>.Filter.Eq(m => m.Id==user.Id);
            bool exists = await _context.Users.Find((n => n.UserName == user.UserName)).AnyAsync();
            if (exists)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<bool> FindNote(string id)
        {
            //  FilterDefinition<User> filter = Builders<User>.Filter.Eq(m => m.Id==user.Id);
            bool exists = await _context.Users.Find((n => n.UserId == id)).AnyAsync();
            if (exists)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<bool> Update(string id, User user)
        {
            user.UserId = id;
            ReplaceOneResult updateResult =
                await _context
                .Users
                .ReplaceOneAsync(
                    filter: g => g.UserId == user.UserId,
                    replacement: user);
            return updateResult.IsAcknowledged &&
                updateResult.ModifiedCount > 0;
        }
        public async Task<bool> Delete(string id)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(m => m.UserId, id);
            DeleteResult deleteResult = await _context
                .Users
                .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged &&
                deleteResult.DeletedCount > 0;
        }
        public async Task UserResFind(Status status)
        {
            var UserExists = await _context.Status.Find("{UserId:\"" + status.UserId + "\"}").Limit(1).ToListAsync();
            // bool UserExists = await _context.Status.Find((n => n.UserId == status.UserId)).AnyAsync();
            if (UserExists.Count == 0)
            {
                //bool ResourceExists=await _context.Status.Find((n => n.Resources. == status.UserId)).AnyAsync();
                await _context.Status.InsertOneAsync(status);
                UserExists.Add(status);
            }
            else
            {
                bool ResourceExists = (UserExists[0].Resources.Find(r => r.ResourceId == status.Resources[0].ResourceId)) != null;
                // bool exists=
                if (ResourceExists)
                {
                    var res = UserExists[0].Resources.Remove(UserExists[0].Resources.Find(r => r.ResourceId == status.Resources[0].ResourceId));
                    // Console.WriteLine(res);

                }
                else
                {

                    UserExists[0].Resources.Add(status.Resources[0]);
                    //Console.WriteLine("ture");
                    // return true;
                }
                var filter = Builders<Status>.Filter.Eq(s => s.UserId, status.UserId);
                var options = new UpdateOptions { IsUpsert = true };
                await _context.Status.ReplaceOneAsync(filter, UserExists[0], options);
                //   return true;
            }
        }
        public async Task CreateQuizResultAndRelationships(UserData userData)
        {
            await _context.QuizResult.InsertOneAsync(userData);
        }
        public async Task<List<UserData>> GetQuizResult(string id)
        {
            FilterDefinition<UserData> filter = Builders<UserData>.Filter.Eq(m => m.UserId, id);
            return await _context
                .QuizResult
                .Find(filter)
                .ToListAsync();
        }

    }
}