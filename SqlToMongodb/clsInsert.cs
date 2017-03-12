using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MongoDB.Bson;

namespace SqlToMongodb
{
    class clsInsert
    {
       
        public void InsertDocument(string InsertCommand)
        {
            MongodbConnection mc = new MongodbConnection();
            string mongoCommand = getJsonCommand(InsertCommand);
            
                
              //  BsonDocument doc= BsonDocument.Parse(mongoCommand[i].ToString());
                MongoDB.Bson.BsonDocument document = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(mongoCommand);

                mc.connect();
                mc.collection.Insert(document);
            
        }

        private string getJsonCommand(string InsertCommand)
        {         
            string[] values= getValues(InsertCommand);
            string[] fields = getFields(InsertCommand);
            string command = "";
            int valueCounter = 0;
            char[] seperators =['[', ']', '(', ')'];
            command= "{";
            for (int i = 0; i < fields.Length; i++)
            {
               if (fields[i].IndexOfAny(seperators) == -1)
               {
                    command = command + " \"" + fields[i].Trim() + "\" : \"" + values[valueCounter].Trim() + "\"";
                    valueCounter++;
                
               }else if (fields[i].IndexOf('(') != -1 )
               {
                            string docName = fields[i].Substring(0, fields[i].IndexOf('(')).Trim();
                            string fieldName = fields[i].Substring(fields[i].IndexOf('(') + 1).Trim();
                            command = command + " \"" + docName + "\" : { " 
                                + "\"" + fieldName + "\" : \"" + values[valueCounter].Trim() + "\" ";
                        }
                        else if (fields[i].IndexOf(']') != -1)
                        {
                            string fieldName = fields[i].Substring(0, fields[i].IndexOf(']')).Trim();
                            command = command + " \"" + fieldName + "\" : \"" + values[valueCounter].Trim() + "\" }]";
                        }

                        if (i < fields.Length-1)
                        {
                            command = command + ",";
                        }
                        
                    }
                    command = command + "}";               
                    
            return command;
        }
        private string getCollectionName(string InsertCommand)
        {
             return InsertCommand.Substring(11, InsertCommand.IndexOf("(")-11).Trim();
        }

        private string[] getFields(string InsertCommand)
        {
            int start = InsertCommand.IndexOf("(") + 1;
            int end = InsertCommand.ToLower().IndexOf("values") - 1;

            string fieldsStr = InsertCommand.Trim().Substring(start  , end - start -1).Trim();


            string[] fieldsArray = fieldsStr.Split(',');
            return fieldsArray;
        }

        private string[] getValues(string InsertCommand)
        {
            int start = InsertCommand.ToLower().IndexOf("values") + 6;
            int end = InsertCommand.LastIndexOf(")");
            
            string valuesStr = InsertCommand.Substring(start, end - start).Trim();
            valuesStr = valuesStr.Substring(1, valuesStr.Length - 1);
     
            string[] valuesArray = valuesStr.Split(',');
            return valuesArray;
        }
    }
}
