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
        private string seperator = ".^arr.";

        public void InsertDocument(string InsertCommand)
        {
            MongodbConnection mc = new MongodbConnection();
            ArrayList mongoCommand = new ArrayList();
            mongoCommand = getJsonCommand(InsertCommand);
            for(int i=0; i< mongoCommand.Count; i++)
            {
                string te;
                BsonDocument doc= BsonDocument.Parse(mongoCommand[i].ToString());
                MongoDB.Bson.BsonDocument document = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(mongoCommand[i].ToString());

                mc.connect();
                mc.collection.Insert(mongoCommand[i].ToString());
            }
        }

        private ArrayList getJsonCommand(string InsertCommand)
        {
            ArrayList commandArray = new ArrayList();
            string[] values= getValues(InsertCommand);
            string[] fields = getField(InsertCommand);
            if (values.Length % fields.Length ==0 )
            {
                string command="";
                int valueCounter = 0;
                for(int j=0; j< values.Length / fields.Length;j++)
                {
                    command= "";
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (fields[i].IndexOf('[') ==-1 && fields[i].IndexOf(']') == -1)
                        {
                            command = command + "{ \"" + fields[i].Trim() + "\" : \"" + values[valueCounter].Trim() + "\"}";
                        }else if (fields[i].IndexOf('[') != -1)
                        {
                            string arrayName = fields[i].Substring(0, fields[i].IndexOf('[')).Trim();
                            string fieldName = fields[i].Substring(fields[i].IndexOf('[') + 1).Trim();
                            command = command + "{ \"" + arrayName + "\" , new BsonArray { new BsonDocument { " 
                                + "{ \"" + fieldName + "\" , \"" + values[valueCounter].Trim() + "\" }";
                        }
                        else if (fields[i].IndexOf(']') != -1)
                        {
                            string arrayName = fields[i].Substring(0, fields[i].IndexOf(']') + 1).Trim();
                            command = command + "{ \"" + fields[i].Trim() + "\" : \"" + values[valueCounter].Trim() + "\"} }}}";
                        }

                        if (i < fields.Length-1)
                        {
                            command = command + ",";
                        }
                        valueCounter++;
                    }
                    command = command + "";
                }
                commandArray.Add(command);
            }        
            return commandArray;
        }
        private string getCollectionName(string InsertCommand)
        {
             return InsertCommand.Substring(11, InsertCommand.IndexOf("(")-11).Trim();
        }

        private ArrayList getFields(string InsertCommand)
        {
            int start = InsertCommand.IndexOf("(") + 1;
            int end = InsertCommand.IndexOf(")") +1 ;
            string fieldsStr = InsertCommand.Substring(start,end-start);
            ArrayList fieldsArray=new ArrayList();
            string fld = "";
            for (int i = 0; i < fieldsStr.Length; i++)
            {
                switch (fieldsStr.Substring(i, 1))
                {
                    case ")":
                        fieldsArray.Add(fld.Trim());
                        break;
                    case ",":
                        fieldsArray.Add(fld.Trim());
                        if (fld.LastIndexOf(seperator) == -1)
                        {
                            fld = "";
                        }
                        else
                        {
                            fld = fld.Substring(0, fld.LastIndexOf(seperator) + seperator.Length);
                        }
                        break;
                    case "[":
                        fld = fld + seperator;
                        break;
                    case "]":
                        fieldsArray.Add(fld.Trim());
                        if (fld.IndexOf(seperator) == fld.LastIndexOf(seperator))
                        {
                            fld = "";
                        }
                        else
                        {
                            fld = fld.Substring(0, fld.IndexOf(seperator) + seperator.Length);
                        }                        
                        i++;
                        break;
                    default:
                        fld = fld + fieldsStr.Substring(i, 1);
                        break;
                }
            }            
            return fieldsArray;

        }

        private string[] getField(string InsertCommand)
        {
            int start = InsertCommand.IndexOf("(");
            int end = InsertCommand.IndexOf(")");

            string fieldsStr = InsertCommand.Substring(start +1 , end - start-1).Trim();


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
