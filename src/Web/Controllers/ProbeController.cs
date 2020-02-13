using System;
using System.Text;
using System.Web.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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
                var body = Encoding.UTF8.GetBytes(message.Text);

                var ampqUri = Environment.GetEnvironmentVariable("rabbitmq:client:uri");

                var connectionFactory = new ConnectionFactory { Uri = new Uri(ampqUri) };

                using (var connection = connectionFactory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: QueueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                        channel.BasicPublish(exchange: "",
                                     routingKey: QueueName,
                                     basicProperties: null,
                                     body: body);
                    }
                }

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
        public ActionResult Receive(Models.Message msg)
        {
            try
            {
                var ampqUri = Environment.GetEnvironmentVariable("rabbitmq:client:uri");

                var connectionFactory = new ConnectionFactory { Uri = new Uri(ampqUri) };

                using (var connection = connectionFactory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: QueueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                        // TODO: rewrite this
                        var consumer = new QueueingBasicConsumer(channel);

                        channel.BasicConsume(queue: QueueName,
                                             autoAck: true,
                                             consumer: consumer);

                        ViewData["Status"] = "No messages found.";

                        var message = string.Empty;

                        while (consumer.Queue.Dequeue(1000, out BasicDeliverEventArgs ea))
                        {
                            message += $"{Encoding.UTF8.GetString(ea.Body)}; ";
                        }

                        if (!string.IsNullOrWhiteSpace(message))
                        {
                            ViewData["Status"] = message;
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
    }
}
