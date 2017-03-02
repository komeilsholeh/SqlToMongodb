using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlToMongodb
{
    public class Class1
    {
        static void Main(string[] args)
        {
            MongodbConnection mc = new MongodbConnection();
            BsonDocument person = new BsonDocument {
               { "first_name", "Steven"},
               { "last_name", "Edouard"},
               { "accounts", new BsonArray {
                   new BsonDocument {
                       { "account_balance", 50000000},
                       { "account_type", "Investment"},
                       { "currency", "USD"}
                   }
               }}
           };
            mc.connect();
            mc.collection.Insert(person);
            System.Console.WriteLine(person["_id"]);
            System.Console.ReadLine();
        }

////
        private void commandConvertor(string SQLcommand)
        {
            string[] command =SQLcommand.Split(' ');

            switch (command[0].ToLower())
            {
                case "select":

                    break;
                case "insert":

                    break;
                case "update":

                    break;
                case "delete":
                    break;
                default:
                    break;
                            } 
        }





    }
}
