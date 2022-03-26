using AutoMapper;
using TelegramBot.InternalAPI.Databases.DTO;
using TelegramBot.InternalAPI.Domain;

namespace TelegramBot.AutoMapper
{
    public sealed class DbProfile : Profile
    {
        public DbProfile()
        {
            CreateMap<TestCollection, TestCollectionData>().ReverseMap();
            CreateMap<TestStep, TestStepData>().ReverseMap();
        }
    }
}
