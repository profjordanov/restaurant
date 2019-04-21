using AutoMapper;
using Restaurant.Core.MealContext.Commands;
using Restaurant.Domain.Entities;

namespace Restaurant.Core.MealContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterMeal, Meal>(MemberList.Source);
        }
    }
}