using AutoMapper;
using Server.Entities;
using Server.Models;

namespace Server.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SignalModel, Signal>()
                .ReverseMap();
        }
    }
}
