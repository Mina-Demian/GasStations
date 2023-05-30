using FakeItEasy;
using GasStations_GasAPI.Controllers;
using GasStations_GasAPI.Models;
using GasStations_GasAPI.Services.GasStationService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GasAPIControllerTest
{
    [TestClass]
    public class GasAPIControllerTests
    {
        private Mock<IGasStationService> _gasStationServiceMock;
        private GasAPIController _controller;
        public GasAPIControllerTests()
        {
            _gasStationServiceMock = new Mock<IGasStationService>();
            _controller = new GasAPIController(_gasStationServiceMock.Object);
        }

        [TestMethod]
        public async Task GetGasStations_Returns_ListOfGasStations1()
        {
            var gasStationList = A.CollectionOfDummy<Gas>(5).AsEnumerable();
            _gasStationServiceMock.Setup(u => u.GetGasStations()).Returns((Task<ActionResult<IEnumerable<Gas>>>)gasStationList);

            var actionResult = await _controller.GetGasStations();
            var result = actionResult.Result as OkObjectResult;
            var returnGasStations = result.Value as IEnumerable<Gas>;

            Assert.AreEqual(gasStationList, returnGasStations);

        }

        //[TestMethod]
        //public async Task GetGasStations_Returns_ListOfGasStations2()
        //{
        //    //Arrange
        //    int count = 5;
        //    var fakeGasStations = A.CollectionOfDummy<Gas>(count).AsEnumerable();
        //    var gasStationService = A.Fake<IGasStationService>();
        //    A.CallTo(() => gasStationService.GetGasStations()).Returns(Task.FromResult(fakeGasStations));
        //    var controller = new GasAPIController(gasStationService);
        //    //Act
        //    var actionResult = await controller.GetGasStations();

        //    //Assert
        //    var result = actionResult.Result as OkObjectResult;
        //    var returnGasStations = result.Value as IEnumerable<Gas>;
        //    Assert.AreEqual(5, count);
        //}

        [TestMethod]
        public async Task GetGasStations_Returns_GasStationWithID()
        {
            int id = 1;
            var gasStationList = A.CollectionOfDummy<Gas>(5).AsEnumerable();
            _gasStationServiceMock.Setup(u => u.GetGasStation(id)).Returns((Task<ActionResult<Gas>>)await Task.FromResult(gasStationList));

            var actionResult = await _controller.GetGasStation(id);
            var result = actionResult.Result as OkObjectResult;
            var returnGasStation = result.Value as Gas;
            Assert.AreEqual(gasStationList.ElementAt(1), returnGasStation);
        }
    }
}