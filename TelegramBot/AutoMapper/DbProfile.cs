using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Database.DTO;
using TelegramBot.Test;

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
