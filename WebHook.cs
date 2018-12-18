using System;
using System.IO;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.Storage ;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Queue;

namespace WebHook
{
    public class OrderTable
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string Price { get; set; }
    }
    public class Order
    {     
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string Price { get; set; }
    }
    public static class WebHook
    {
        [FunctionName("Start_WebHook")]
        public static async Task<object> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "create")] HttpRequest req, ILogger log, 
            [Queue("outputQueueItem", Connection = "AzureWebJobsStorage")] IAsyncCollector<Order> outputQueueItem, 
            [Table("orders", Connection = "AzureWebJobsStorage")] IAsyncCollector<OrderTable> orderTable)
        {
            log.LogInformation($"Orderinfo received");
            string jsonContent = await req.ReadAsStringAsync();

            var order = JsonConvert.DeserializeObject<Order>(jsonContent);
            var table = JsonConvert.DeserializeObject<OrderTable>(jsonContent);
            table.PartitionKey = "Orders";
            table.RowKey = order.OrderId;

            log.LogInformation($"Order {order.OrderId} recevied from {order.FromEmail} for product {order.ProductId}");

            await outputQueueItem.AddAsync(order);
            await orderTable.AddAsync(table);

            return new CreatedResult(
                "/api/route", $"Thank you for your order! Your order {order.ProductId} " +
                $"will be sent & and a confirmation will be sent to email {order.ToEmail}.");
        }
    }
}
