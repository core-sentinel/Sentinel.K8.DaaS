using Sentinel.ConnectionChecks.ConnectionCheck.CosmosDB;
using Sentinel.ConnectionChecks.Models;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sentinel.ConnectionChecks.Tests
{
    public class CosmosDBDeserializationTests
    {

        string Json1 = @"{
              ""Url"": "".documents.azure.com"",
              ""Port"": 443,
              ""UseMSI"": false,
              ""SelectedAuthenticationType"": ""None"",
              ""DatabaseName"": ""testdb"",
              ""ContainerName"": """",
              ""ConnectionString"": """"
            }";


        public CosmosDBDeserializationTests()
        {

        }

        [Fact]
        public void deserialization()
        {

            Console.WriteLine(Json1);
            Assert.NotNull(Json1);

            //IBasicCheckAccessRequest
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CosmosDBConnectionCheckRequest>(Json1);
            Assert.NotNull(obj);
            obj.Url.ShouldBe(".documents.azure.com");

            var obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject<IBasicCheckAccessRequest>(Json1);
            Assert.NotNull(obj2);
            var cosobj = obj2 as CosmosDBConnectionCheckRequest;
            Assert.NotEmpty(cosobj.DatabaseName);



        }

        [Fact]
        public void serialization()
        {
            var obj = new CosmosDBConnectionCheckRequest()
            {
                Url = ".documents.azure.com",
                Port = 443,
                UseMSI = false,
                SelectedAuthenticationType = "None",
                DatabaseName = "testdb",
                ContainerName = "",
                ConnectionString = ""
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
            Assert.NotNull(json);
            Console.WriteLine(json);

        }


        [Fact]
        public void discovery_creation()
        {
            var obj = new CosmosDBConnectionCheckRequest()
            {
                Url = ".documents.azure.com",
                Port = 443,
                UseMSI = false,
                SelectedAuthenticationType = "None",
                DatabaseName = "testdb",
                ContainerName = "",
                ConnectionString = ""
            };

            var discovery = new ConnectionCheckDiscovery(typeof(CosmosDBConnectionCheckRequest));
            Assert.NotNull(discovery);
            Assert.True(discovery.Categories.ContainsKey("CosmosDBConnectionCheckRequest"));
            var checkRequest = discovery.CreateCheckAccessRequest("CosmosDBConnectionCheckRequest");
            Assert.NotNull(checkRequest);
            Assert.IsType<CosmosDBConnectionCheckRequest>(checkRequest);

            Dictionary<string,List<object>> requests = new Dictionary<string,List<object>>();

            requests["CosmosDBConnectionCheckRequest"] = new List<object> ()               
             as dynamic;
            requests["CosmosDBConnectionCheckRequest"].Add(obj);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requests, Newtonsoft.Json.Formatting.Indented);

            Assert.NotNull(json);


            var eleman = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<object>>>(json);

            Assert.NotNull(eleman);


        }
    }
}
