using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SqlToMongodb
{
    public class clsSqlToMongo
    {
        //static void Main(string[] args)
        //{
        //    MongodbConnection mc = new MongodbConnection();
        //BsonDocument person = new BsonDocument {
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
        BsonDocument te=new BsonDocument { { "Name", "'komeil'" }, { "family", "'sholeh'" }, { "age", "35" }, { "Contact", new BsonArray { new BsonDocument { { "Tel", "['0753'" }, { "Address]", "'ub8']" } } } }, { "NiNumber", "'sl73'" } };
        // mc.connect();
        //    mc.collection.Insert(person);
        //    System.Console.WriteLine(person["_id"]);
        //    System.Console.ReadLine();
        //}

            ////
        public string commandConvertor(string SQLcommand)
        {
            string[] command =SQLcommand.Split(' ');
            ArrayList mongoCommand = new ArrayList();
            switch (command[0].ToLower())
            {
                case "select":

                    break;
                case "insert":
                    clsInsert ci = new clsInsert();
                    ci.InsertDocument(SQLcommand);                    
                    break;
                case "update":

                    break;
                case "delete":
                    break;
                default:
                    break;
            }
            return mongoCommand[0].ToString(); 
        }





    }
}
