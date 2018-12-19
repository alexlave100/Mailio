# Azure serverless functions

Example of common bindings and their use cases when developing Azure functions from VS17.



The program will execute an HTTP triggered function (WebHook.cs) when a POST request is made and add information into a queue and a storage table. Another azfunc (GenerateLicenseFile.cs) will be triggered when a new item is added to the queue. This queue item will be added to blob storage. Whenever a new item is added to the blob storage, a blob triggered function (EmailLicenseFile.cs) will execute. This function will retrieve the desired row from table storage for the email address of the sender and receiver of the request, create a new Email object (SendGridMessage) and send it, along with some additional information about the send data, to the receiver email address.

## POST request:
The POST request should have the same structure as the 'Order' object in WebHook.cs. 
## Example:
    {
	"OrderId": "1",
    "ProductId": "50",
	"ToEmail": "<YourOwnEmailIfYouWantToEnsureItworks>",
	"FromEmail": "<AnyValidEmailReally>",
    "Price": "1000"
    }

For the SendGrid implementation, don't forget to create a SendGrid Api Key and update the application settings in the Azure portal.



#### If running the solution locally:
The local.settings.json is .gitignored and must also be added and configured. The 'AzureWebJobsStorage' key value must be set to 'true' and the 'AzureWebJobsSendGridApiKey' must contain a valid Api key. 
## Example: 
    {
        "IsEncrypted": false,
        "Values": {
         "AzureWebJobsSendGridApiKey": <ApiKey>,
         "AzureWebJobsStorage": "UseDevelopmentStorage=true",
         "FUNCTIONS_WORKER_RUNTIME": "dotnet"
         },
    "ConnectionStrings": {}
    }
