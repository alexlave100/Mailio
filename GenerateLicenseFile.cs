using System;
using System.IO;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace WebHook
{
    public static class GenerateLicenseFile
    {
        [FunctionName("GenerateLicenseFile")]
        public static void Run([QueueTrigger("outputQueueItem", Connection = "AzureWebJobsStorage")] Order queueItem,
            ILogger log, 
            IBinder binder)
        {
            log.LogInformation($"Order received and placed in queue storage: {queueItem.OrderId} {queueItem.ProductId}");
            using (var blobbyBrown = binder.Bind<TextWriter>(new BlobAttribute($"licenses/{queueItem.OrderId}.lic")))
            {
                blobbyBrown.WriteLine($"OrderId: {queueItem.OrderId}");
                blobbyBrown.WriteLine($"Receiving Email: {queueItem.ToEmail}");
                blobbyBrown.WriteLine($"Sending Email: {queueItem.FromEmail}");
                blobbyBrown.WriteLine($"ProductId: {queueItem.ProductId}");
                blobbyBrown.WriteLine($"Price : {queueItem.Price}");

                var md5 = MD5.Create();
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(queueItem.ToEmail + "secret"));
                blobbyBrown.WriteLine($"Secretcode: {BitConverter.ToString(hash).Replace("-", "")}");
            }          
        }
    }
}
