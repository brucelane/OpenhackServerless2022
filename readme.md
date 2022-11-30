# Openhack Serverless

Create a `local.settings.json` file with the following contents:

```json
{
  "IsEncrypted": false,
  "Values": {
    "CosmosDb": "ratingsDB",
    "CosmosCollOut": "ratingsContainer",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
  },
  "ConnectionStrings": {
    "CosmosConnection": "Connection string to be replaced with the PRIMARY CONNECTION STRING key in the settings of CosmosDB"
  }
}
```
