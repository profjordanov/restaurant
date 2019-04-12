using AutoMapper;
using Restaurant.Core.AuthContext.Commands;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Views.Auth;

namespace Restaurant.Core.AuthContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Register, User>(MemberList.Source)
                .ForMember(d => d.UserName, opts => opts.MapFrom(s => s.Email));

            CreateMap<User, UserView>(MemberList.Destination);
        }
    }
}