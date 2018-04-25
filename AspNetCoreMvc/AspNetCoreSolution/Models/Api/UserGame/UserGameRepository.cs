using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDbGenericRepository;

namespace AspNetCoreSolution.Models.Api.UserGame
{
    public class UserGameRepository : IUserGameRepository
    {
        private readonly IMongoDbContext _context;

        public UserGameRepository(IMongoDbContext context)
        {
            _context = context;
        }

        public async Task AddUserGame(UserGame item)
        {
            await _context.GetCollection<UserGame, int>().InsertOneAsync(item);
        }

        public async Task<UserGame> GetUserGame(int id)
        {
            var filter = Builders<UserGame>.Filter.Eq("Id", id);
            return await _context.GetCollection<UserGame, int>().Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserGame>> GetUserGames(int skip = 0, int limit = 50)
        {
            var collection = _context.GetCollection<UserGame, int>();
            var query = collection.AsQueryable()
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(limit);

            return await query.ToAsyncEnumerable().ToList();
        }

        public async Task<DeleteResult> RemoveUserGame(int id)
        {
            var filter = Builders<UserGame>.Filter.Eq("Id", id);
            return await _context.GetCollection<UserGame, int>().DeleteOneAsync(filter);
        }

        public async Task<ReplaceOneResult> ReplaceUserGame(int id, UserGame item)
        {
            var filter = Builders<UserGame>.Filter.Eq("Id", id);
            return await _context.GetCollection<UserGame, int>().ReplaceOneAsync(filter, item);
        }
    }
}
