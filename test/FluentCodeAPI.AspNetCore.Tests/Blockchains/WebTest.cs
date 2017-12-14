using FluentCodeAPI.AspNetCore.Blockchains.Internal;
using System.Threading.Tasks;
using Xunit;

namespace FluentCodeAPI.AspNetCore.Blockchains.Tests
{
    public class WebTest
    {
        [Fact(DisplayName = "Can get http response via Web")]
        public async Task CanGetHttpResponse()
        {
            var actual = await Web.GetAsync("http://icanhazip.com/");

            Assert.NotNull(actual);
        }
    }
}