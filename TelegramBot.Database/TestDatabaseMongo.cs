using MongoDB.Driver;
using TelegramBot.Database.DTO;

namespace TelegramBot.Database
{
    public class TestDatabaseMongo
    {
        public readonly static TestDatabaseMongo Instance = new TestDatabaseMongo();

        private IMongoCollection<UserData> _usersCollection;
        private IMongoCollection<TestCollectionData> _testsCollection;
        public TestDatabaseMongo()
        {
            var mongoClient = new MongoClient("mongodb+srv://admin:admin@cluster0.utpvn.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");

            var mongoDatabase = mongoClient.GetDatabase("TestBot");

            _usersCollection = mongoDatabase.GetCollection<UserData>("UserCollection");
            _testsCollection = mongoDatabase.GetCollection<TestCollectionData>("TestsCollection");
        }

        public Task CreateUser(string name)
        {
            var user = new UserData() { Id = Guid.NewGuid(), Name = name };
            return _usersCollection.InsertOneAsync(user);
        }

        public Task<List<UserData>> GetAllUsers()
        {
            return _usersCollection.Find(_ => true).ToListAsync();
        }

        public async Task<TestCollectionData?> GetTestById(Guid id)
        {
            return (await _testsCollection.FindAsync(x => x.Id == id)).FirstOrDefault();
        }
        
        public Task AddTest(TestCollectionData newTest)
        {
            return _testsCollection.InsertOneAsync(newTest);
        }

        public Task<List<TestCollectionData>> GetAllClientTests(long chatClientOwner)
        {
            return _testsCollection.Find(x => x.ChatIdOwner == chatClientOwner).ToListAsync();
        }

        public void DeleteTest(string testToDelete)
        {
            _testsCollection.DeleteOneAsync(x => x.Name == testToDelete);
        }
    }
}