using GasStations_GasAPI.Services.GasStationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStations_GasAPITests
{
    [TestClass]
    public class GasAPIControllerTests
    {
        [TestMethod]
        public void Test()
        {
            var gasStationService = new Mock<IGasStationService>();
        }
    }
}
