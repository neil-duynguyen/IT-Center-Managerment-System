using AutoFixture;
using Domain.Tests;
using FluentAssertions;
using KidProEdu.API.Controllers.Admin;
using KidProEdu.API.Controllers.Manager;
using KidProEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Controllers
{
    public class LocationControllerTests : SetupTest
    {
        private readonly LocationController _locationController;
        private readonly Fixture _customFixture;

        public LocationControllerTests()
        {
            _locationController = new LocationController(_locationServiceMock.Object);
            _customFixture = new Fixture();
            _customFixture.Customizations.Add(new TypePropertyOmitter("UserAccount", "AdviseRequests"));
        }

        [Fact]
        public async void GetAllLocation_ReturnList()
        {
            //arr
            var mocks = _customFixture.Build<Location>().CreateMany(3).ToList();
            _locationServiceMock.Setup(x => x.GetLocations()).ReturnsAsync(mocks);

            //act
            var result = await _locationController.Locations() as OkObjectResult;

            //ass
            _locationServiceMock.Verify(x => x.GetLocations(), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetLocationById_ReturnLocation()
        {
            //arr
            var mocks = _customFixture.Build<Location>().Create();
            _locationServiceMock.Setup(x => x.GetLocationById(It.IsAny<Guid>())).ReturnsAsync(mocks);

            //act
            var result = await _locationController.Location(It.IsAny<Guid>()) as OkObjectResult;

            //ass
            _locationServiceMock.Verify(x => x.GetLocationById(It.IsAny<Guid>()), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void Delete_CorrectId_ChangeIsDeleteTrue()
        {
            //arr
            Guid id = Guid.NewGuid();
            _locationServiceMock.Setup(x => x.DeleteLocation(id)).ReturnsAsync(true);

            //act
            var result = await _locationController.DeleteLocation(id) as OkObjectResult;

            //ass
            _locationServiceMock.Verify(x => x.DeleteLocation(id), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
