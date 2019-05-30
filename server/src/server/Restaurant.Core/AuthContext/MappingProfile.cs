using AutoMapper;
using Restaurant.Core.AuthContext.Commands;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Views.Auth;
using System;

namespace Restaurant.Core.AuthContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Register, User>(MemberList.Source)
                .ForMember(d => d.UserName, opts => opts.MapFrom(s => s.Email))
                .ForMember(dest => dest.RegistrationDate, opts => opts.MapFrom(src => DateTime.UtcNow));

            CreateMap<User, UserView>(MemberList.Destination);
        }
    }
}