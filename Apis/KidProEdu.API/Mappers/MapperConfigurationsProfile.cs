using AutoMapper;
using KidProEdu.Application.ViewModels.AdviseRequestViewModels;
using KidProEdu.Application.ViewModels.AttendanceViewModels;
using KidProEdu.Application.ViewModels.BlogViewModels;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.CertificateViewModel;
using KidProEdu.Application.ViewModels.ChildrenAnswerViewModels;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
using KidProEdu.Application.ViewModels.ClassViewModels;
using KidProEdu.Application.ViewModels.ConfigJobType;
using KidProEdu.Application.ViewModels.ContractViewModels;
using KidProEdu.Application.ViewModels.CourseViewModels;
using KidProEdu.Application.ViewModels.DivisionUserAccountViewModels;
using KidProEdu.Application.ViewModels.DivisionViewModels;
using KidProEdu.Application.ViewModels.EnrollmentViewModels;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using KidProEdu.Application.ViewModels.ExamViewModels;
using KidProEdu.Application.ViewModels.FeedBackViewModels;
using KidProEdu.Application.ViewModels.LessonViewModels;
using KidProEdu.Application.ViewModels.LocationViewModel;
using KidProEdu.Application.ViewModels.LogEquipmentViewModels;
using KidProEdu.Application.ViewModels.LoginViewModel;
using KidProEdu.Application.ViewModels.NotificationUserViewModels;
using KidProEdu.Application.ViewModels.NotificationViewModels;
using KidProEdu.Application.ViewModels.OrderDetailViewModels;
using KidProEdu.Application.ViewModels.OrderViewModelsV2;
using KidProEdu.Application.ViewModels.QuestionViewModels;
using KidProEdu.Application.ViewModels.RatingViewModels;
using KidProEdu.Application.ViewModels.RequestViewModels;
using KidProEdu.Application.ViewModels.ResourceViewModels;
using KidProEdu.Application.ViewModels.RoleViewModels;
using KidProEdu.Application.ViewModels.RoomViewModels;
using KidProEdu.Application.ViewModels.ScheduleViewModels;
using KidProEdu.Application.ViewModels.SkillCertificateViewModels;
using KidProEdu.Application.ViewModels.SkillViewModels;
using KidProEdu.Application.ViewModels.SlotViewModels;
using KidProEdu.Application.ViewModels.TagViewModels;
using KidProEdu.Application.ViewModels.TransactionViewModels;
using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;

namespace KidProEdu.API.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {

            CreateMap<Role, RoleViewModel>().ReverseMap();

            CreateMap<UserAccount, LoginViewModel>().ReverseMap();
            CreateMap<UserAccount, CreateUserViewModel>().ReverseMap();
            CreateMap<UserAccount, UpdateUserViewModel>().ReverseMap();
            CreateMap<UserViewModel, UserAccount>().ReverseMap().ForMember(des => des.RoleName, src => src.MapFrom(x => x.Role.Name))
                                                                .ForMember(des => des.LocationName, src => src.MapFrom(x => x.Location.Name));

            CreateMap<CreateTagViewModel, Tag>().ReverseMap();
            CreateMap<UpdateTagViewModel, Tag>().ReverseMap();

            CreateMap<CreateLocationViewModel, Location>().ReverseMap();
            CreateMap<UpdateLocationViewModel, Location>().ReverseMap();

            CreateMap<CreateSkillViewModel, Skill>().ReverseMap();
            CreateMap<UpdateSkillViewModel, Skill>().ReverseMap();
            CreateMap<Skill, SkillViewModel>().ReverseMap();

            CreateMap<CreateSkillCertificateViewModel, SkillCertificate>().ReverseMap();
            CreateMap<UpdateSkillCertificateViewModel, SkillCertificate>().ReverseMap();
            CreateMap<SkillCertificate, SkillCertificateViewModel>().ReverseMap();

            CreateMap<CategoryEquipment, CategoryEquipmentViewModel>().ReverseMap().ForMember(des => des.TypeCategoryEquipment, src => src.MapFrom(x => x.TypeCategoryEquipment != null ? (string)x.TypeCategoryEquipment.ToString() : (string?)null));
            CreateMap<CreateCategoryEquipmentViewModel, CategoryEquipment>().ReverseMap();
            CreateMap<UpdateCategoryEquipmentViewModel, CategoryEquipment>().ReverseMap();

            CreateMap<RoomViewModel, Room>().ReverseMap().ForMember(des => des.Status, src => src.MapFrom(x => x.Status != null ? (string)x.Status.ToString() : (string?)null));
            CreateMap<CreateRoomViewModel, Room>().ReverseMap();
            CreateMap<UpdateRoomViewModel, Room>().ReverseMap();
            CreateMap<RoomForScheduleViewModel, Room>().ReverseMap();

            CreateMap<CreateChildrenViewModel, ChildrenProfile>().ReverseMap();
            CreateMap<UpdateChildrenViewModel, ChildrenProfile>().ReverseMap();
            CreateMap<ChildrenViewModel, ChildrenProfile>().ReverseMap();
            CreateMap<ChildrenProfile, ChildrenProfileViewModel>();

            CreateMap<CreateEquipmentViewModel, Equipment>().ReverseMap();
            CreateMap<UpdateEquipmentViewModel, Equipment>().ReverseMap();
            CreateMap<Equipment, EquipmentViewModel>().ReverseMap();
            CreateMap<Equipment, EquipmentByIdViewModel>()
                .ForMember(dest => dest.UserAccountId, opt => opt.MapFrom(src => src.LogEquipments.FirstOrDefault().UserAccountId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.LogEquipments.FirstOrDefault().UserAccount.UserName))
                .ForMember(dest => dest.BorrowedDate, opt => opt.MapFrom(src => src.LogEquipments.FirstOrDefault().BorrowedDate))
                .ForMember(dest => dest.RepairDate, opt => opt.MapFrom(src => src.LogEquipments.FirstOrDefault().RepairDate))
                .ForMember(dest => dest.ReturnedDate, opt => opt.MapFrom(src => src.LogEquipments.FirstOrDefault().ReturnedDate))
                .ForMember(dest => dest.ReturnedDealine, opt => opt.MapFrom(src => src.LogEquipments.FirstOrDefault().ReturnedDealine))
                .ReverseMap();

            CreateMap<EquipmentManagementViewModel, Equipment>().ReverseMap();

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

            CreateMap<LessonViewModel, Lesson>().ReverseMap().ForMember(des => des.CategoryEquipmentsName, src => src.MapFrom(x => x.CategoryEquipments.Select(x => x.Name)));
            CreateMap<CreateLessonViewModel, Lesson>().ReverseMap();
            CreateMap<UpdateLessonViewModel, Lesson>().ReverseMap();

            CreateMap<QuestionViewModel, Question>().ReverseMap().ForMember(des => des.Type, src => src.MapFrom(x => x.Type != null ? (string)x.Type.ToString() : (string?)null));
            CreateMap<CreateQuestionViewModel, Question>().ReverseMap();
            CreateMap<UpdateQuestionViewModel, Question>().ReverseMap();
            CreateMap<Question, Question2ViewModel>().ReverseMap()
                .ForMember(des => des.Type, src => src.MapFrom(x => x.Type != null ? (string)x.Type.ToString() : (string?)null));

            CreateMap<RequestViewModel, Request>().ReverseMap();
                //.ForMember(des => des.CreatedBy, src => src.MapFrom(x => x.RequestUserAccounts.);
            //CreateMap<CreateRequestViewModel, Request>().ReverseMap();
            CreateMap<UpdateRequestViewModel, Request>().ReverseMap();

            CreateMap<CreateCourseViewModel, Course>().ReverseMap();
            CreateMap<CourseViewModelById, Course>().ReverseMap();
            CreateMap<CreateCourseParentViewModel, Course>().ReverseMap();
            CreateMap<UpdateCourseViewModel, Course>().ReverseMap();
            CreateMap<UpdateCourseParentViewModel, Course>().ReverseMap();
            CreateMap<CourseViewModel, Course>().ReverseMap().ForMember(des => des.CourseType, src => src.MapFrom(x => x.CourseType != null ? (string)x.CourseType.ToString() : (string?)null));

            CreateMap<ClassViewModel, Class>().ReverseMap()
                                    .ForMember(des => des.StatusOfClass, src => src.MapFrom(x => x.StatusOfClass != null ? (string)x.StatusOfClass.ToString() : (string?)null))
                                    .ForMember(des => des.CourseCode, src => src.MapFrom(x => x.Course.CourseCode));
            CreateMap<CreateClassViewModel, Class>().ReverseMap();
            CreateMap<UpdateClassViewModel, Class>().ReverseMap();
            CreateMap<ClassForScheduleViewModel, Class>().ReverseMap();
            //.ForMember(des => des.TotalDuration, src => src.MapFrom(x => x.Course.DurationTotal));

            //CreateMap<RequestUserAccount, CreateRequestUserAccountViewModel>().ReverseMap();

            CreateMap<Resource, ResourceViewModel>().ReverseMap();
            CreateMap<CreateResourceViewModel, Resource>().ReverseMap();
            CreateMap<UpdateResourceViewModel, Resource>().ReverseMap();

            CreateMap<Exam, ExamViewModel>().ReverseMap();
            CreateMap<CreateExamViewModel2, Exam>().ReverseMap();
            CreateMap<UpdateExamViewModel, Exam>().ReverseMap();
            CreateMap<CreateExamFinalPracticeViewModel, Exam>().ReverseMap();

            CreateMap<LogEquipment, LogEquipmentViewModel>()
                .ForMember(dest => dest.FullName, src => src.MapFrom(x => x.UserAccount.FullName))
                .ForMember(dest => dest.Status, src => src.MapFrom(x => x.Status != null ? (string)x.Status.ToString() : (string?)null))
                .ForMember(dest => dest.LogType, src => src.MapFrom(x => x.LogType != null ? (string)x.LogType.ToString() : (string?)null))
                .ReverseMap();
            CreateMap<CreateLogEquipmentViewModel, LogEquipment>().ReverseMap();
            CreateMap<UpdateLogEquipmentViewModel, LogEquipment>().ReverseMap();

            CreateMap<TagViewModel, Tag>().ReverseMap().ForMember(des => des.TagType, src => src.MapFrom(x => x.TagType != null ? (string)x.TagType.ToString() : (string?)null)); ;

            CreateMap<AdviseRequestViewModel, AdviseRequest>().ReverseMap()
                .ForMember(des => des.StatusAdviseRequest, src => src.MapFrom(x => x.StatusAdviseRequest != null ? (string)x.StatusAdviseRequest.ToString() : (string?)null))
                //.ForMember(x => x.Location, src => src.MapFrom(x => x.Location.Name))
                //.ForMember(x => x.Slot, src => src.MapFrom(x => x.Slot.Name))
                ;
            CreateMap<CreateAdviseRequestViewModel, AdviseRequest>().ReverseMap();
            CreateMap<UpdateAdviseRequestViewModel, AdviseRequest>().ReverseMap();

            CreateMap<DivisionUserAccount, DivisionUserAccountViewModel>().ReverseMap();
            CreateMap<CreateDivisionUserAccountViewModel, DivisionUserAccount>().ReverseMap();
            CreateMap<UpdateDivisionUserAccountViewModel, DivisionUserAccount>().ReverseMap();

            CreateMap<EnrollmentViewModel, Enrollment>().ReverseMap().ForMember(des => des.ClassCode, src => src.MapFrom(x => x.Class.ClassCode))
                                                                    .ForMember(des => des.ChildrenName, src => src.MapFrom(x => x.ChildrenProfile.FullName));
            CreateMap<CreateEnrollmentViewModel, Enrollment>().ReverseMap()
                .ForMember(dest => dest.ChildrenProfileIds, opt => opt.MapFrom(src => src.ChildrenProfileId));
            CreateMap<UpdateEnrollmentViewModel, Enrollment>().ReverseMap();
            CreateMap<ClassChildrenViewModel, Enrollment>().ReverseMap()
                                                            .ForMember(x => x.Code, src => src.MapFrom(x => x.Class.ClassCode))
                                                            //.ForMember(x => x.chid, src => src.MapFrom(x => x.ChildrenProfileId))
                                                            .ForMember(x => x.Avatar, src => src.MapFrom(x => x.ChildrenProfile.Avatar))
                                                            .ForMember(x => x.FullName, src => src.MapFrom(x => x.ChildrenProfile.FullName))
                                                            .ForMember(x => x.ChildrenCode, src => src.MapFrom(x => x.ChildrenProfile.ChildrenCode));

            CreateMap<Contract, ContractViewModel>().ReverseMap()
                .ForMember(des => des.StatusOfContract, src => src.MapFrom(x => x.StatusOfContract != null ? (string)x.StatusOfContract.ToString() : (string?)null));
            CreateMap<CreateContractViewModel, Contract>().ReverseMap();
            CreateMap<UpdateContractViewModel, Contract>().ReverseMap();

            CreateMap<ConfigJobTypeViewModel, ConfigJobType>().ReverseMap()
                .ForMember(des => des.JobType, src => src.MapFrom(x => x.JobType != null ? (string)x.JobType.ToString() : (string?)null));
            CreateMap<UpdateConfigJobTypeViewModel, ConfigJobType>().ReverseMap();

            CreateMap<Schedule, ScheduleViewModel>().ReverseMap();
            CreateMap<CreateScheduleViewModel, Schedule>().ReverseMap();
            CreateMap<UpdateScheduleViewModel, Schedule>().ReverseMap();
            CreateMap<ScheduleForAutoViewModel, Schedule>().ReverseMap();
            //.ForMember(des => des.SlotForSchedule, src=>src.MapFrom(x=>x.Slot));

            CreateMap<CreateAttendanceViewModel, Attendance>().ReverseMap();
            CreateMap<UpdateAttendanceViewModel, Attendance>()
                .ForMember(des => des.StatusAttendance, src => src.MapFrom(x => x.StatusAttendance != null ? (string)x.StatusAttendance.ToString() : (string?)null));
            CreateMap<Attendance, AttendanceViewModel>().ReverseMap()
                .AfterMap((src, dest) =>
                {
                    dest.ChildrenProfile.UserId = src.ChildrenProfile.UserId;
                    dest.ChildrenProfile.FullName = src.ChildrenProfile.FullName;
                    dest.ChildrenProfile.Avatar = src.ChildrenProfile.Avatar;
                    dest.ChildrenProfile.BirthDay = src.ChildrenProfile.BirthDay;
                    dest.ChildrenProfile.GenderType = src.ChildrenProfile.GenderType;
                    dest.ChildrenProfile.SpecialSkill = src.ChildrenProfile.SpecialSkill;
                })
                .ForMember(des => des.StatusAttendance, src => src.MapFrom(x => x.StatusAttendance != null ? (string)x.StatusAttendance.ToString() : (string?)null)).ReverseMap();
            CreateMap<Attendance, AttendanceDetailsViewModel>()
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Schedule.Class.Course.Id))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Schedule.Class.Course.Name))
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.Schedule.Class.Course.CourseCode))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Slot, opt => opt.MapFrom(src => src.Schedule.Slot.Name))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Schedule.Slot.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.Schedule.Slot.EndTime))
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Schedule.ScheduleRooms.FirstOrDefault().Room.Name))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Schedule.Class.TeachingClassHistories.FirstOrDefault().UserAccount.FullName))
                .ForMember(dest => dest.ClassCode, opt => opt.MapFrom(src => src.Schedule.Class.ClassCode))
                .ForMember(dest => dest.TeacherComment, opt => opt.MapFrom(src => src.Note))
                .ForMember(dest => dest.AttendanceStatus, opt => opt.MapFrom(src => src.StatusAttendance != null ? src.StatusAttendance.ToString() : null));
            CreateMap<Attendance, AttendanceWithChildrenProfileViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ChildrenName, opt => opt.MapFrom(src => src.ChildrenProfile.FullName))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.ChildrenProfile.Avatar))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
                .ForMember(dest => dest.StatusAttendance, opt => opt.MapFrom(src => src.StatusAttendance != null ? src.StatusAttendance.ToString() : null));

            CreateMap<OrderViewModel, Order>().ReverseMap().ForMember(des => des.PaymentStatus, src => src.MapFrom(x => x.PaymentStatus != null ? (string)x.PaymentStatus.ToString() : (string?)null))
                                                            .ForMember(des => des.FullName, src => src.MapFrom(x => x.UserAccount.FullName));

            CreateMap<OrderDetailViewModel, OrderDetail>().ReverseMap()
                .ForMember(des => des.OrderDetailId, src => src.MapFrom(x => x.Id))
                .ForMember(des => des.PayType, src => src.MapFrom(x => x.PayType != null ? (string)x.PayType.ToString() : (string?)null))
                .ForMember(des => des.CourseCode, src => src.MapFrom(x => x.Course.CourseCode))
                .ForMember(des => des.ChildrenName, src => src.MapFrom(x => x.ChildrenProfile.FullName))
                .ForMember(des => des.ParentId, src => src.MapFrom(x => x.Order.UserAccount.Id))
                .ForMember(des => des.ParentName, src => src.MapFrom(x => x.Order.UserAccount.FullName))
                .ForMember(des => des.EWalletMethod, src => src.MapFrom(x => x.Order.EWalletMethod));

            CreateMap<ChildrenAnswer, ChildrenAnswerViewModel>().ReverseMap();
            CreateMap<CreateChildrenAnswerViewModel, ChildrenAnswer>().ReverseMap();
            CreateMap<UpdateChildrenAnswerViewModel, ChildrenAnswer>().ReverseMap();

            CreateMap<Feedback, FeedBackViewModel>().ReverseMap();
            CreateMap<CreateFeedBackViewModel, Feedback>().ReverseMap();
            CreateMap<UpdateFeedBackViewModel, Feedback>().ReverseMap();

            CreateMap<TransactionViewModel, Transaction>().ReverseMap()
                                                .ForMember(des => des.StatusTransaction, src => src.MapFrom(x => x.StatusTransaction != null ? (string)x.StatusTransaction.ToString() : (string?)null));

            CreateMap<SlotForScheduleViewModel, Slot>().ReverseMap()
                .ForMember(des => des.SlotType, src => src.MapFrom(x => x.SlotType != null ? (string)x.SlotType.ToString() : (string?)null))
                .ReverseMap();


            CreateMap<CreateCertificateViewModel, Certificate>().ReverseMap();
            CreateMap<CertificateViewModel, Certificate>().ReverseMap();
        }
    }
}
