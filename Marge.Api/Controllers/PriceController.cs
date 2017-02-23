using System;
using System.Collections.Generic;
using Marge.Core.Commands;
using Marge.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Marge.Api.Controllers
{
    [Route("api/[controller]")]
    public class PriceController : Controller
    {
        private readonly CommandBus commandBus;

        public PriceController(CommandBus commandBus)
        {
            this.commandBus = commandBus;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody]CreatePriceCommand command)
        {
            commandBus.Publish(command);
        }

        [HttpPut("{priceId}")]
        public void Put(Guid priceId, [FromBody]ChangeDiscountCommand command)
        {
            commandBus.Publish(new ChangeDiscountCommand(priceId, command.Discount));
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
