using FluentCodeAPI.AspNetCore.Blockchains.Host;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace FluentCodeAPI.AspNetCore.Blockchains.Tests
{
    public class FakeApplicationContext
    {
        private readonly TestServer _server;

        public FakeApplicationContext(string[] args = null)
        {
            var webHostBuilder = Program.CreateWebHostBuilder(args);

            _server = new TestServer(webHostBuilder);
        }

        public HttpClient Client => _server.CreateClient();

        public T GetService<T>() => _server.Host.Services.GetService<T>();
    }
}