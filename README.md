# documentdb-sqlapi

Upload json file (document) to Cosmos Db Account using SQL API.

## Config needed

Takes command line and/or appsettings.json config.

* cosmosEndpoint - endpoint address from Azure portal (`https://<account name>.documents.azure.com:443/`)
* cosmosToken - acccess token from Azure portal
* databaseName - database name, will create db if it does not exists
* containerName - container name, will create container if it does not exists
* partitionKey - partition key from the doucment to use for partitioning
