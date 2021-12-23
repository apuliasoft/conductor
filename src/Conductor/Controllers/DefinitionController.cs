using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conductor.Auth;
using Conductor.Domain.Interfaces;
using Conductor.Domain.Models;
using Conductor.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MassTransit;

namespace Conductor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefinitionController : ControllerBase
    {
        private readonly IDefinitionService _service;
        readonly IPublishEndpoint _publishEndpoint;

        public DefinitionController(IDefinitionService service, IPublishEndpoint publishEndpoint)
        {
            _service = service;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        [Authorize(Policy = Policies.Author)]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Policies.Author)]
        public ActionResult<Definition> Get(string id)
        {
            var result = _service.GetDefinition(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = Policies.Author)]
        public ActionResult Post([FromBody] Definition value)
        {
            Task.Run(async () =>
            {
                await _publishEndpoint.Publish<CreateDefinition>(new
                {
                    Definition = value
                });
            });
            return NoContent();
        }

        //[HttpPut]
        //public void Put([FromBody] string value)
        //{
        //    _service.RegisterNewDefinition(value);
        //}

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.Author)]
        public void Delete(int id)
        {
        }
    }
}