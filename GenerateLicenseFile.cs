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
        public static void Run([QueueTrigger("outputQueueItem", Connection = "AzureWebJobsStorage")] Order myQueueItem,
            ILogger log, 
            IBinder binder)
        {
            log.LogInformation($"Order received and placed in queue storage: {myQueueItem.OrderId} {myQueueItem.ProductId}");
            using (var blobbyBrown = binder.Bind<TextWriter>(new BlobAttribute($"licenses/{myQueueItem.OrderId}.lic")))
            {
                blobbyBrown.WriteLine($"OrderId: {myQueueItem.OrderId}");
                blobbyBrown.WriteLine($"Receiving Email: {myQueueItem.ToEmail}");
                blobbyBrown.WriteLine($"Sending Email: {myQueueItem.FromEmail}");
                blobbyBrown.WriteLine($"ProductId: {myQueueItem.ProductId}");
                blobbyBrown.WriteLine($"Price : {myQueueItem.Price}");

                var md5 = MD5.Create();
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(myQueueItem.ToEmail + "secret"));
                blobbyBrown.WriteLine($"Secretcode: {BitConverter.ToString(hash).Replace("-", "")}");
            }          
        }
    }
}
