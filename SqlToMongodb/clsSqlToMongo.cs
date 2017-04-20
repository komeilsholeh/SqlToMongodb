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
        public string commandConvertor(string SQLcommand)
        {
            string[] command =SQLcommand.Split(' ');
            string mongoCommand = "";
            switch (command[0].ToLower())
            {
                case "select":
                    // calling related class and method;
                    break;
                case "insert":
                    // calling related class and method;
                    clsInsert ci = new clsInsert();
                    mongoCommand= ci.InsertDocument(SQLcommand);                    
                    break;
                case "update":
                    // calling related class and method;
                    break;
                case "delete":
                    // calling related class and method;
                    break;
                default:
                    break;
            }
            return mongoCommand; 
        }
    }
}
