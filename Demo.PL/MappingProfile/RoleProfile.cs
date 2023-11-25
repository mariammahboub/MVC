using AutoMapper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Demo.PL.MappingProfile
{
    public class RoleProfile :Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleViewModel, IdentityRole>()
                .ForMember(d =>d.Name ,O => O.MapFrom(S =>S.RoleName))
                .ReverseMap();
        }
    }
}
