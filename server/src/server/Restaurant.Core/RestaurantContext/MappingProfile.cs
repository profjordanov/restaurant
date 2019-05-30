using AutoMapper;
using Restaurant.Core.RestaurantContext.Commands;

namespace Restaurant.Core.RestaurantContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRestaurant, Domain.Entities.Restaurant>(MemberList.Source);
        }
    }
}