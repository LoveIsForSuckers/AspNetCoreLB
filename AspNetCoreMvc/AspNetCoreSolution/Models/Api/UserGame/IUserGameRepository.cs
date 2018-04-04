using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSolution.Models.Api.UserGame
{
    public interface IUserGameRepository
    {
        Task<IEnumerable<UserGame>> GetUserGames();
        Task<UserGame> GetUserGame(int id);
        Task AddUserGame(UserGame item);
        Task<DeleteResult> RemoveUserGame(int id);
        Task<ReplaceOneResult> ReplaceUserGame(int id, UserGame item);
    }
}
