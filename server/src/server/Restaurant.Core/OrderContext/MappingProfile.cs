using AutoMapper;
using Restaurant.Core.OrderContext.Commands;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enumerations;
using Restaurant.Domain.Views.Order;
using System;

namespace Restaurant.Core.OrderContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MakeNewOrder, Order>(MemberList.Source)
                .ForMember(dest => dest.CreatedOn, opts => opts.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.OrderStatus, opts => opts.MapFrom(_ => OrderStatus.Pending));

            CreateMap<Order, PendingOrderView>()
                .ForMember(dest => dest.Meal, opts => opts.MapFrom(src => src.Meal));
        }
    }
}