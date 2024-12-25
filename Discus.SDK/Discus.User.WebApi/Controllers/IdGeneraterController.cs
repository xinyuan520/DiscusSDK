using Discus.SDK.IdGenerater.IdGeneraterFactory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discus.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdGeneraterController : ControllerBase
    {
        /// <summary>
        /// 测试Id生成器
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetIdGenerater")]
        public long GetIdGenerater()
        {
            return IdGenerater.GetNextId();
        }
    }
}
