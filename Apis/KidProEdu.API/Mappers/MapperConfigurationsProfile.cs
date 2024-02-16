using AutoMapper;
using KidProEdu.Application.ViewModels.BlogTagViewModels;
using KidProEdu.Application.ViewModels.BlogViewModels;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.LoginViewModel;
using KidProEdu.Application.ViewModels.RoleViewModels;
using KidProEdu.Application.ViewModels.RoomViewModels;
using KidProEdu.Application.ViewModels.SemesterViewModels;
using KidProEdu.Application.ViewModels.TagViewModels;
using KidProEdu.Application.ViewModels.TrainingProgramCategoryViewModels;
using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.Domain.Entities;

namespace KidProEdu.API.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile() { 

            CreateMap<Role, RoleViewModel>().ReverseMap();

            CreateMap<UserAccount, LoginViewModel>().ReverseMap();
            CreateMap<UserAccount, CreateUserViewModel>().ReverseMap();
            CreateMap<UserAccount, UserViewModel>().ReverseMap();

            CreateMap<CreateTagViewModel, Tag>().ReverseMap();
            CreateMap<UpdateTagViewModel, Tag>().ReverseMap();

            CreateMap<CreateLocationViewModel, Location>().ReverseMap();
            CreateMap<UpdateLocationViewModel, Location>().ReverseMap();

            CreateMap<CreateCategoryEquipmentViewModel, CategoryEquipment>().ReverseMap();
            CreateMap<UpdateCategoryEquipmentViewModel, CategoryEquipment>().ReverseMap();

            CreateMap<CreateSemesterViewModel, Semester>().ReverseMap();
            CreateMap<UpdateSemesterViewModel, Semester>().ReverseMap();

            CreateMap<CreateRoomViewModel, Room>().ReverseMap();
            CreateMap<UpdateRoomViewModel, Room>().ReverseMap();

            CreateMap<CreateChildrenViewModel, ChildrenProfile>().ReverseMap();
            CreateMap<UpdateChildrenViewModel, ChildrenProfile>().ReverseMap();

            CreateMap<CreateEquipmentViewModel, Equipment>().ReverseMap();
            CreateMap<UpdateEquipmentViewModel, Equipment>().ReverseMap();

            CreateMap<CreateTrainingProgramCategoryViewModel, TrainingProgramCategory>().ReverseMap();
            CreateMap<UpdateTrainingProgramCategoryViewModel, TrainingProgramCategory>().ReverseMap();

            CreateMap<CreateBlogViewModel, Blog>().ReverseMap();
            CreateMap<UpdateBlogViewModel, Blog>().ReverseMap();

            CreateMap<CreateBlogTagViewModel, BlogTag>().ReverseMap();
            CreateMap<UpdateBlogTagViewModel, BlogTag>().ReverseMap();

        }
    }
}
