using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlToMongodb
{
    public class clsSqlToMongo
    {
        //static void Main(string[] args)
        //{
        //    MongodbConnection mc = new MongodbConnection();
        //    BsonDocument person = new BsonDocument {
        //       { "first_name", "Steven"},
        //       { "last_name", "Edouard"},
        //       { "accounts", new BsonArray {
        //           new BsonDocument {
        //               { "account_balance", 50000000},
        //               { "account_type", "Investment"},
        //               { "currency", "USD"}
        //           }
        //       }}
        //   };
        //    mc.connect();
        //    mc.collection.Insert(person);
        //    System.Console.WriteLine(person["_id"]);
        //    System.Console.ReadLine();
        //}

////
        public string commandConvertor(string SQLcommand)
        {
            string[] command =SQLcommand.Split(' ');
            string mongoCommand="";
            switch (command[0].ToLower())
            {
                case "select":

                    break;
                case "insert":
                    clsInsert ci = new clsInsert();
                    mongoCommand = ci.getJsonCommand(SQLcommand);
                    
                    break;
                case "update":

                    break;
                case "delete":
                    break;
                default:
                    break;
            }
            return mongoCommand; 
        }





    }
}
