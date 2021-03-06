using MongoDB.Bson.Serialization.Attributes;

namespace TelegramBot.InternalAPI.Databases.DTO
{
    public class UserData
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
