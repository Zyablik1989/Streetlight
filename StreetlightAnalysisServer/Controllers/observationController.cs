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
        public ObservationDto add(ObservationRequest request)
        {
            return new ObservationDto()
            {
                 status = Status.Ok,
                 response = new ObservationResponse()
                 {
                      start = new int[] {1,3},
                       missing = new string[] {"0011100","1100010"}
                 }
            };
        }
    }
}
