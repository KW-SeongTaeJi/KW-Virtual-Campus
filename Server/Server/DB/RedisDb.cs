using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.DB
{
    public class RedisDb: IDisposable
    {
        // TODO : Redis 커넥션 수정
        //static string redisConnectionString = "localhost:6379";
        static string redisConnectionString = "52.78.191.67:6379,password=rlaalsxo";
        ConnectionMultiplexer redisConnection;
        IDatabase redisDb;


        public RedisDb()
        {
            redisConnection = ConnectionMultiplexer.Connect(redisConnectionString);
            if (redisConnection.IsConnected)
            {
                redisDb = redisConnection.GetDatabase();
            }
        }

        public void Dispose()
        {
        }

        public string GetHash(string key, string field)
        {
            RedisValue value = redisDb.HashGet(key, field);
            return value.ToString();
        }

        public void Set(string key, string value)
        {
            HashEntry[] entry = { new HashEntry("data", value) };
            redisDb.HashSet(key, entry);
        }

        public bool Remove(string key)
        {
            return redisDb.KeyDelete(key);
        }
    }
}
