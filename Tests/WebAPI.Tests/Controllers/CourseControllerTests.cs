using AutoFixture;
using Domain.Tests;
using FluentAssertions;
using KidProEdu.API.Controllers.Admin;
using KidProEdu.Application.ViewModels.CourseViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Controllers
{
    public class CourseControllerTests : SetupTest
    {
        private readonly CourseController _courseController;

        public CourseControllerTests()
        {
            _courseController = new CourseController(_courseServiceMock.Object);
        }

        [Fact]
        public async void GetAllCourse_ReturnList()
        {
            //arr
            var mocks = _fixture.Build<CourseViewModel>().CreateMany(3).ToList();
            _courseServiceMock.Setup(x => x.GetAllCourse()).ReturnsAsync(mocks);

            //act
            var result = await _courseController.GetAllCourse() as OkObjectResult;

            //ass
            _courseServiceMock.Verify(x => x.GetAllCourse(), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetCourseById_ReturnCourse()
        {
            //arr
            var mocks = _fixture.Build<CourseViewModelById>().Create();
            _courseServiceMock.Setup(x => x.GetCourseById(It.IsAny<Guid>())).ReturnsAsync(mocks);

            //act
            var result = await _courseController.GetCourseById(It.IsAny<Guid>()) as OkObjectResult;

            //ass
            _courseServiceMock.Verify(x => x.GetCourseById(It.IsAny<Guid>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void DeleteCorrectById_ChangeIsDeleteTrue()
        { 
            //arr
            Guid id = Guid.NewGuid();
            _courseServiceMock.Setup(x => x.DeleteCourseAsync(id)).ReturnsAsync(true);

            //act
            var result = await _courseController.DeleteCourse(id) as OkObjectResult;

            //ass
            _courseServiceMock.Verify(x => x.DeleteCourseAsync(id), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void Delete_InvalidCourse_ReturnBadRequest()
        {
            //arr
            Guid id = Guid.NewGuid();
            _courseServiceMock.Setup(x => x.DeleteCourseAsync(id)).ReturnsAsync(false);

            //act
            var result = await _courseController.DeleteCourse(id) as BadRequestObjectResult;

            //ass
            _courseServiceMock.Verify(x => x.DeleteCourseAsync(id), Times.Once);
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
