using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.InternalAPI.Databases.DTO;

namespace TelegramBot.InternalAPI.Databases
{
    public interface ITestDatabase
    {
        Task<List<TestCollectionData>> GetAllClientTests(long chatClientOwner);
        Task AddTest(TestCollectionData newTest);
        Task<TestCollectionData?> GetTestById(Guid id);
        Task DeleteTest(string testToDelete);
    }
}
