﻿using Microsoft.Azure.Cosmos;
using MongoDB.Driver;
using Sentinel.ConnectionChecks.Models;
using System.Text;

namespace Sentinel.ConnectionChecks.ConnectionCheck.CosmosDB
{
    internal static class TestCosmosDBConnection
    {
        public static async Task<TestNetConnectionResponse> TestConnectionAsync(CosmosDBConnectionCheckRequest request)
        {
            try
            {
                if (request.ConnectionString.Contains("mongodb://"))
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    var client = new MongoClient(request.ConnectionString);

                    var settings = client.Settings;

                    Console.WriteLine(settings.Server.Host);
                    stringBuilder.AppendLine(settings.Server.Host);
                    stringBuilder.AppendLine(settings.Server.Port.ToString());

                    if (!string.IsNullOrWhiteSpace(request.DatabaseName))
                    {
                        stringBuilder.AppendLine("DatabaseName Provided Listing Collection Names");
                        stringBuilder.AppendLine("=======================");
                        var collectionnames = client.GetDatabase(request.DatabaseName).ListCollectionNames();

                        foreach (var dbname in collectionnames.ToList())
                        {
                            Console.WriteLine(dbname);
                            stringBuilder.AppendLine(dbname);
                        }

                    }
                    else
                    {
                        stringBuilder.AppendLine("DatabaseName empty list DB Names");
                        stringBuilder.AppendLine("=======================");
                        var dbnames = client.ListDatabaseNames();

                        foreach (var dbname in dbnames.ToList())
                        {
                            Console.WriteLine(dbname);
                            stringBuilder.AppendLine(dbname);
                        }
                    }








                    return new TestNetConnectionResponse(CheckAccessRequestResourceType.CosmosDB, true, stringBuilder.ToString(), 0);
                }
                else
                {



                    var cosmosClient = new CosmosClient(connectionString: request.ConnectionString);

                    // var cosmosClient = new CosmosClient(request.ConnectionString);

                    var database = cosmosClient.GetDatabase(request.DatabaseName);
                    var container = database.GetContainer(request.ContainerName);
                    var query = container.GetItemQueryIterator<dynamic>("SELECT * FROM c");
                    var results = new List<dynamic>();
                    while (query.HasMoreResults)
                    {
                        var response = await query.ReadNextAsync();
                        results.AddRange(response);
                    }

                    return new TestNetConnectionResponse(CheckAccessRequestResourceType.CosmosDB, true, 0);
                }
            }
            catch (Exception ex)
            {
                return new TestNetConnectionResponse(CheckAccessRequestResourceType.CosmosDB, false, 0);
            }
        }
    }
}
