using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Marge.Api;
using Marge.Core.Commands;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NFluent;
using Xunit;

namespace Marge.Tests.Integration
{
    public class AddingNewPrice
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public AddingNewPrice()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>();

            _server = new TestServer(builder);

            _client = _server.CreateClient();
        }

        [Fact]
        public async Task ReturnHelloWorld()
        {
            var response = await _client.GetAsync("/api/price");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Check.That(responseString).Equals("[]");
        }

        [Fact]
        public async Task WhenPutRequestSendThenGetAllShouldReturnPutElement()
        {
            var seralizedCommand = JsonConvert.SerializeObject(new CreatePriceCommand(69, 42));
            await _client.PostAsync("/api/price", new StringContent(seralizedCommand, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            var response = await _client.GetAsync("/api/price");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            Check.That(responseString).Contains("[{\"id\":\"")
                .And.Contains("\",\"amount\":69.0,\"discount\":0.0,\"profit\":0.3913043478260869565217391304}]");
        }
    }
}