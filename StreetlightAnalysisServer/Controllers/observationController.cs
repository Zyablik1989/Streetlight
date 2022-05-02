using Microsoft.AspNetCore.Mvc;
using StreetlightExchange;

namespace StreetlightAnalysisServer.Controllers
{
    [Route("[controller]/add")]
    [ApiController]
    public class observationController : ControllerBase
    {
        // GET: <observationController>/add
        [HttpPost]
        public ObservationResponse add(ObservationRequest request)
        {
            return new ObservationResponse()
            {

            };
        }

        //// GET api/<observationController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<observationController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<observationController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<observationController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
