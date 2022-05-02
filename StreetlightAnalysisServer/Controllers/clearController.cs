using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StreetlightExchange;

namespace StreetlightAnalysisServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class clearController : ControllerBase
    {
        [HttpGet]
        public ClearDto clear()
        {
            return new ClearDto()
            {
                status = Status.Ok,
                response = Status.Ok
            };
        }
    }
}
