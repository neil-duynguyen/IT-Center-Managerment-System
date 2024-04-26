using AutoFixture;
using Domain.Tests;
using FluentAssertions;
using KidProEdu.API.Controllers.Admin;
using KidProEdu.API.Controllers.Manager;
using KidProEdu.Application.ViewModels.CourseViewModels;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
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
    public class EquipmentControllerTests : SetupTest
    {
        private readonly EquipmentController _equipmentController;
        public EquipmentControllerTests() {
            _equipmentController = new EquipmentController(_equipmentServiceMock.Object);
        }

        [Fact]
        public async void GetAllEquipment_ReturnList()
        {
            //arr
            var mocks = _fixture.Build<EquipmentViewModel>().CreateMany(3).ToList();
            _equipmentServiceMock.Setup(x => x.GetEquipments()).ReturnsAsync(mocks);

            //act
            var result = await _equipmentController.Equipments() as OkObjectResult;

            //ass
            _equipmentServiceMock.Verify(x => x.GetEquipments(), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetEquipmentById_ReturnEquipment()
        {
            //arr
            var mocks = _fixture.Build<EquipmentByIdViewModel>().Create();
            _equipmentServiceMock.Setup(x => x.GetEquipmentById(It.IsAny<Guid>())).ReturnsAsync(mocks);

            //act
            var result = await _equipmentController.Equipment(It.IsAny<Guid>()) as OkObjectResult;

            //ass
            _equipmentServiceMock.Verify(x => x.GetEquipmentById(It.IsAny<Guid>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void Delete_EquipmentId_ChangeIsDeleteTrue()
        {
            //arr
            Guid id = Guid.NewGuid();
            _equipmentServiceMock.Setup(x => x.DeleteEquipment(id)).ReturnsAsync(true);

            //act
            var result = await _equipmentController.DeleteEquipment(id) as OkObjectResult;

            //ass
            _equipmentServiceMock.Verify(x => x.DeleteEquipment(id), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
