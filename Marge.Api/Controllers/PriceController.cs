using System;
using System.Collections.Generic;
using Marge.Core.Commands;
using Marge.Core.Queries;
using Marge.Core.Queries.Models;
using Marge.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Marge.Api.Controllers
{
    [Route("api/[controller]")]
    public class PriceController : Controller
    {
        private readonly CommandBus commandBus;
        private readonly IPriceQuery priceQuery;

        public PriceController(CommandBus commandBus, IPriceQuery priceQuery)
        {
            this.commandBus = commandBus;
            this.priceQuery = priceQuery;
        }

        [HttpGet]
        public IEnumerable<Price> Get()
        {
            return priceQuery.RetrieveAll();
        }

        [HttpGet("{id}")]
        public Price Get(Guid id)
        {
            return priceQuery.RetrieveById(id);
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
