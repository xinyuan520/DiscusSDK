using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobWebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowController : ControllerBase
    {
        [HttpGet("show")]
        public string Show()
        {
            return "show info ";
        }
    }
}
