using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ApiFront.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            //docker run -d --hostname my-rabbit --name some-rabbit -p 8080:15672 -e RABBITMQ_DEFAULT_USER=biplabhome -e RABBITMQ_DEFAULT_PASS=nakshal rabbitmq:3-management
            //RabbitMqHelper.RabbitMqPutMessageAsync();
            //RabbitMqHelper.RabbitMqReadMessageAsync();
            //RabbitMqHelper.RabbitMqCloseAsync();

            //docker run --name some-redis -p 6379:6379 -d redis 
            RedisHelper.Set("name", "Arindam");
            Console.WriteLine(RedisHelper.Get("name"));
        }

        [HttpGet]
        [Route("")]
        public string Get()
        {
            return "test";
        }
    }
}
