# Introduction 
An Event Sourcing, CQRS project using an Azure Cosmos DB.

# Description
This project is an example of using Event Sourcing with an Azure Cosmos DB. 
Additionally I have created an InMemory EventStore used by the unit testing.
This project uses the example of a restaurant, where an order is placed, and the order can be editted by 
adding and removing order items.

The Ezley.EventStore, Ezley.ProjectionStore, and Ezley.SnapshotStore projects were originally taken in large part from https://github.com/amolenk/CosmosEventSourcing by Sander Molenkamp. I found it originally through a youtube vid https://www.youtube.com/watch?v=UejwRlmV6E4&t=19s
that gives a nice intro to Event Sourcing. His code is available on GitHub at https://github.com/amolenk/CosmosEventSourcing. 
 
# Getting Started
1. You will need to create an Azure Cosmos DB (SQL API) service.
1. Then create a database in your Cosmos Db Account named 'restuarantdb'
  1. Create a container in the database called 'events' with a partition key of '/stream/id'. (your partition key will be less than 100 bytes in this example.)
  1.Create another container named 'leases' with a partition key of '/id'.
  1. Create another container named 'views' with a partition key of '/id'.
  1. Next add a Stored Procedure to the 'events' container. You'll find it in the codebase at 'Ezley.EventStore/js/sbAppendToStream.js'. Name it 'sbAppendToStream', and save it.
  1. Verify the Azure settings in your appSettings.Development.json files are correct for your 
cosmos db.


You'll need the Authorization key from your Azure db (Under 'Keys' in the menu for your Azure db). 
Use the Primary key as the appSettings 'AuthKey' value.


# Build and Test
1. In [Testing]/Ezley.Testing, set the config settings in 'TestConfig.cs' to match your Azure Cosmos Db.
1. Build your solution.
1. Run the test in _RunProjectionEngine.cs (now your projection engine is ready to build projections
when data is added to your event store). Let that continue to run in the background.
1. Next start your other tests by either running all tests in the solution or opening a specific file and running tests.



# Contribute
If you would like to contribute, create a fork and have at it. You can create pull requests with your improvements.
