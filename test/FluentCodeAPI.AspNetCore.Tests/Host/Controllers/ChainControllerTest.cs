using FluentCodeAPI.AspNetCore.Blockchains.Host.Controllers;
using FluentCodeAPI.AspNetCore.Blockchains.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FluentCodeAPI.AspNetCore.Blockchains.Tests.Host.Controllers
{
    public class ChainControllerTest
    {
        [Fact(DisplayName = "Can get chain")]
        public void Test()
        {
            var application = new FakeApplicationContext();

            var blockchain = application.GetService<Blockchain>();
            var controller = new ChainController(blockchain);

            var response = controller.Index();

            var actual = ((List<Block>)response.Value).First().PreviousHash;

            Assert.NotNull(actual);
            Assert.Equal("7F83B1657FF1FC53B92DC18148A1D65DFC2D4B1FA3D677284ADDD200126D9069", actual);
        }
    }
}