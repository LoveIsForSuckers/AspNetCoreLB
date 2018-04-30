using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSolution.Models.Api.Library
{
    public interface ILibraryRepository
    {
        Task<LibraryViewModel> GetLibrary();

        Task<int> GetNextStarshipId();
        Task<IEnumerable<Starship>> GetStarships();
        Task<Starship> GetStarship(int id);
        Task AddStarship(Starship item);
        Task<DeleteResult> RemoveStarship(int id);
        Task<ReplaceOneResult> ReplaceStarship(int id, Starship item);

        Task<int> GetNextWeaponId();
        Task<IEnumerable<Weapon>> GetWeapons();
        Task<Weapon> GetWeapon(int id);
        Task AddWeapon(Weapon item);
        Task<DeleteResult> RemoveWeapon(int id);
        Task<ReplaceOneResult> ReplaceWeapon(int id, Weapon item);

        Task<int> GetNextUpgradeId();
        Task<IEnumerable<Upgrade>> GetUpgrades();
        Task<Upgrade> GetUpgrade(int id);
        Task AddUpgrade(Upgrade item);
        Task<DeleteResult> RemoveUpgrade(int id);
        Task<ReplaceOneResult> ReplaceUpgrade(int id, Upgrade item);

        Task<int> GetDeletedItemsVersionCount();
        Task IncreaseDeletedItemsVersionCount(int delta);
    }
}
