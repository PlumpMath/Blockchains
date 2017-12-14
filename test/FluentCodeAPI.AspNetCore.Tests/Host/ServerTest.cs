using FluentCodeAPI.AspNetCore.Blockchains.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FluentCodeAPI.AspNetCore.Blockchains.Tests
{
    public class ServerTest
    {
        [Fact(DisplayName = "Can get http response via TestServer HttpClient")]
        public async Task CanGetHttpResponse()
        {
            var application = new FakeApplicationContext();

            var response = await application.Client.GetAsync("/");
            var value = await response.Content.ReadAsStringAsync();

            var actual = JsonConvert.DeserializeObject<List<Block>>(value);

            Assert.NotNull(actual);
            Assert.Equal("7F83B1657FF1FC53B92DC18148A1D65DFC2D4B1FA3D677284ADDD200126D9069", actual.First().PreviousHash);
        }
    }
}