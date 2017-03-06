using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Marge.Api;
using Marge.Core.Commands;
using Marge.Core.Queries.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NFluent;
using Xunit;
using System.Data.SqlClient;
using Dapper;

namespace Marge.Tests.Integration
{
    public class AddingNewPrice : IDisposable
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
        public async Task WhenGetTheFirstTimeThenReturnEmptyArray()
        {
            var response = await _client.GetAsync("/api/price");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Check.That(responseString).Equals("[]");
        }

        [Fact]
        public async Task GivenAnExistingPriceWhenDeleteItThenDoNotRetrieveIt()
        {
            var input = new List<CreatePriceCommand>
            {
                new CreatePriceCommand(1000m, 800m, Guid.NewGuid()),
                new CreatePriceCommand(10000m, 7000m, Guid.NewGuid()),
                new CreatePriceCommand(10m, 5m, Guid.NewGuid()),
                new CreatePriceCommand(100m, 10m, Guid.NewGuid())
            };
            var expected = new List<Price>
            {
                new Price(input[0].CommandId, input[0].TargetPrice, 0,0.2m),
                new Price(input[2].CommandId, input[2].TargetPrice, 0,0.5m),
                new Price(input[3].CommandId, input[3].TargetPrice, 0,0.9m)
            };

            foreach (var createPriceCommand in input)
            {
                var seralizedCommand = JsonConvert.SerializeObject(createPriceCommand);
                await _client.PostAsync("/api/price", new StringContent(seralizedCommand, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            }
            await _client.DeleteAsync("/api/price/" + input[1].CommandId);
            var response = await _client.GetAsync("/api/price");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var prices = JsonConvert.DeserializeObject<List<Price>>(responseString);

            Check.That(prices).Contains(expected);
        }

        public void Dispose()
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["MargeDb"].ConnectionString))
            {
                db.Execute("delete Prices");
            }
        }
    }
}