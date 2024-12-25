using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discus.DistributeCalculate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowController : ControllerBase
    {
        /// <summary>
        /// show
        /// </summary>
        /// <returns></returns>
        [HttpGet("show")]
        public string Show()
        {
            return "Show()";
        }
    }
}
