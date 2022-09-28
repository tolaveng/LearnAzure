# create cosmos account
> az cosmosdb create --name learncosmosdbaccount --resource-group az204-rg
note:
 - account must be lower case !
 - documentEndpoint: 

# retrieve the primary key
> az cosmosdb keys list --name learnCosmosDbAccount --resource-group az204-rg

# .net
	- create client:  new CosmosClient(EndpointUri, PrimaryKey);
	- create database: cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
	- create container: database.CreateContainerIfNotExistsAsync(containerId, "/LastName");

# clean up
> az group delete --name az204-rg --no-wait