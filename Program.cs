using System;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace documentdb_sqlapi
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", true);
            builder.AddCommandLine(args);
            var config = builder.Build();

            RunStuff(config["cosmosEndpoint"], config["cosmosToken"], config["databaseName"], config["containerName"], config["partitionKey"], config["filename"]).GetAwaiter().GetResult();
        }

        private static async Task RunStuff(string cosmosEndpoint, string cosmosToken, string databaseName, string containerName, string partitionKey, string filename)
        {
            CosmosClient client = new CosmosClient(cosmosEndpoint, cosmosToken);
            Database database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            Container container = await database.CreateContainerIfNotExistsAsync(containerName, partitionKey, 400);

            using (TextReader tr = new StreamReader(filename))
            {
                string inputstring = tr.ReadToEnd();
                var json = System.Text.Json.JsonDocument.Parse(inputstring);
                var jsonObject = JsonConvert.DeserializeObject<PimEntity>(inputstring, new ExpandoObjectConverter());
                await container.UpsertItemAsync<PimEntity>(jsonObject);
            }
        }
    }

    public class PimEntity
    {
        public string EntityType { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("sourceId")]
        public string SourceId => Id;
        public dynamic Fields { get; set; }
    }
}
