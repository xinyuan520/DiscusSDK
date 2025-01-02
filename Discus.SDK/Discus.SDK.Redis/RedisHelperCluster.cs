using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discus.SDK.Redis
{
    public class RedisHelperCluster
    {
        private static CSRedisClient masterClient;
        private static CSRedisClient[] slaveClients;
        private static int currentSlaveIndex = 0;
        private static readonly object lockObj = new object();

        public static void Initialize(string masterConnectionString, string[] slaveConnectionStrings)
        {
            // 初始化主节点
            masterClient = new CSRedisClient(masterConnectionString);

            // 初始化从节点
            slaveClients = new CSRedisClient[slaveConnectionStrings.Length];
            for (int i = 0; i < slaveConnectionStrings.Length; i++)
            {
                slaveClients[i] = new CSRedisClient(slaveConnectionStrings[i]);
            }
        }

        public static void Initialize(string masterConnectionString, string slaveConnectionString)
        {
            // 初始化主节点
            masterClient = new CSRedisClient(masterConnectionString);
            slaveClients = new CSRedisClient[1];
            slaveClients[0] = new CSRedisClient(slaveConnectionString);
        }

        // 获取用于写操作的主节点客户端
        public static CSRedisClient Master => masterClient;

        // 使用轮询方式获取用于读操作的从节点客户端
        public static CSRedisClient Slave
        {
            get
            {
                if (slaveClients == null || slaveClients.Length == 0)
                    return masterClient;

                lock (lockObj)
                {
                    currentSlaveIndex = (currentSlaveIndex + 1) % slaveClients.Length;
                    return slaveClients[currentSlaveIndex];
                }
            }
        }
    }
}
