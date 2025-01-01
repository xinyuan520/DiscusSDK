using Discus.SDK.Redis.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Discus.SDK.Redis
{
    public class RedisClient : IRedisClient
    {
        private readonly IOptions<RedisConfiguration> _options;
        private readonly ILogger<RedisClient> _logger;

        public RedisClient(IOptions<RedisConfiguration> options, ILogger<RedisClient> logger)
        {
            _options = options;
            _logger = logger;
            InitializeRedis(_options);
        }

        private static string InitializeRedis(IOptions<RedisConfiguration> options)
        {
            if (!string.IsNullOrEmpty(options.Value.SlaveConnectionString))
            {
                RedisHelperCluster.Initialize(options.Value.MasterConnectionString, options.Value.SlaveConnectionString);
                return options.Value.SlaveConnectionString;
            }
            if (options.Value.SlaveConnectionStrings != null && options.Value.SlaveConnectionStrings.Length > 0)
            {
                RedisHelperCluster.Initialize(options.Value.MasterConnectionString, options.Value.SlaveConnectionStrings);
                return options.Value.SlaveConnectionStrings[0];
            }
            throw new InvalidOperationException("redis 配置异常");
        }

        #region Keys
        public long KeyDel(string cacheKey)
        {
            return RedisHelperCluster.Master.Del(cacheKey);
        }

        public async Task<long> KeyDelAsync(string cacheKey)
        {
            return await RedisHelperCluster.Master.DelAsync(cacheKey);
        }

        public bool KeyExists(string cacheKey)
        {
            return RedisHelperCluster.Slave.Exists(cacheKey);
        }

        public async Task<bool> KeyExistsAsync(string cacheKey)
        {
            return await RedisHelperCluster.Slave.ExistsAsync(cacheKey);
        }

        public bool KeyExpire(string cacheKey, int second)
        {
            return RedisHelperCluster.Slave.Expire(cacheKey, second);
        }

        public async Task<bool> KeyExpireAsync(string cacheKey, int second)
        {
            return await RedisHelperCluster.Slave.ExpireAsync(cacheKey, second);
        }

        public long TTL(string cacheKey)
        {
            return RedisHelperCluster.Slave.Ttl(cacheKey);
        }

        public async Task<long> TTLAsync(string cacheKey)
        {
            return await RedisHelperCluster.Slave.TtlAsync(cacheKey);
        }
        #endregion

        #region String
        public long IncrBy(string cacheKey, long value = 1)
        {
            return RedisHelperCluster.Master.IncrBy(cacheKey, value);
        }

        public async Task<long> IncrByAsync(string cacheKey, long value = 1)
        {
            return await RedisHelperCluster.Master.IncrByAsync(cacheKey, value);
        }

        public decimal IncrByFloat(string cacheKey, decimal value = 1)
        {
            return RedisHelperCluster.Master.IncrByFloat(cacheKey, value);
        }

        public async Task<decimal> IncrByFloatAsync(string cacheKey, decimal value = 1)
        {
            return await RedisHelperCluster.Master.IncrByFloatAsync(cacheKey, value);
        }

        public bool StringSet(string cacheKey, object cacheValue, int expireSeconds = -1, RedisExistence? exists = null)
        {
            return RedisHelperCluster.Master.Set(cacheKey, cacheValue, expireSeconds, exists);
        }

        public async Task<bool> StringSetAsync(string cacheKey, object cacheValue, int expireSeconds = -1, RedisExistence? exists = null)
        {
            return await RedisHelperCluster.Master.SetAsync(cacheKey, cacheValue, expireSeconds, exists);
        }

        public string StringGet(string cacheKey)
        {
            return RedisHelperCluster.Slave.Get(cacheKey);

        }

        public async Task<string> StringGetAsync(string cacheKey)
        {
            return await RedisHelperCluster.Slave.GetAsync(cacheKey);
        }

        public long StringLen(string cacheKey)
        {
            return RedisHelperCluster.Slave.StrLen(cacheKey);
        }

        public async Task<long> StringLenAsync(string cacheKey)
        {
            return await RedisHelperCluster.Slave.StrLenAsync(cacheKey);
        }

        public long StringSetRange(string cacheKey, uint offest, string value)
        {
            return RedisHelperCluster.Master.SetRange(cacheKey, offest, value);
        }

        public async Task<long> StringSetRangeAsync(string cacheKey, uint offest, string value)
        {
            return await RedisHelperCluster.Master.SetRangeAsync(cacheKey, offest, value);
        }

        public string StringGetRange(string cacheKey, long start, long end)
        {
            return RedisHelperCluster.Slave.GetRange(cacheKey, start, end);
        }

        public async Task<string> StringGetRangeAsync(string cacheKey, long start, long end)
        {
            return await RedisHelperCluster.Slave.GetRangeAsync(cacheKey, start, end);
        }
        #endregion

        #region Hashes
        public bool HMSet(string cacheKey, Dictionary<string, string> vals, TimeSpan? expiration = null)
        {
            return RedisHelperCluster.Master.HMSet(cacheKey, vals, expiration);
        }

        public bool HSet(string cacheKey, string field, string cacheValue)
        {
            return RedisHelperCluster.Master.HSet(cacheKey, field, cacheValue);
        }

        public bool HExists(string cacheKey, string field)
        {
            return RedisHelperCluster.Master.HExists(cacheKey, field);
        }

        public long HDel(string cacheKey, string[] fields = null)
        {
            return RedisHelperCluster.Master.HDel(cacheKey, fields);

        }

        public string HGet(string cacheKey, string field)
        {
            return RedisHelperCluster.Master.HGet(cacheKey, field);
        }

        public Dictionary<string, string> HGetAll(string cacheKey)
        {
            return RedisHelperCluster.Master.HGetAll(cacheKey);
        }

        public long HIncrBy(string cacheKey, string field, long val = 1)
        {
            return RedisHelperCluster.Master.HIncrBy(cacheKey, field, val);
        }

        public string[] HKeys(string cacheKey)
        {
            return RedisHelperCluster.Master.HKeys(cacheKey);
        }

        public long HLen(string cacheKey)
        {
            return RedisHelperCluster.Master.HLen(cacheKey);
        }

        public string[] HVals(string cacheKey)
        {
            return RedisHelperCluster.Master.HVals(cacheKey);
        }

        public string[] HMGet(string cacheKey, string[] fields)
        {
            return RedisHelperCluster.Master.HMGet(cacheKey, fields);
        }

        public async Task<bool> HMSetAsync(string cacheKey, object[] keyValues)
        {
            return await RedisHelperCluster.Master.HMSetAsync(cacheKey, keyValues);
        }

        public async Task<bool> HSetAsync(string cacheKey, string field, string cacheValue)
        {
            return await RedisHelperCluster.Master.HSetAsync(cacheKey, field, cacheValue);
        }

        public Task<bool> HExistsAsync(string cacheKey, string field)
        {
            return RedisHelperCluster.Master.HExistsAsync(cacheKey, field);
        }

        public async Task<long> HDelAsync(string cacheKey, string[] fields)
        {
            return await RedisHelperCluster.Master.HDelAsync(cacheKey, fields);
        }

        public async Task<string> HGetAsync(string cacheKey, string field)
        {
            return await RedisHelperCluster.Master.HGetAsync(cacheKey, field);
        }

        public async Task<Dictionary<string, string>> HGetAllAsync(string cacheKey)
        {
            return await RedisHelperCluster.Master.HGetAllAsync(cacheKey);
        }

        public async Task<long> HIncrByAsync(string cacheKey, string field, long val = 1)
        {
            return await RedisHelperCluster.Master.HIncrByAsync(cacheKey, field, val);
        }

        public async Task<string[]> HKeysAsync(string cacheKey)
        {
            return await RedisHelperCluster.Master.HKeysAsync(cacheKey);
        }

        public async Task<long> HLenAsync(string cacheKey)
        {
            return await RedisHelperCluster.Master.HLenAsync(cacheKey);
        }

        public async Task<string[]> HValsAsync(string cacheKey)
        {
            return await RedisHelperCluster.Master.HValsAsync(cacheKey);
        }

        public async Task<string[]> HMGetAsync(string cacheKey, string[] fields)
        {
            return await RedisHelperCluster.Master.HMGetAsync(cacheKey, fields);
        }
        #endregion

        #region List
        public T LIndex<T>(string cacheKey, long index)
        {
            return RedisHelperCluster.Master.LIndex<T>(cacheKey, index);
        }

        public long LLen(string cacheKey)
        {
            return RedisHelperCluster.Master.LLen(cacheKey);
        }

        public T LPop<T>(string cacheKey)
        {
            return RedisHelperCluster.Master.LPop<T>(cacheKey);
        }

        public long LPush<T>(string cacheKey, T[] cacheValues)
        {
            return RedisHelperCluster.Master.LPush<T>(cacheKey, cacheValues);
        }

        public T[] LRange<T>(string cacheKey, long start, long stop)
        {
            return RedisHelperCluster.Master.LRange<T>(cacheKey, start, stop);
        }

        public long LRem(string cacheKey, long count, object cacheValue)
        {
            return RedisHelperCluster.Master.LRem(cacheKey, count, cacheValue);
        }

        public bool LSet(string cacheKey, long index, object cacheValue)
        {
            return RedisHelperCluster.Master.LSet(cacheKey, index, cacheValue);
        }

        public bool LTrim(string cacheKey, long start, long stop)
        {
            return RedisHelperCluster.Master.LTrim(cacheKey, start, stop);
        }

        public long LPushX(string cacheKey, object cacheValue)
        {
            return RedisHelperCluster.Master.LPushX(cacheKey, cacheValue);
        }

        public long LInsertBefore(string cacheKey, object pivot, object cacheValue)
        {
            return RedisHelperCluster.Master.LInsertBefore(cacheKey, pivot, cacheValue);
        }

        public long LInsertAfter(string cacheKey, object pivot, object cacheValue)
        {
            return RedisHelperCluster.Master.LInsertAfter(cacheKey, pivot, cacheValue);
        }

        public long RPushX(string cacheKey, object cacheValue)
        {
            return RedisHelperCluster.Master.RPushX(cacheKey, cacheValue);
        }

        public long RPush<T>(string cacheKey, T[] cacheValues)
        {
            return RedisHelperCluster.Master.RPush<T>(cacheKey, cacheValues);
        }

        public T RPop<T>(string cacheKey)
        {
            return RedisHelperCluster.Master.RPop<T>(cacheKey);
        }

        public async Task<T> LIndexAsync<T>(string cacheKey, long index)
        {
            return await RedisHelperCluster.Master.LIndexAsync<T>(cacheKey, index);
        }

        public async Task<long> LLenAsync(string cacheKey)
        {
            return await RedisHelperCluster.Master.LLenAsync(cacheKey);
        }

        public async Task<T> LPopAsync<T>(string cacheKey)
        {
            return await RedisHelperCluster.Master.LPopAsync<T>(cacheKey);
        }

        public async Task<long> LPushAsync<T>(string cacheKey, T[] cacheValues)
        {
            return await RedisHelperCluster.Master.LPushAsync<T>(cacheKey, cacheValues);
        }

        public async Task<T[]> LRangeAsync<T>(string cacheKey, long start, long stop)
        {
            return await RedisHelperCluster.Master.LRangeAsync<T>(cacheKey, start, stop);
        }

        public async Task<long> LRemAsync(string cacheKey, long count, object cacheValue)
        {
            return await RedisHelperCluster.Master.LRemAsync(cacheKey, count, cacheValue);
        }

        public async Task<bool> LSetAsync(string cacheKey, long index, object cacheValue)
        {
            return await RedisHelperCluster.Master.LSetAsync(cacheKey, index, cacheValue);
        }

        public async Task<bool> LTrimAsync(string cacheKey, long start, long stop)
        {
            return await RedisHelperCluster.Master.LTrimAsync(cacheKey, start, stop);
        }

        public async Task<long> LPushXAsync(string cacheKey, object cacheValue)
        {
            return await RedisHelperCluster.Master.LPushXAsync(cacheKey, cacheValue);
        }

        public async Task<long> LInsertBeforeAsync(string cacheKey, object pivot, object cacheValue)
        {
            return await RedisHelperCluster.Master.LInsertBeforeAsync(cacheKey, pivot, cacheValue);
        }

        public async Task<long> LInsertAfterAsync(string cacheKey, object pivot, object cacheValue)
        {
            return await RedisHelperCluster.Master.LInsertAfterAsync(cacheKey, pivot, cacheValue);
        }

        public async Task<long> RPushXAsync(string cacheKey, object cacheValue)
        {
            return await RedisHelperCluster.Master.RPushXAsync(cacheKey, cacheValue);
        }

        public async Task<long> RPushAsync<T>(string cacheKey, T[] cacheValues)
        {
            return await RedisHelperCluster.Master.RPushAsync<T>(cacheKey, cacheValues);
        }

        public async Task<T> RPopAsync<T>(string cacheKey)
        {
            return await RedisHelperCluster.Master.RPopAsync<T>(cacheKey);
        }
        #endregion

        #region Set

        public long SAdd<T>(string cacheKey, T[] cacheValues)
        {
            return RedisHelperCluster.Master.SAdd<T>(cacheKey, cacheValues);
        }

        public long SCard(string cacheKey)
        {
            return RedisHelperCluster.Master.SCard(cacheKey);
        }

        public bool SIsMember(string cacheKey, object cacheValue)
        {
            return RedisHelperCluster.Master.SIsMember(cacheKey, cacheValue);
        }

        public string[] SMembers(string cacheKey)
        {
            return RedisHelperCluster.Master.SMembers(cacheKey);
        }

        public T SPop<T>(string cacheKey)
        {
            return RedisHelperCluster.Master.SPop<T>(cacheKey);
        }

        public T SRandMember<T>(string cacheKey)
        {
            return RedisHelperCluster.Master.SRandMember<T>(cacheKey);
        }

        public long SRem<T>(string cacheKey, T[] cacheValues)
        {
            return RedisHelperCluster.Master.SRem<T>(cacheKey, cacheValues);
        }

        public async Task<long> SAddAsync<T>(string cacheKey, T[] cacheValues)
        {
            return await RedisHelperCluster.Master.SAddAsync<T>(cacheKey, cacheValues);
        }

        public async Task<long> SCardAsync(string cacheKey)
        {
            return await RedisHelperCluster.Master.SCardAsync(cacheKey);
        }

        public async Task<bool> SIsMemberAsync(string cacheKey, object cacheValue)
        {
            return await RedisHelperCluster.Master.SIsMemberAsync(cacheKey, cacheValue);
        }

        public async Task<string[]> SMembersAsync(string cacheKey)
        {
            return await RedisHelperCluster.Master.SMembersAsync(cacheKey);
        }

        public async Task<T> SPopAsync<T>(string cacheKey)
        {
            return await RedisHelperCluster.Master.SPopAsync<T>(cacheKey);
        }

        public async Task<T> SRandMemberAsync<T>(string cacheKey)
        {
            return await RedisHelperCluster.Master.SRandMemberAsync<T>(cacheKey);
        }

        public async Task<long> SRemAsync<T>(string cacheKey, T[] cacheValues)
        {
            return await RedisHelperCluster.Master.SRemAsync<T>(cacheKey, cacheValues);
        }
        #endregion

        #region Sorted Set
        public long ZAdd(string cacheKey, (decimal, object)[] cacheValues)
        {
            return RedisHelperCluster.Master.ZAdd(cacheKey, cacheValues);
        }

        public long ZCard(string cacheKey)
        {
            return RedisHelperCluster.Slave.ZCard(cacheKey);
        }

        public long ZCount(string cacheKey, decimal min, decimal max)
        {
            return RedisHelperCluster.Slave.ZCount(cacheKey, min, max);
        }

        public decimal ZIncrBy(string cacheKey, string field, decimal increment = 1m)
        {
            return RedisHelperCluster.Master.ZIncrBy(cacheKey, field, increment);
        }

        public long ZLexCount(string cacheKey, string min, string max)
        {
            return RedisHelperCluster.Slave.ZLexCount(cacheKey, min, max);
        }

        public T[] ZRange<T>(string cacheKey, long start, long stop)
        {
            return RedisHelperCluster.Slave.ZRange<T>(cacheKey, start, stop);
        }

        public long? ZRank(string cacheKey, object cacheValue)
        {
            return RedisHelperCluster.Master.ZRank(cacheKey, cacheValue);
        }

        public long ZRem<T>(string cacheKey, T[] cacheValues)
        {
            return RedisHelperCluster.Master.ZRem(cacheKey, cacheValues);
        }

        public decimal? ZScore(string cacheKey, object cacheValue)
        {
            return RedisHelperCluster.Master.ZScore(cacheKey, cacheValue);
        }

        public async Task<long> ZAddAsync(string cacheKey, params (decimal, object)[] cacheValues)
        {
            return await RedisHelperCluster.Master.ZAddAsync(cacheKey, cacheValues);
        }

        public async Task<long> ZCardAsync(string cacheKey)
        {
            return await RedisHelperCluster.Master.ZCardAsync(cacheKey);
        }

        public async Task<long> ZCountAsync(string cacheKey, decimal min, decimal max)
        {
            return await RedisHelperCluster.Master.ZCountAsync(cacheKey, min, max);
        }

        public async Task<decimal> ZIncrByAsync(string cacheKey, string field, decimal val = 1m)
        {
            return await RedisHelperCluster.Master.ZIncrByAsync(cacheKey, field, val);
        }

        public async Task<long> ZLexCountAsync(string cacheKey, string min, string max)
        {
            return await RedisHelperCluster.Master.ZLexCountAsync(cacheKey, min, max);
        }

        public async Task<T[]> ZRangeAsync<T>(string cacheKey, long start, long stop)
        {
            return await RedisHelperCluster.Master.ZRangeAsync<T>(cacheKey, start, stop);
        }

        public async Task<long?> ZRankAsync(string cacheKey, object cacheValue)
        {
            return await RedisHelperCluster.Master.ZRankAsync(cacheKey, cacheValue);
        }

        public async Task<long> ZRemAsync<T>(string cacheKey, T[] cacheValues)
        {
            return await RedisHelperCluster.Master.ZRemAsync<T>(cacheKey, cacheValues);
        }

        public async Task<decimal?> ZScoreAsync<T>(string cacheKey, object cacheValue)
        {
            return await RedisHelperCluster.Master.ZScoreAsync(cacheKey, cacheValue);

        }
        #endregion

        public async Task<dynamic> EvalAsync(string script, string keys, object[] args)
        {
            return await RedisHelperCluster.Master.EvalAsync(script, keys, args);
        }

    }
}
