using AutoMapper;
using Restaurant.Core.RatingContext.Commands;
using Restaurant.Domain.Entities;

namespace Restaurant.Core.RatingContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RateRestaurant, Rating>(MemberList.Source);
        }
    }
}