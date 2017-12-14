using FluentCodeAPI.AspNetCore.Blockchains.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FluentCodeAPI.AspNetCore.Blockchains.Tests
{
    public class BlockchainTest
    {
        [Fact(DisplayName = "Can hash block")]
        public void CanHashBlock()
        {
            var application = new FakeApplicationContext();

            var blockchain = application.GetService<Blockchain>();

            var block = new Block()
            {
                Index = 1,
                Timestamp = 10,
                Transactions = new List<Transaction>(),
                Proof = 100,
                PreviousHash = "7F83B1657FF1FC53B92DC18148A1D65DFC2D4B1FA3D677284ADDD200126D9069"
            };

            var actual = blockchain.HashBlock(block);

            Assert.NotNull(actual);
            Assert.Equal("4B3E01D751A9B74F46FB85757177FD6E17C8E7201A0F1193E1EF3B57FCE8264B", actual);
        }

        [Fact(DisplayName = "Can compute proof of work")]
        public void CanComputeProofOfWork()
        {
            var application = new FakeApplicationContext();

            var blockchain = application.GetService<Blockchain>();

            var actual = blockchain.ComputeProofOfWork();

            Assert.Equal(33575, actual);
        }

        [Fact(DisplayName = "Can validate proof")]
        public void CanValidateProof()
        {
            var application = new FakeApplicationContext();

            var blockchain = application.GetService<Blockchain>();

            var actual = blockchain.IsValidProof(100, 33575);

            Assert.True(actual);
        }

        [Fact(DisplayName = "Can register node")]
        public void CanRegisterNode()
        {
            var application = new FakeApplicationContext();

            var blockchain = application.GetService<Blockchain>();

            Assert.Empty(blockchain.Nodes);

            var actual = blockchain.RegisterNode("http://localhost:8080/");

            Assert.True(actual);
            Assert.NotEmpty(blockchain.Nodes);
        }

        [Fact(DisplayName = "Can validate chain")]
        public void CanValidateChain()
        {
            var application = new FakeApplicationContext();

            var blockchain = application.GetService<Blockchain>();

            var actual = blockchain.IsValidChain(blockchain.Chain);

            Assert.True(actual);
        }

        [Fact(DisplayName = "Can resolve conflicts on empty chains")]
        public async Task CanResolveConflictsOnEmptyChains()
        {
            var application = new FakeApplicationContext();

            var blockchain = application.GetService<Blockchain>();

            var actual = await blockchain.ResolveConflictsAsync();

            Assert.False(actual);
        }
    }
}