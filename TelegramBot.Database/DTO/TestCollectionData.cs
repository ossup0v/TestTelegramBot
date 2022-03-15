using MongoDB.Bson.Serialization.Attributes;

namespace TelegramBot.Database.DTO
{
    public class TestCollectionData
    {
        [BsonId]
        public Guid Id { get; set; }
        public long ChatIdOwner { get; set; }
        public string Name { get; set; }
        public TestStepData[] Steps { get; set; }
    }

    public class TestStepData
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
