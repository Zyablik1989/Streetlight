using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StreetlightAnalysisServer.Controllers
{
    [Route("[controller]/create")]
    [ApiController]
    public class sequence : ControllerBase
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
