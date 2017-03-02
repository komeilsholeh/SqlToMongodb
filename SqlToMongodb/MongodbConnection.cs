using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SqlToMongodb
{
    
    class MongodbConnection
    {

        private static MongoClient client;
        private static MongoServer server;
        private static MongoDatabase dataBase;
        public MongoCollection<BsonDocument> collection;
        public void connect()
        {
            try
            {
                server.Ping();
                Console.WriteLine("Connected"); 
            }
            catch 
            {
                Console.WriteLine("Failed");
                client = new MongoClient("mongodb://127.0.0.1:27017");
                server = client.GetServer();
                dataBase = server.GetDatabase("local");
                collection = dataBase.GetCollection<BsonDocument>("komeiltest");
            }            
        }

        public void createCollection(String databaseName)
        {
            CollectionOptionsBuilder options = CollectionOptions.SetCapped(true);
            dataBase.CreateCollection(databaseName, options);
        }

    }
}
