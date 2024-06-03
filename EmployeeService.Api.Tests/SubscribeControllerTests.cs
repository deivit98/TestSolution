using EmployeeService.Server.Controllers;
using EmployeeService.Service.Services.Subscribe;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeService.Api.Tests
{
    public class SubscribeControllerTests
    {
        private Mock<ISubscribeService> _subscribeService;
        private SubscribeController _sut;

        public SubscribeControllerTests()
        {
            _subscribeService = new Mock<ISubscribeService>();
            _sut = new SubscribeController(_subscribeService.Object);
        }

        [Fact]
        public async Task Get_ShouldReturn_200()
        {
            // Arrange
            var requestDate = "1198-12-11";

            _subscribeService.Setup(x => x.Subscribe(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await _sut.Get(requestDate);

            // Arrange
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }
    }
}
