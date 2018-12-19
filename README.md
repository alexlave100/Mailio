# Azure serverless functions

Simple example of common bindings and their use cases when deploying Azure functions


For the SendGrid implementation, don't forget to create a SendGrid API Key and update the application settings in the Azure portal.



## The local.settings.json must also be added and configured if running the functions locally.
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
