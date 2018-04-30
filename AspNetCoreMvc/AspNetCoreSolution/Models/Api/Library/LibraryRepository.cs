using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDbGenericRepository;

namespace AspNetCoreSolution.Models.Api.Library
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly IMongoDbContext _context;

        public LibraryRepository(IMongoDbContext context)
        {
            _context = context;
        }
        

        // LIB

        public async Task<LibraryViewModel> GetLibrary()
        {
            var tasks = new List<Task>();

            var starshipsTask = GetStarships();
            var weaponsTask = GetWeapons();
            var upgradesTask = GetUpgrades();

            tasks.Add(starshipsTask);
            tasks.Add(weaponsTask);
            tasks.Add(upgradesTask);

            await Task.WhenAll(tasks);

            var viewModel = new LibraryViewModel() { starships = starshipsTask.Result, weapons = weaponsTask.Result, upgrades = upgradesTask.Result };
            viewModel.ResolveVersion();
            viewModel.version += await GetDeletedItemsVersionCount();

            return viewModel;
        }


        // COUNTERS

        public async Task<int> GetNextWeaponId()
        {
            return await GetAndSaveIncrementedId("weapons");
        }

        public async Task<int> GetNextUpgradeId()
        {
            return await GetAndSaveIncrementedId("upgrades");
        }

        public async Task<int> GetNextStarshipId()
        {
            return await GetAndSaveIncrementedId("starships");
        }

        public async Task<int> GetAndSaveIncrementedId(string collectionId)
        {
            var counters = _context.GetCollection<Counter, String>();
            var filter = Builders<Counter>.Filter.Eq("Id", collectionId);

            var findResult = await counters.FindAsync(filter);
            var result = await findResult.FirstOrDefaultAsync();

            result.Count++;
            result.Version++;
            await counters.ReplaceOneAsync(filter, result);

            return result.Count;
        }

        public async Task IncreaseDeletedItemsVersionCount(int delta)
        {
            var counters = _context.GetCollection<Counter, String>();
            var filter = Builders<Counter>.Filter.Eq("Id", "deleted");

            var findResult = await counters.FindAsync(filter);
            var result = await findResult.FirstOrDefaultAsync();

            result.Count += delta;
            result.Version++;
            await counters.ReplaceOneAsync(filter, result);
        }

        public async Task<int> GetDeletedItemsVersionCount()
        {
            var counters = _context.GetCollection<Counter, String>();
            var filter = Builders<Counter>.Filter.Eq("Id", "deleted");

            var findResult = await counters.FindAsync(filter);
            var result = await findResult.FirstOrDefaultAsync();

            return result.Count;
        }


        // ADD

        public async Task AddStarship(Starship item)
        {
            await _context.GetCollection<Starship, int>().InsertOneAsync(item);
        }

        public async Task AddUpgrade(Upgrade item)
        {
            await _context.GetCollection<Upgrade, int>().InsertOneAsync(item);
        }

        public async Task AddWeapon(Weapon item)
        {
            await _context.GetCollection<Weapon, int>().InsertOneAsync(item);
        }


        // GET ONE

        public async Task<Starship> GetStarship(int id)
        {
            var filter = Builders<Starship>.Filter.Eq("Id", id);
            return await _context.GetCollection<Starship, int>().Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Upgrade> GetUpgrade(int id)
        {
            var filter = Builders<Upgrade>.Filter.Eq("Id", id);
            return await _context.GetCollection<Upgrade, int>().Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Weapon> GetWeapon(int id)
        {
            var filter = Builders<Weapon>.Filter.Eq("Id", id);
            return await _context.GetCollection<Weapon, int>().Find(filter).FirstOrDefaultAsync();
        }


        // GET MANY

        public async Task<IEnumerable<Starship>> GetStarships()
        {
            var collection = _context.GetCollection<Starship, int>();
            var query = collection.AsQueryable().OrderBy(x => x.Id);

            return await query.ToAsyncEnumerable().ToList();
        }

        public async Task<IEnumerable<Upgrade>> GetUpgrades()
        {
            var collection = _context.GetCollection<Upgrade, int>();
            var query = collection.AsQueryable().OrderBy(x => x.Id);

            return await query.ToAsyncEnumerable().ToList();
        }

        public async Task<IEnumerable<Weapon>> GetWeapons()
        {
            var collection = _context.GetCollection<Weapon, int>();
            var query = collection.AsQueryable().OrderBy(x => x.Id);

            return await query.ToAsyncEnumerable().ToList();
        }


        // REMOVE

        public async Task<DeleteResult> RemoveStarship(int id)
        {
            var filter = Builders<Starship>.Filter.Eq("Id", id);
            return await _context.GetCollection<Starship, int>().DeleteOneAsync(filter);
        }

        public async Task<DeleteResult> RemoveUpgrade(int id)
        {
            var filter = Builders<Upgrade>.Filter.Eq("Id", id);
            return await _context.GetCollection<Upgrade, int>().DeleteOneAsync(filter);
        }

        public async Task<DeleteResult> RemoveWeapon(int id)
        {
            var filter = Builders<Weapon>.Filter.Eq("Id", id);
            return await _context.GetCollection<Weapon, int>().DeleteOneAsync(filter);
        }


        // REPLACE

        public async Task<ReplaceOneResult> ReplaceStarship(int id, Starship item)
        {
            var filter = Builders<Starship>.Filter.Eq("Id", id);
            return await _context.GetCollection<Starship, int>().ReplaceOneAsync(filter, item);
        }

        public async Task<ReplaceOneResult> ReplaceUpgrade(int id, Upgrade item)
        {
            var filter = Builders<Upgrade>.Filter.Eq("Id", id);
            return await _context.GetCollection<Upgrade, int>().ReplaceOneAsync(filter, item);
        }

        public async Task<ReplaceOneResult> ReplaceWeapon(int id, Weapon item)
        {
            var filter = Builders<Weapon>.Filter.Eq("Id", id);
            return await _context.GetCollection<Weapon, int>().ReplaceOneAsync(filter, item);
        }
    }
}
