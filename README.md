# Azure serverless functions

Example of common bindings and their use cases when developing Azure functions from VS17.

The program will execute an HTTP triggered function (WebHook.cs) when a POST request is made and add information into a queue and a storage table. Another azfunc (GenerateLicenseFile.cs) will be triggered when a new item is added to the queue. This queue item will be added to blob storage. Whenever a new item is added to the blob storage, a blob triggered function (EmailLicenseFile.cs) will execute. This function will perform a lookup in the table storage for the email address of the sender and receiver of the request, create a new Email object (SendGridMessage) and send it, along with some additional information about the send data, to the receiver email address.

For the SendGrid implementation, don't forget to create a SendGrid Api Key and update the application settings in the Azure portal.



##### The local.settings.json must also be added and configured if running the functions locally. The 'AzureWebJobsStorage' key value must be set to 'true' and the 'AzureWebJobsSendGridApiKey' must contain a valid Api key. 
### Example: 
    {
        "IsEncrypted": false,
        "Values": {
         "AzureWebJobsSendGridApiKey": <ApiKey>,
         "AzureWebJobsStorage": "UseDevelopmentStorage=true",
         "FUNCTIONS_WORKER_RUNTIME": "dotnet"
         },
    "ConnectionStrings": {}
    }
