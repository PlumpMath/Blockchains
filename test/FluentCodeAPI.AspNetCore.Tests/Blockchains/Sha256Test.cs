using FluentCodeAPI.AspNetCore.Blockchains.Internal;
using Xunit;

namespace FluentCodeAPI.AspNetCore.Blockchains.Tests
{
    public class Sha256Test
    {
        [Fact(DisplayName = "Can compute sha 256")]
        public void CanComputeSHA256()
        {
            var actual = Hasher.ComputeSHA256("Hello World!");

            Assert.NotNull(actual);
            Assert.Equal("7F83B1657FF1FC53B92DC18148A1D65DFC2D4B1FA3D677284ADDD200126D9069", actual);
        }
    }
}