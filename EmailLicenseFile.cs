using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using Microsoft.Azure.WebJobs.Extensions.SendGrid;
using System;
using Microsoft.AspNetCore.Hosting.Server;
using Newtonsoft.Json;

namespace WebHook
{
    public static class EmailLicenseFile
    {
        [FunctionName("EmailLicenseFile")]
        public static void Run([BlobTrigger("licenses/{filename}.lic", Connection = "AzureWebJobsStorage")]string myBlob, 
            string filename, 
            IBinder binder,
            ILogger log,
            [SendGrid(ApiKey = "AzureWebJobsSendGridApiKey")] out SendGridMessage mailMessage)
        {
            var orderTable = binder.Bind<OrderTable>(new TableAttribute("orders", "Orders", filename));
            var outgoingBodies = new List<Content>();
            var recipients = new List<EmailAddress>();
            mailMessage = new SendGridMessage();
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{filename} \n Size: {myBlob.Length*2} Bytes");

            CreateCompleteEmail(outgoingBodies, recipients, mailMessage, orderTable, myBlob);
        }

        public static void CreateCompleteEmail (
            List<Content>outgoingBodies, 
            List<EmailAddress> recipients, 
            SendGridMessage mailMessage, 
            OrderTable orderTable, 
            string myBlob )
        {
            outgoingBodies.Add(new Content("text/plain", "This email is sent because an Azure serverless function was succesfully execeuted!\n\n Blob size: " + myBlob.Length * 2 + "bytes"));
            recipients.Add(new EmailAddress(orderTable.ToEmail));

            mailMessage.AddTos(recipients);
            mailMessage.AddContents(outgoingBodies);
            mailMessage.SetFrom(new EmailAddress(orderTable.FromEmail));
            mailMessage.SetSubject("Mottagen order");

            var attach = CreateAttachment(myBlob);
            mailMessage.AddAttachment(attach);
        }
        public static Attachment CreateAttachment(string myBlob)
        {
            var plainTextBytes = UTF8Encoding.UTF8.GetBytes(myBlob);           
            return new Attachment
            {
                Content = System.Convert.ToBase64String(plainTextBytes), Type = "application/json",
                Filename = "order", Disposition = "attachment", ContentId = "Json File"
            };
        }
    }   
}
