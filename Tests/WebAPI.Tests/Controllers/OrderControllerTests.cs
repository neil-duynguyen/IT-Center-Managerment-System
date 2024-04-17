using AutoFixture;
using Domain.Tests;
using FluentAssertions;
using KidProEdu.API.Controllers.Admin;
using KidProEdu.API.Controllers.Staff;
using KidProEdu.Application.ViewModels.OrderViewModelsV2;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Tests.Controllers
{
    public class OrderControllerTests : SetupTest
    {
        private readonly OrderController _orderController;
        public OrderControllerTests() {
            _orderController = new OrderController(_orderServiceMock.Object);
        }

        [Fact]
        public async void GetOrderById_ReturnOrder()
        {
            //arr
            var mocks = _fixture.Build<OrderViewModel>().CreateMany(3).ToList();
            _orderServiceMock.Setup(x => x.GetOrderByStaffId()).ReturnsAsync(mocks);

            //act
            var result = await _orderController.GetOrderByStaffId() as OkObjectResult;

            //ass
            _orderServiceMock.Verify(x => x.GetOrderByStaffId(), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
