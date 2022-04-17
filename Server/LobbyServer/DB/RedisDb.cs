using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace LobbyServer.DB
{
    public class RedisDb: IDisposable
    {
        // TODO : Redis 커넥션 수정
        static string redisConnectionString = "localhost:6379";
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
    }
}
