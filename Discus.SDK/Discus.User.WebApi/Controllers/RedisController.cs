using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discus.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IRedisClient _redisClient;

        private readonly IDistributedLocker _distributedLocker;

        private readonly static string _distributedLockerkey = "lock03";

      public RedisController(IRedisClient redisClient, IDistributedLocker distributedLocker)
        {
            _redisClient = redisClient;
            _distributedLocker= distributedLocker;
        }

        /// <summary>
        /// StringSetAsync
        /// </summary>
        /// <returns></returns>
        [Route("StringSetAsync"), HttpGet]
        public async Task<bool> StringSetAsync()
        {
            return await _redisClient.StringSetAsync("xusc", "159272409542");
        }

        /// <summary>
        /// StringGetAsync
        /// </summary>
        /// <returns></returns>
        [Route("StringGetAsync"), HttpGet]
        public async Task<string> StringGetAsync()
        {
            return await _redisClient.StringGetAsync("xusc");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("locker"), HttpGet]
        public async Task<bool> DistributedLockerTest()
        {
            var res = await _distributedLocker.LockAsync(_distributedLockerkey, 5,true);
            try
            {
                if (res.Success == true)
                {
                    await Console.Out.WriteLineAsync("获取锁成功");
                    Thread.Sleep(5000);
                }
                else 
                {
                    await Console.Out.WriteLineAsync("获取锁失败");
                }
            }
            catch (Exception e)
            {

            }
            finally 
            {
                _distributedLocker.SafedUnLock(_distributedLockerkey, res.LockValue);
            }
            return res.Success;
        }
    }
}
