using AutoFixture;
using Domain.Tests;
using FluentAssertions;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.Domain.Entities;
using Moq;

namespace Application.Tests.Services
{
    public class UserServiceTests : SetupTest
    {
        private readonly IUserService _userService;
        private readonly Fixture _customFixture;

        public UserServiceTests()
        {
            _userService = new UserService(_mapperConfig, _unitOfWorkMock.Object,_claimsServiceMock.Object);
            ;
            _customFixture = new Fixture();
            _customFixture.Customizations.Add(new TypePropertyOmitter(
                "Role",
                    "AdviseRequests", "Feedbacks", "TeachingClassHistories", "Location", "ChildrenProfile",
                    "Enrollments", "NotificationUsers", "Blogs", "Contracts", "RequestUserAccounts", "LogEquipments",
                    "SkillCertificate", "DivisionUserAccounts"
                ));
        }

        /*[Fact]
        public async Task GetAllUser_ShouldReturnCorrentData()
        {
            //arrange
            var mocks = _customFixture.Build<UserAccount>().CreateMany(5).ToList();

            var expectedResult = _mapperConfig.Map<List<UserViewModel>>(mocks);

            _unitOfWorkMock.Setup(x => x.UserRepository.GetAllAsync()).ReturnsAsync(mocks);

            //act
            //var result = await _userService.GetAllUser();

            //assert
            //_unitOfWorkMock.Verify(x => x.UserRepository.GetAllAsync(), Times.Once());
            mocks.Should().NotBeEmpty();
        }*/
    }
}
