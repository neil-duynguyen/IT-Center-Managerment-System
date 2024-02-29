﻿using AutoMapper;
using KidProEdu.Application.ViewModels.BlogViewModels;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
using KidProEdu.Application.ViewModels.ClassViewModels;
using KidProEdu.Application.ViewModels.CourseViewModels;
using KidProEdu.Application.ViewModels.DivisionViewModels;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using KidProEdu.Application.ViewModels.LessonViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.LoginViewModel;
using KidProEdu.Application.ViewModels.NotificationUserViewModels;
using KidProEdu.Application.ViewModels.NotificationViewModels;
using KidProEdu.Application.ViewModels.QuestionViewModels;
using KidProEdu.Application.ViewModels.RatingViewModels;
using KidProEdu.Application.ViewModels.RequestViewModels;
using KidProEdu.Application.ViewModels.RoleViewModels;
using KidProEdu.Application.ViewModels.RoomViewModels;
using KidProEdu.Application.ViewModels.SemesterCourseViewModels;
using KidProEdu.Application.ViewModels.SemesterViewModels;
using KidProEdu.Application.ViewModels.TagViewModels;
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
            CreateMap<UserAccount, UpdateUserViewModel>().ReverseMap();
            CreateMap<UserViewModel, UserAccount>().ReverseMap().ForMember(des => des.RoleName, src => src.MapFrom(x => x.Role.Name));

            CreateMap<CreateTagViewModel, Tag>().ReverseMap();
            CreateMap<UpdateTagViewModel, Tag>().ReverseMap();

            CreateMap<CreateLocationViewModel, Location>().ReverseMap();
            CreateMap<UpdateLocationViewModel, Location>().ReverseMap();

            CreateMap<CreateCategoryEquipmentViewModel, CategoryEquipment>().ReverseMap();
            CreateMap<UpdateCategoryEquipmentViewModel, CategoryEquipment>().ReverseMap();

            CreateMap<CreateSemesterViewModel, Semester>().ReverseMap();
            CreateMap<UpdateSemesterViewModel, Semester>().ReverseMap();

            CreateMap<RoomViewModel, Room>().ReverseMap().ForMember(des => des.Status, src => src.MapFrom(x => x.Status != null ? (string)x.Status.ToString() : (string?)null));
            CreateMap<CreateRoomViewModel, Room>().ReverseMap();
            CreateMap<UpdateRoomViewModel, Room>().ReverseMap();

            CreateMap<CreateChildrenViewModel, ChildrenProfile>().ReverseMap();
            CreateMap<UpdateChildrenViewModel, ChildrenProfile>().ReverseMap();

            CreateMap<CreateEquipmentViewModel, Equipment>().ReverseMap();
            CreateMap<UpdateEquipmentViewModel, Equipment>().ReverseMap();         

            CreateMap<BlogViewModel, Blog>().ReverseMap().ForMember(des => des.Tags, src => src.MapFrom(x => x.Tags.Select(x => x.TagName)))
                                                          .ForMember(des => des.Author, src => src.MapFrom(x => x.UserAccount.FullName));
            CreateMap<CreateBlogViewModel, Blog>().ReverseMap();
            CreateMap<UpdateBlogViewModel, Blog>().ReverseMap();


            CreateMap<CreateNotificationViewModel, Notification>().ReverseMap();
                //.ForMember(dest => dest.NotificationUser, opt => opt.MapFrom(src => src.CreateNotificationUserViewModels)).ReverseMap();
            CreateMap<CreateNotificationUserViewModel, NotificationUser>().ReverseMap();
            CreateMap<Notification, NotificationWithUserViewModel>().ReverseMap();

            CreateMap<Rating, RatingViewModel>()
                .ForMember(des => des.CourseName, src => src.MapFrom(x => x.Course.Name)).ReverseMap();
            CreateMap<CreateRatingViewModel, Rating>().ReverseMap();
            CreateMap<UpdateRatingViewModel, Rating>().ReverseMap();

            CreateMap<Division, DivisionViewModel>().ReverseMap();
            CreateMap<CreateDivisionViewModel, Division>().ReverseMap();
            CreateMap<UpdateDivisionViewModel, Division>().ReverseMap();

            CreateMap<Lesson, LessonViewModel>().ReverseMap();
            CreateMap<CreateLessonViewModel, Lesson>().ReverseMap();
            CreateMap<UpdateLessonViewModel, Lesson>().ReverseMap();
            
            CreateMap<QuestionViewModel, Question>().ReverseMap();
            CreateMap<CreateQuestionViewModel, Question>().ReverseMap();
            CreateMap<UpdateQuestionViewModel, Question>().ReverseMap();
            
            CreateMap<RequestViewModel, Request>().ReverseMap().ForMember(des => des.Status, src => src.MapFrom(x => x.Status != null ? (string)x.Status.ToString() : (string?)null));
            CreateMap<CreateRequestViewModel, Request>().ReverseMap();
            CreateMap<UpdateRequestViewModel, Request>().ReverseMap();

            CreateMap<CreateCourseViewModel, Course>().ReverseMap();
            CreateMap<CourseViewModel, Course>().ReverseMap().ForMember(des => des.CourseType, src => src.MapFrom(x => x.CourseType != null ? (string)x.CourseType.ToString() : (string?)null));

            CreateMap<ClassViewModel, Class>().ReverseMap().ForMember(des => des.StatusOfClass, src => src.MapFrom(x => x.StatusOfClass != null ? (string)x.StatusOfClass.ToString() : (string?)null));
            CreateMap<CreateClassViewModel, Class>().ReverseMap();
            CreateMap<UpdateClassViewModel, Class>().ReverseMap();

            CreateMap<SemesterCourse, CreateSemesterCourseViewModel>().ReverseMap();
        }
    }
}
