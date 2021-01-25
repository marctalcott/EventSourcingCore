# Introduction 
EventSourcing POC
This project is an example of using Event Sourcing with an Azure Cosmos DB. 
It uses the example of a bank account.
The core code responsible for interacting with Azure Cosmos DB was written by Sander 
Molenkamp. I found it originally through a youtube vid https://www.youtube.com/watch?v=UejwRlmV6E4&t=19s
that gives a nice intro to Event Sourcing. His code is available on GitHub at https://github.com/amolenk/CosmosEventSourcing

I see he has since updated it to include snapshotting, which I have not included here.


# Getting Started
You will need to create an Azure Cosmos DB (SQL API)
Then create a database in your Cosmos Db Account named 'bank-account-poc'
Create a container in the database called 'events'
 with a partition key of '/stream/id'. (your partition key will be less than 100 bytes in this example.)
Create another container named 'leases' with a partition key of '/id'.
Create another container named 'views' with a partition key of '/id'.
Next add a Stored Procedure to the 'events' container. You'll find it in the codebase
in 'EventStore/js/sbAppendToStream.js'. Name it (set the id to) 'sbAppendToStream', and save it.

Verify the Azure settings in your appSettings.Development.json file are correct for your 
cosmos db.

You'll need the Authorization key from your Azure db (Under 'Keys' in the menu for your Azure db). 
Use the Primary key as the appSettings 'AuthKey' value.




# Build and Test
Build your solution.
In POC.Testing, set the config settings in 'TestConfig.cs'.
Run the test in _RunProjectionEngine.cs (now your projection engine is ready to build projections
when data is added to your event store).



# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)