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
            // check the command and call related class
            switch (command[0].ToLower()) 
            {   
                case "select":
                    clsSelect sel = new clsSelect();
                    mongoCommand = sel.SelectDocument(SQLcommand);
                    break;
                case "insert":
                    clsInsert ins = new clsInsert();
                    mongoCommand= ins.InsertDocument(SQLcommand);                    
                    break;
                case "update":
                    clsUpdate upd = new clsUpdate();
                    mongoCommand = upd.UpdateDocument(SQLcommand);
                    break;
                case "delete":
                    clsDelete del = new clsDelete();
                    mongoCommand = del.DeleteDocument(SQLcommand);
                    break;
                default:
                    mongoCommand = "syntax error";
                    break;
            }
            return mongoCommand; 
        }
    }
}
