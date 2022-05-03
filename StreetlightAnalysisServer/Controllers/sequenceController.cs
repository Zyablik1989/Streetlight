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

                    sequence = System.Guid.NewGuid().ToString(),
                }

            };
        }
    }
}
