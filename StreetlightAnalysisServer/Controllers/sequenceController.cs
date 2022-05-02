using Microsoft.AspNetCore.Mvc;
using StreetlightExchange;
using System.Collections.Generic;

namespace StreetlightAnalysisServer.Controllers
{
    [Route("[controller]/create")]
    [ApiController]
    public class sequenceController : ControllerBase
    {
        // GET: sequence/create
        [HttpPost]
        public SequenceDto create()
        {
            return new SequenceDto()
            {
                status = Status.Ok,
                response = new SequenceResponse()
                {
                    sequence = System.Guid.NewGuid()
                }

            };
        }

        // GET api/<StreetlightReport>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<StreetlightReport>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<StreetlightReport>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<StreetlightReport>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
