using AutoFixture;
using Domain.Tests;
using FluentAssertions;
using KidProEdu.API.Controllers;
using KidProEdu.API.Controllers.Admin;
using KidProEdu.Application.ViewModels.ChildrenViewModels;
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
    public class ChildrenControllerTests : SetupTest
    {
        private readonly ChildrenController _childrenController;
        public ChildrenControllerTests() {
            _childrenController = new ChildrenController(_childrenServiceMock.Object);
        }

        [Fact]
        public async void GetChildrenById_ReturnChildren()
        {
            //arr
            var mocks = _fixture.Build<ChildrenViewModel>().Create();
            _childrenServiceMock.Setup(x => x.GetChildrenById(It.IsAny<Guid>())).ReturnsAsync(mocks);

            //act
            var result = await _childrenController.GetChildrenById(It.IsAny<Guid>()) as OkObjectResult;

            //ass
            _childrenServiceMock.Verify(x => x.GetChildrenById(It.IsAny<Guid>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetChildrenByParentId_ReturnChildren()
        {
            //arr
            var mocks = _fixture.Build<ChildrenViewModel>().CreateMany(2).ToList();
            _childrenServiceMock.Setup(x => x.GetChildrenByParentId(It.IsAny<Guid>())).ReturnsAsync(mocks);

            //act
            var result = await _childrenController.GetChildrenByParentId(It.IsAny<Guid>()) as OkObjectResult;

            //ass
            _childrenServiceMock.Verify(x => x.GetChildrenByParentId(It.IsAny<Guid>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
