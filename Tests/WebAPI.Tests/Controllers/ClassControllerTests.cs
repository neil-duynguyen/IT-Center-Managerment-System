using AutoFixture;
using Domain.Tests;
using FluentAssertions;
using KidProEdu.API.Controllers.Admin;
using KidProEdu.API.Controllers.Manager;
using KidProEdu.Application.ViewModels.ClassViewModels;
using KidProEdu.Application.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Controllers
{
    public class ClassControllerTests : SetupTest
    {
        private readonly ClassController _classController;

        public ClassControllerTests()
        {
            _classController = new ClassController(_classServiceMock.Object);
        }

        [Fact]
        public async void GetListClass_ReturnList()
        {
            //arr
            var mocks = _fixture.Build<ClassViewModel>().CreateMany(3).ToList();
            _classServiceMock.Setup(x => x.GetClasses()).ReturnsAsync(mocks);

            //act
            var result = await _classController.Classes() as OkObjectResult;

            //ass
            _classServiceMock.Verify(x => x.GetClasses(), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetClassById_ReturnClass()
        {
            //arr
            var mocks = _fixture.Build<ClassViewModel>().Create();
            _classServiceMock.Setup(x => x.GetClassById(It.IsAny<Guid>())).ReturnsAsync(mocks);

            //act
            var result = await _classController.Class(It.IsAny<Guid>()) as OkObjectResult;

            //ass
            _classServiceMock.Verify(x => x.GetClassById(It.IsAny<Guid>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void Delete_CorrectId_ChangeIsDeleteTrue()
        {
            //arr
            Guid id = Guid.NewGuid();
            _classServiceMock.Setup(x => x.DeleteClass(id)).ReturnsAsync(true);

            //act
            var result = await _classController.DeleteClass(id) as OkObjectResult;

            //ass
            _classServiceMock.Verify(x => x.DeleteClass(id), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
