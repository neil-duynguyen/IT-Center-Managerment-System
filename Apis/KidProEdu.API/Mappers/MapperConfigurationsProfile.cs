using AutoMapper;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.LoginViewModel;
using KidProEdu.Application.ViewModels.RoleViewModels;
using KidProEdu.Application.ViewModels.TagViewModels;
using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.Domain.Entities;

namespace KidProEdu.API.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile() { 

            CreateMap<Role, RoleViewModel>().ReverseMap();

            CreateMap<User, LoginViewModel>().ReverseMap();
            CreateMap<User, CreateUserViewModel>().ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<CreateTagViewModel, Tag>().ReverseMap();
            CreateMap<UpdateTagViewModel, Tag>().ReverseMap();
            CreateMap<CreateLocationViewModel, Location>().ReverseMap();
            CreateMap<UpdateLocationViewModel, Location>().ReverseMap();
            CreateMap<CreateCategoryEquipmentViewModel, CategoryEquipment>().ReverseMap();
            CreateMap<UpdateCategoryEquipmentViewModel, CategoryEquipment>().ReverseMap();
        }
    }
}
