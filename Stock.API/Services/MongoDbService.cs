using MongoDB.Driver;

namespace Stock.API.Services
{
    public class MongoDbService
    {
        readonly IMongoDatabase _mongoDatabase;

        public MongoDbService(IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _mongoDatabase = client.GetDatabase("OrchestrationStockDb");
        }

        public IMongoCollection<T> GetCollection<T>() => _mongoDatabase.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
    }
}
