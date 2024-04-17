using AutoFixture;
using Domain.Tests;
using FluentAssertions;
using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Controllers
{
    public class UserControllerTests : SetupTest
    {
        private readonly UserController _userController;
        public UserControllerTests() { 
            _userController = new UserController(_userServicesMock.Object);
        }

        [Fact]
        public async void GetAllUser_ReturnList()
        {
            //arr
            var mocks = _fixture.Build<UserViewModel>().CreateMany(3).ToList();
            _userServicesMock.Setup(x => x.GetAllUser()).ReturnsAsync(mocks);

            //act
            var result = await _userController.UserAsync() as OkObjectResult;

            //ass
            _userServicesMock.Verify(x => x.GetAllUser(), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetUserById_ReturnUser()
        {
            //arr
            var mocks = _fixture.Build<UserViewModel>().Create();
            _userServicesMock.Setup(x => x.GetUserById(It.IsAny<Guid>())).ReturnsAsync(mocks);

            //act
            var result = await _userController.UserAsync(It.IsAny<Guid>()) as OkObjectResult;

            //ass
            _userServicesMock.Verify(x => x.GetUserById(It.IsAny<Guid>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void Delete_CorrectId_ChangeIsDeleteTrue()
        {
            //arr
            Guid id = Guid.NewGuid();
            _userServicesMock.Setup(x => x.DeleteUser(id)).ReturnsAsync(true);

            //act
            var result = await _userController.DeleteUser(id) as OkObjectResult;

            //ass
            _userServicesMock.Verify(x => x.DeleteUser(id), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
