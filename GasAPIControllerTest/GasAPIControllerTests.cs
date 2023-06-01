using AutoFixture;
using GasStations_GasAPI.Controllers;
using GasStations_GasAPI.Models;
using GasStations_GasAPI.Services.GasStationService;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GasAPIControllerTest
{
    [TestClass]

    public class GasAPIControllerTests
    {
       
        private Mock<IGasStationService> _gasStationServiceMock;
        private GasAPIController _controller;
        private Fixture _fixture;
        public GasAPIControllerTests()
        {
            _gasStationServiceMock = new Mock<IGasStationService>();
            _controller = new GasAPIController(_gasStationServiceMock.Object);
            _fixture = new Fixture();
        }

        /// <summary>
        /// GetGasStations() Unit Tests
        /// </summary>

        [TestMethod]
        public void GetGasStations_Returns_OK()
        {
            var gasStationList = _fixture.CreateMany<Gas>(3).ToList();
            _gasStationServiceMock.Setup(u => u.GetGasStations()).Returns(gasStationList);

            var actionResult = _controller.GetGasStations();
            var result = actionResult as ObjectResult;

            Assert.AreEqual(200, result.StatusCode);
        }

        /// <summary>
        /// GetGasStation() Unit Tests
        /// </summary>

        [TestMethod]
        public void GetGasStation_Returns_OK()
        {
            var gasStationList = _fixture.CreateMany<Gas>(3).ToList();
            _gasStationServiceMock.Setup(u => u.GetGasStation(2)).Returns(gasStationList[2]);

            var actionResult = _controller.GetGasStation(2);
            var result = actionResult as ObjectResult;

            Assert.AreEqual(200, result.StatusCode);
        }
        [TestMethod]
        public void GetGasStation_Returns_NotFound()
        {
            var gasStationList = _fixture.CreateMany<Gas>(3).ToList();
            _gasStationServiceMock.Setup(u => u.GetGasStation(0));

            var actionResult = _controller.GetGasStation(0);
            var result = actionResult as NotFoundResult;

            Assert.AreEqual(404, result.StatusCode);
        }

        //[TestMethod]
        //public void GetGasStation_Returns_BadRequest()
        //{
        //    var gas = _fixture.Build<Gas>().With(u => u.Id, null).Create();
        //    _gasStationServiceMock.Setup(u => u.GetGasStation(gas.Id));

        //    var actionResult = _controller.GetGasStation(gas.Id);
        //    var result = actionResult as BadRequestResult;

        //    Assert.AreEqual(400, result.StatusCode);
        //}

        /// <summary>
        /// CreateGasStation() Unit Tests
        /// </summary>

        [TestMethod]
        public void CreateGasStation_Returns_CreatedAtRoute()
        {
            var gas = _fixture.Build<Gas>().Without(u => u.Id).Create();
            _gasStationServiceMock.Setup(u => u.CreateGasStation(gas)).Returns(gas);

            var actionResult = _controller.CreateGasStation(gas);
            var result = actionResult as CreatedAtRouteResult;

            Assert.AreEqual(201, result.StatusCode);
        }
        //[TestMethod]
        //public void CreateGasStation_Returns_BadRequest()
        //{
        //    var gas = _fixture.Build<Gas>().Without(u => u.Id == null).Create();
        //    _gasStationServiceMock.Setup(u => u.CreateGasStation(gas)).Returns(gas);

        //    var actionResult = _controller.CreateGasStation(gas);
        //    var result = actionResult as ObjectResult;

        //    Assert.AreEqual(400, result.StatusCode);
        //}
        [TestMethod]
        public void CreateGasStation_Returns_InternalServerError()
        {
            var gas = _fixture.Create<Gas>();
            _gasStationServiceMock.Setup(u => u.CreateGasStation(gas)).Returns(gas);

            var actionResult = _controller.CreateGasStation(gas);
            var result = actionResult as StatusCodeResult;

            Assert.AreEqual(500, result.StatusCode);
        }

        /// <summary>
        /// DeleteGasStation() Unit Tests
        /// </summary>

        [TestMethod]
        public void DeleteGasStation_Returns_NoContent()
        {
            var gas = _fixture.Create<Gas>();
            _gasStationServiceMock.Setup(u => u.DeleteGasStation(gas.Id)).Returns(true);

            var actionResult = _controller.DeleteGasStation(gas.Id);
            var result = actionResult as NoContentResult;

            Assert.AreEqual(204, result.StatusCode);
        }
        [TestMethod]
        public void DeleteGasStation_Returns_BadRequest()
        {
            _gasStationServiceMock.Setup(u => u.DeleteGasStation(0)).Returns(false);

            var actionResult = _controller.DeleteGasStation(0);
            var result = actionResult as BadRequestResult;

            Assert.AreEqual(400, result.StatusCode);
        }
        //[TestMethod]
        //public void DeleteGasStation_Returns_NotFound()
        //{
        //    _gasStationServiceMock.Setup(u => u.DeleteGasStation(It.IsAny<int>())).Returns(false);

        //    var actionResult = _controller.DeleteGasStation(It.IsAny<int>());
        //    var result = actionResult as NotFoundResult;

        //    Assert.AreEqual(404, result.StatusCode);
        //}



        /// <summary>
        /// UpdateGasStation() Unit Tests
        /// </summary>

        [TestMethod]
        public void UpdateGasStation_Returns_NoContent()
        {
            var gas = _fixture.Create<Gas>();
            _gasStationServiceMock.Setup(u => u.UpdateGasStation(gas.Id, gas));

            var actionResult = _controller.UpdateGasStation(gas.Id, gas);
            var result = actionResult as NoContentResult;

            Assert.AreEqual(204, result.StatusCode);
        }
        [TestMethod]
        public void UpdateGasStation_Returns_BadRequest()
        {
            int id = 5;
            var gas = _fixture.Create<Gas>();
            _gasStationServiceMock.Setup(u => u.UpdateGasStation(id, gas));

            var actionResult = _controller.UpdateGasStation(id, gas);
            var result = actionResult as BadRequestResult;

            Assert.AreEqual(400, result.StatusCode);
        }


        //------------------------------------------------------------------------------------------------------------------------------------------------------------\\

        //[TestMethod]
        //public async Task GetGasStations_Returns_ListOfGasStations()
        //{
        //    IEnumerable<Gas> gasStations = new[]
        //    {
        //    new Gas
        //    {
        //        Id = 1,
        //        Name = "Shell",
        //        Address = "600 Dundas St",
        //        Number_of_Pumps = 8,
        //        Price = 147.3,
        //        Purity = 87,
        //    },
        //    new Gas
        //    {
        //        Id = 2,
        //        Name = "Petro Canada",
        //        Address = "1525 Burnhamthorpe Rd",
        //        Number_of_Pumps = 12,
        //        Price = 146.8,
        //        Purity = 87,
        //    }
        //    };
        //var _controller = new GasAPIController();
        //var result = await _controller.GetGasStations();
        //var obj = gasStations as ObjectResult;
        //result.Should().BeOfType<ActionResult>();
        //result.Should().BeEquivalentTo(gasStations);
        //Assert.AreEqual(gasStations, result);

    }
        //    private Mock<IGasStationService> _gasStationServiceMock;
        //    private GasAPIController _controller;
        //    public GasAPIControllerTests()
        //    {
        //        _gasStationServiceMock = new Mock<IGasStationService>();
        //        _controller = new GasAPIController(_gasStationServiceMock.Object);
        //    }

        //    [TestMethod]
        //    public async Task GetGasStations_Returns_ListOfGasStations1()
        //    {
        //        var gasStationList = A.CollectionOfDummy<Gas>(5).AsEnumerable();
        //        _gasStationServiceMock.Setup(u => u.GetGasStations()).Returns((Task<ActionResult<IEnumerable<Gas>>>)gasStationList);

        //        var actionResult = await _controller.GetGasStations();
        //        var result = actionResult.Result as OkObjectResult;
        //        var returnGasStations = result.Value as IEnumerable<Gas>;

        //        Assert.AreEqual(gasStationList, returnGasStations);

        //    }

        //    [TestMethod]
        //    public async Task GetGasStations_Returns_ListOfGasStations2()
        //    {
        //        //Arrange
        //        int count = 5;
        //        var fakeGasStations = A.CollectionOfDummy<Gas>(count).AsEnumerable();
        //        var gasStationService = A.Fake<IGasStationService>();
        //        A.CallTo(() => gasStationService.GetGasStations()).Returns(Task.FromResult(fakeGasStations));
        //        var controller = new GasAPIController(gasStationService);
        //        //Act
        //        var actionResult = await controller.GetGasStations();

        //        //Assert
        //        var result = actionResult.Result as OkObjectResult;
        //        var returnGasStations = result.Value as IEnumerable<Gas>;
        //        Assert.AreEqual(5, count);
        //    }

        //    [TestMethod]
        //    public async Task GetGasStations_Returns_GasStationWithID()
        //    {
        //        int id = 1;
        //        var gasStationList = A.CollectionOfDummy<Gas>(5).AsEnumerable();
        //        _gasStationServiceMock.Setup(u => u.GetGasStation(id)).Returns((Task<ActionResult<Gas>>)await Task.FromResult(gasStationList));

        //        var actionResult = await _controller.GetGasStation(id);
        //        var result = actionResult.Result as OkObjectResult;
        //        var returnGasStation = result.Value as Gas;
        //        Assert.AreEqual(gasStationList.ElementAt(1), returnGasStation);
        //    }
}