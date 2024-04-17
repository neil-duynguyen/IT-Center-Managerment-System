using AutoFixture;
using Domain.Tests;
using FluentAssertions;
using KidProEdu.API.Controllers;
using KidProEdu.API.Controllers.Manager;
using KidProEdu.Application.ViewModels.AdviseRequestViewModels;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Controllers
{
    public class AdviseRequestControllerTests : SetupTest
    {
        private readonly AdviseRequestController _adviseRequestController;

        public AdviseRequestControllerTests() {
            _adviseRequestController = new AdviseRequestController(_adviseRequestServiceMock.Object);
        }

        [Fact]
        public async void GetAllAdviseRequest_ReturnList()
        {
            //arr
            var mocks = _fixture.Build<AdviseRequestViewModel>().CreateMany(3).ToList();
            _adviseRequestServiceMock.Setup(x => x.GetAdviseRequests()).ReturnsAsync(mocks);

            //act
            var result = await _adviseRequestController.AdviseRequests() as OkObjectResult;

            //ass
            _adviseRequestServiceMock.Verify(x => x.GetAdviseRequests(), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetAdviseRequestById_ReturnAdviseRequest()
        {
            //arr
            var mocks = _fixture.Build<AdviseRequestViewModel>().Create();
            _adviseRequestServiceMock.Setup(x => x.GetAdviseRequestById(It.IsAny<Guid>())).ReturnsAsync(mocks);

            //act
            var result = await _adviseRequestController.AdviseRequest(It.IsAny<Guid>()) as OkObjectResult;

            //ass
            _adviseRequestServiceMock.Verify(x => x.GetAdviseRequestById(It.IsAny<Guid>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void Delete_AdviseRequestById_ChangeIsDeleteTrue()
        {
            //arr
            Guid id = Guid.NewGuid();
            _adviseRequestServiceMock.Setup(x => x.DeleteAdviseRequest(id)).ReturnsAsync(true);

            //act
            var result = await _adviseRequestController.DeleteAdviseRequest(id) as OkObjectResult;

            //ass
            _adviseRequestServiceMock.Verify(x => x.DeleteAdviseRequest(id), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
