using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.MappingProfile
{
    public class DepartmentProfile :Profile
    {
        public DepartmentProfile()
        {
                CreateMap<Department ,DepartmentViewModel>().ReverseMap();   
        }
    }
}
