using Drivers.Api.Configurations;
using Drivers.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Drivers.Api.Services
{
    public class DriverServices
    {
        private readonly IMongoCollection<Drive> _driversCollection;

        public DriverServices(IOptions<Databasetting> databaseSettings)
        {
            var client = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = client.GetDatabase(databaseSettings.Value.DatabaseName);
            _driversCollection = database.GetCollection<Drive>(databaseSettings.Value.CollectionName);
        }

        public async Task<List<Drive>> GetAsync()
        {
            return await _driversCollection.Find(c => true).ToListAsync();
        }
        public async Task<Drive> GetDriverById(string id)
        {
            return await _driversCollection.FindAsync(new BsonDocument{{"_id", new ObjectId(id)}}).Result.FirstAsync();

        }
        public async Task InsertDriver(Drive drive)
        {
            await _driversCollection.InsertOneAsync(drive);
        }
        public async Task DeleteDriver(string id)
        {
            var filter = Builders<Drive>.Filter.Eq(s=> s.Id, id);
            await _driversCollection.DeleteOneAsync(filter); 
        }
        public async Task UpdateDriver(Drive drive)
        {
            var filter = Builders<Drive>.Filter.Eq(s=> s.Id, drive.Id);
            await _driversCollection.ReplaceOneAsync(filter, drive); 
        }
    }
}