using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceStack.Redis;

namespace Helpers
{
    public class RedisHelper
    {
        public Lazy<RedisHelper> _redisHelper = new Lazy<RedisHelper>();
        public static string _hostName;
        public static RedisClient _redisClient;
        static RedisHelper()
        {
            _hostName = "127.0.0.1:6379";

            try
            {
                _redisClient = new RedisClient(_hostName);
            }
            catch (Exception ex)
            {
                
            }
        }
        public static string Get(string key)
        {
            return _redisClient.Get<string>(key);
        }
        public static bool Set(string key, string value)
        {
            bool isSuccess = false;
            if (_redisClient.Get<string>(key) == null)
            {
                isSuccess = _redisClient.Set(key, value);
            }
            return isSuccess;
        }
    }
}
