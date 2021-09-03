using Common.Helper;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Redis
{
    public class RedisCacheManager : IRedisCacheManager
    {
        private readonly string redisConnenctionString;
        private readonly int defaultDB;
        private readonly bool IsOpenCache;
        public volatile ConnectionMultiplexer redisConnection;
        private readonly object redisConnectionLock = new object();
        public RedisCacheManager()
        {
            string redisConfiguration = Appsettings.app(new string[] { "AppSettings", "RedisCaching", "ConnectionString" });//获取连接字符串
            int defaultdb = Convert.ToInt32(Appsettings.app(new string[] { "AppSettings", "RedisCaching", "DefaultDB" }));//获取连接字符串
            bool isopencache = Appsettings.app(new string[] { "AppSettings", "RedisCaching", "Enabled" }).ToString() == "True";
            IsOpenCache = isopencache;
            if (string.IsNullOrWhiteSpace(redisConfiguration))
            {
                throw new ArgumentException("redis config is empty", nameof(redisConfiguration));
            }
            if (IsOpenCache)
            {
                this.redisConnenctionString = redisConfiguration;
                this.redisConnection = GetRedisConnection();
                this.defaultDB = defaultdb;
            }
        }

        /// <summary>
        /// 核心代码，获取连接实例
        /// 通过双if 夹lock的方式，实现单例模式
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer GetRedisConnection()
        {
            //如果已经连接实例，直接返回
            if (this.redisConnection != null && this.redisConnection.IsConnected)
            {
                return this.redisConnection;
            }
            //加锁，防止异步编程中，出现单例无效的问题
            lock (redisConnectionLock)
            {
                if (this.redisConnection != null)
                {
                    //释放redis连接
                    this.redisConnection.Dispose();
                }
                try
                {
                    this.redisConnection = ConnectionMultiplexer.Connect(redisConnenctionString);
                }
                catch (Exception)
                {

                    throw new Exception("Redis服务未启用，请开启该服务");
                }
            }
            return this.redisConnection;
        }

        public void Clear()
        {
            foreach (var endPoint in this.GetRedisConnection().GetEndPoints())
            {
                var server = this.GetRedisConnection().GetServer(endPoint);
                foreach (var key in server.Keys())
                {
                    redisConnection.GetDatabase().KeyDelete(key);
                }
            }
        }

        public bool Get(string key)
        {
            return redisConnection.GetDatabase(defaultDB).KeyExists(key);
        }

        public string GetValue(string key)
        {
            return redisConnection.GetDatabase(defaultDB).StringGet(key);
        }
        public TEntity Get<TEntity>(string key)
        {
            var value = redisConnection.GetDatabase(defaultDB).StringGet(key);
            if (value.HasValue)
            {
                //需要用的反序列化，将Redis存储的Byte[]，进行反序列化
                return SerializeHelper.Deserialize<TEntity>(value);
            }
            else
            {
                return default(TEntity);
            }
        }

        public void Remove(string key)
        {
            redisConnection.GetDatabase(defaultDB).KeyDelete(key);
        }

        public void Set(string key, object value, TimeSpan cacheTime)
        {
            if (value != null)
            {
                //序列化，将object值生成RedisValue
                redisConnection.GetDatabase(defaultDB).StringSet(key, SerializeHelper.Serialize(value), cacheTime);
            }
        }

        public bool SetValue(string key, byte[] value)
        {
            return redisConnection.GetDatabase(defaultDB).StringSet(key, value, TimeSpan.FromSeconds(120));
        }
        public bool GetEnable()
        {
            return IsOpenCache;
        }
    }
}
