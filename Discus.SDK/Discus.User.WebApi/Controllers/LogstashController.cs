using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discus.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogstashController : ControllerBase
    {
        private readonly ILogger<LogstashController> _logger;


        public LogstashController(ILogger<LogstashController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="info"></param>
        [HttpGet("AddLog/{info}")]
        public void AddLog(string info)
        {
            _logger.LogInformation(info);
        }
    }
}
