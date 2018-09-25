using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using StackExchange.Redis;
namespace ZJB.Core.Utilities
{
    public class RedisHelper
    {
        //        private static readonly string Server = "192.168.0.176";
        //        private static readonly string Port = "6379";
        //        private static readonly int Db = 1;
        private static readonly string Server = ConfigUtility.GetValue("redis.server");
        private static readonly string Port = ConfigUtility.GetValue("redis.port");
        private static readonly int Db = 1;
        /// <summary>
        ///     将字符串从左侧插入队列中，队列长度加一
        ///     LPUSH
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool Inqueue(string queueName, string str)
        {
            using (ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(Server + ":" + Port + ",resolvedns=1"))
            {
                IDatabase db = muxer.GetDatabase(Db);
                try
                {
                    db.ListLeftPush(queueName, str);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///     用来Trim来限制队列的长度
        ///     注意LTRIM命令和编程语言区间函数的区别
        ///     LTRIM list 0 10 ，结果是一个包含11个元素的列表，这表明 stop 下标也在 LTRIM 命令的取值范围之内(闭区间)
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="str"></param>
        /// <param name="queueLimit"></param>
        /// <returns></returns>
        public static bool LPushWithTrim(string queueName, string str, int queueLimit)
        {
            using (ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(Server + ":" + Port + ",resolvedns=1"))
            {
                IDatabase db = muxer.GetDatabase(Db);
                try
                {
                    db.ListLeftPush(queueName, str);
                    db.ListTrim(queueName, 0, queueLimit - 1);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///     从队列左侧中读出数据，队列长度减一
        ///     LPOP
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public static string OutQueue(string queueName)
        {
            using (ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(Server + ":" + Port + ",resolvedns=1"))
            {
                IDatabase db = muxer.GetDatabase(Db);
                return db.ListLeftPop(queueName);
            }
        }

        /// <summary>
        ///     计算队列长度
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public static long CountQueue(string queueName)
        {
            using (ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(Server + ":" + Port + ",resolvedns=1"))
            {
                IDatabase db = muxer.GetDatabase(Db);
                return db.ListLength(queueName);
            }
        }

        /// <summary>
        ///     清空/删除队列
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public static bool ClearQueue(string queueName)
        {
            using (ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(Server + ":" + Port + ",resolvedns=1"))
            {
                IDatabase db = muxer.GetDatabase(Db);
                try
                {
                    db.KeyDelete(queueName);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="hashId">Hash名，如 HeziTasks_1000007 </param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>插入是否成功</returns>
        public static bool SetHash(string hashId, string key, string value)
        {
            using (ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(Server + ":" + Port + ",resolvedns=1"))
            {
                IDatabase db = muxer.GetDatabase(Db);
                try
                {
                    return db.HashSet(hashId, key, value);
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="hashId">Hash名，如 HeziTasks_1000007 </param>
        /// <param name="expireTime">过期时间(毫秒时间戳 13位)</param>
        /// <returns>插入是否成功</returns>
        public static bool SetHashExpire(string hashId, DateTime expireTime)
        {
            using (ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(Server + ":" + Port + ",resolvedns=1"))
            {
                IDatabase db = muxer.GetDatabase(Db);
                try
                {
                    return db.KeyExpire(hashId, expireTime);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="hashId">Hash名，如 HeziTasks_1000007 </param>
        /// <param name="key">键</param>
        /// <returns>hash中key对应的value</returns>
        public static string GetHash(string hashId, string key)
        {
            using (ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(Server + ":" + Port + ",resolvedns=1"))
            {
                IDatabase db = muxer.GetDatabase(Db);
                return db.HashGet(hashId, key);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="hashId">Hash名，如 HeziTasks_1000007</param>
        /// <returns>hash中key对应的value</returns>
        public static Dictionary<string, string> GetAllEntriesFromHash(string hashId)
        {
            //            var dic = redisClient.GetAllEntriesFromHash(hashId);
            using (ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(Server + ":" + Port + ",resolvedns=1"))
            {
                IDatabase db = muxer.GetDatabase(Db);
                HashEntry[] all = db.HashGetAll(hashId);
                return all.ToDictionary<HashEntry, string, string>(hashEntry => hashEntry.Name,
                    hashEntry => hashEntry.Value);
            }
        }

        //        public static int Ttl(string hashId)
        //        {
        //            using (var muxer = ConnectionMultiplexer.Connect(Server + ":" + Port +",resolvedns=1"))
        //            {
        //                var db = muxer.GetDatabase(Db);
        //                return db.KeyTimeToLive(hashId);
        //            }
        //        }
    }
}