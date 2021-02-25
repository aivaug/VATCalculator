using HomeworkProject.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Services.VATServices;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests.ControllerTests
{
    public class VATControllerTests
    {
        private readonly IVATService _vatService;
        private readonly VATController _controller;
        public VATControllerTests()
        {
            _vatService = Substitute.For<IVATService>();
            _controller = new VATController(_vatService);
        }

        [Fact]
        public async Task GetFullPrice_CalculatePrice_ReturnsOKObjectDecimalValue()
        {
            //Arrange
            int providerId = 1;
            int clientId = 2;
            decimal price = 10;
            _vatService.GetVATFullPrice(providerId, clientId, price).Returns(price);

            //Act
            var result = await _controller.GetFullPrice(providerId, clientId, price);
            var okObjectResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okObjectResult);
            Assert.True(okObjectResult.Value is decimal);
            Assert.Equal(price, okObjectResult.Value);
        }

        [Fact]
        public async Task GetFullPrice_InvalidProviderId_ReturnsBadRequestObjectErrorMessage()
        {
            //Arrange
            int providerId = 0;
            int clientId = 2;
            decimal price = 10;
            string resultMessage = "Please specify valid members properties";

            //Act
            var result = await _controller.GetFullPrice(providerId, clientId, price);
            var badRequestResult = result as BadRequestObjectResult;

            //Assert
            Assert.NotNull(badRequestResult);
            Assert.True(badRequestResult.Value.Equals(resultMessage));
        }

        [Fact]
        public async Task GetFullPrice_InvalidClientId_ReturnsBadRequestObjectErrorMessage()
        {
            //Arrange
            int providerId = 1;
            int clientId = 0;
            decimal price = 10;
            string resultErrorMessage = "Please specify valid members properties";

            //Act
            var result = await _controller.GetFullPrice(providerId, clientId, price);
            var badRequestResult = result as BadRequestObjectResult;

            //Assert
            Assert.NotNull(badRequestResult);
            Assert.True(badRequestResult.Value.ToString() == resultErrorMessage);
        }

        [Fact]
        public async Task GetFullPrice_NegativePrice_ReturnsBadRequestObjectErrorMessage()
        {
            //Arrange
            int providerId = 1;
            int clientId = 2;
            decimal price = -1;
            string resultErrorMessage = "Please specify valid price";

            //Act
            var result = await _controller.GetFullPrice(providerId, clientId, price);
            var badRequestResult = result as BadRequestObjectResult;

            //Assert
            Assert.NotNull(badRequestResult);
            Assert.True(badRequestResult.Value.ToString() == resultErrorMessage);
        }

        [Fact]
        public async Task GetFullPrice_ZeroPrice_ReturnsOkObjectZeroPrice()
        {
            //Arrange
            int providerId = 1;
            int clientId = 2;
            decimal price = 0;

            //Act
            var result = await _controller.GetFullPrice(providerId, clientId, price);
            var okObjectResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okObjectResult);
            Assert.Equal(0, okObjectResult.Value);
        }

        [Fact]
        public async Task GetFullPrice_NotExistingProvider_ReturnsNotFoundWithErrorMessage()
        {
            //Arrange
            int providerId = 1;
            int clientId = 2;
            decimal price = 10;
            _vatService.GetVATFullPrice(providerId, clientId, price).Returns<Task<decimal>>(x => { throw new ArgumentException($"Existing provider with id {providerId} not found"); });

            //Act
            var result = await _controller.GetFullPrice(providerId, clientId, price);
            var notFoundObjectResult = result as NotFoundObjectResult;

            //Assert
            Assert.NotNull(notFoundObjectResult);
            Assert.Equal($"Existing provider with id {providerId} not found", notFoundObjectResult.Value );
        }

        [Fact]
        public async Task GetFullPrice_NotExistingClient_ReturnsNotFoundWithErrorMessage()
        {
            //Arrange
            int providerId = 1;
            int clientId = 2;
            decimal price = 10;
            _vatService.GetVATFullPrice(providerId, clientId, price).Returns<Task<decimal>>(x => { throw new ArgumentException($"Existing provider with id {clientId} not found"); });

            //Act
            var result = await _controller.GetFullPrice(providerId, clientId, price);
            var notFoundObjectResult = result as NotFoundObjectResult;

            //Assert
            Assert.NotNull(notFoundObjectResult);
            Assert.Equal($"Existing provider with id {clientId} not found", notFoundObjectResult.Value);
        }

        [Fact]
        public async Task GetFullPrice_GenericExceptionThrow_ReturnsBadRequestWithErrorMessage()
        {
            //Arrange
            int providerId = 1;
            int clientId = 2;
            decimal price = 10;
            _vatService.GetVATFullPrice(providerId, clientId, price).Returns<Task<decimal>>(x => { throw new Exception("Exception message"); });

            //Act
            var result = await _controller.GetFullPrice(providerId, clientId, price);
            var badRequestObjectResult = result as BadRequestObjectResult;

            //Assert
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal("Error occured: Exception message", badRequestObjectResult.Value);
        }
    }
}
