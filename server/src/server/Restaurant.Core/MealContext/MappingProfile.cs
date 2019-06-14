using AutoMapper;
using Restaurant.Core.MealContext.Commands;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Views.Meal;

namespace Restaurant.Core.MealContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterMeal, Meal>(MemberList.Source);

            CreateMap<Meal, MealView>(MemberList.Destination);
        }
    }
}