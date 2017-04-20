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

                    break;
                case "insert":
                    clsInsert ci = new clsInsert();
                    mongoCommand= ci.InsertDocument(SQLcommand);                    
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
