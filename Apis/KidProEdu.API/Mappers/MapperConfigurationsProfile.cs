using AutoMapper;
using KidProEdu.Application.ViewModels.LoginViewModel;
using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.Domain.Entities;

namespace KidProEdu.API.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile() { 
            CreateMap<User, LoginViewModel>().ReverseMap();
            CreateMap<User, CreateUserViewModel>().ReverseMap();
        }
    }
}
