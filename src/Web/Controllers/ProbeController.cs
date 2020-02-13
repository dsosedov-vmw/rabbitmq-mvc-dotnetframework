using System;
using System.Web.Mvc;
using RabbitMQ.Client;

namespace Web.Controllers
{
    public class ProbeController : Controller
    {
        const string QueueName = @"rabbitmq-mvc-dotnetframework-probe";

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var ampqUri = Environment.GetEnvironmentVariable("rabbitmq:client:uri");

                var connectionFactory = new ConnectionFactory { Uri = new Uri(ampqUri) };

                using (var connection = connectionFactory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        if (channel.IsOpen)
                        {
                            ViewData["Status"] = "Connection established successfully.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Status"] = ex.Message;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Send()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Send(Models.Message message)
        {
            try
            {
                ViewData["Status"] = "Message sent successfully.";
            }
            catch (Exception ex)
            {
                ViewData["Status"] = ex.Message;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Receive()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Receive(Models.Message message)
        {
            try
            {
                string msgBody;

                msgBody = "foo";

                ViewData["Status"] = msgBody;
            }
            catch (Exception ex)
            {
                ViewData["Status"] = ex.Message;
            }

            return View();
        }
    }
}
