using AutoFixture;
using Domain.Tests;
using FluentAssertions;
using KidProEdu.API.Controllers.Admin;
using KidProEdu.API.Controllers.Manager;
using KidProEdu.Application.ViewModels.RoomViewModels;
using KidProEdu.Application.ViewModels.RoomViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Controllers
{
    public class RoomControllerTests : SetupTest
    {
        private readonly RoomController _roomController;
        public RoomControllerTests() {
            _roomController = new RoomController(_roomServiceMock.Object);
        }

        [Fact]
        public async void GetAllRoom_ReturnList()
        {
            //arr
            var mocks = _fixture.Build<RoomViewModel>().CreateMany(3).ToList();
            _roomServiceMock.Setup(x => x.GetRooms()).ReturnsAsync(mocks);

            //act
            var result = await _roomController.Rooms() as OkObjectResult;

            //ass
            _roomServiceMock.Verify(x => x.GetRooms(), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetRoomById_ReturnRoom()
        {
            //arr
            var mocks = _fixture.Build<RoomViewModel>().Create();
            _roomServiceMock.Setup(x => x.GetRoomById(It.IsAny<Guid>())).ReturnsAsync(mocks);

            //act
            var result = await _roomController.Room(It.IsAny<Guid>()) as OkObjectResult;

            //ass
            _roomServiceMock.Verify(x => x.GetRoomById(It.IsAny<Guid>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void Delete_CorrectId_ChangeIsDeleteTrue()
        {
            //arr
            Guid id = Guid.NewGuid();
            _roomServiceMock.Setup(x => x.DeleteRoom(id)).ReturnsAsync(true);

            //act
            var result = await _roomController.DeleteRoom(id) as OkObjectResult;

            //ass
            _roomServiceMock.Verify(x => x.DeleteRoom(id), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
