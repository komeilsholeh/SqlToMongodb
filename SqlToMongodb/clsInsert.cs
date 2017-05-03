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
        // calling json generate method and convert json string to bson document for mongo
        public string InsertDocument(string InsertCommand)
        {
            MongodbConnection mc = new MongodbConnection();
            string mongoCommand = getJsonCommand(InsertCommand);
            MongoDB.Bson.BsonDocument document = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(mongoCommand);
            mc.connect();
            mc.collection.Insert(document);
            return mongoCommand;
        }

        public string getJsonCommand(string InsertCommand)
        {
            // getting fields and values into string array
            string[] values = getValues(InsertCommand); 
            string[] fields = getFields(InsertCommand);

            // getting substring of all field into a string
            int start = InsertCommand.IndexOf("(") + 1;
            int end = InsertCommand.ToLower().IndexOf("values") - 1;
            string fieldsStr = InsertCommand.Trim().Substring(start, end - start - 1).Trim();

            // getting substring of all values into a string
            start = InsertCommand.ToLower().IndexOf("values") + 6;
            end = InsertCommand.LastIndexOf(")");
            string valuesStr = InsertCommand.Substring(start, end - start).Trim();
            valuesStr = valuesStr.Substring(1, valuesStr.Length - 1);
            ArrayList docFields = new ArrayList();
            string command = "{";
            int cnt = 0;
            char[] seperators = { '[', ']', '(', ')' };
            string value;

            //////////// generating JSON //////////////
            // checking all fields one by one for different conditions
            for (int i = 0; i < fields.Length; i++)
            {
                // check if the field has no array or embedded document
                if(fields[i].IndexOfAny(seperators) == -1)
                {   
                    // add field and value in json format to a string
                    command += "\"" + getString(fields[i]) + "\" : \"" + getString(values[cnt]) + "\"";
                    cnt++;
                }
                // check if the field is an array
                if (fields[i].Trim().EndsWith("[]") == true)
                {   
                    // add the array name to json string
                    command += "\"" + fields[i].Substring(0,fields[i].Trim().IndexOf('[')) + "\" : [";

                    // check values of the array field and add them to the json string
                    do
                    {
                        if (command.EndsWith("[") == false) command += ",";
                        value = getString(values[cnt]);
                        command += " \"" + value + "\"";
                        cnt++;
                    } while (values[cnt-1].IndexOf(']') == -1);
                    command += " ]";
                }
                // check if the field is array of embedded documents
                if (fields[i].IndexOf('[') != -1 && fields[i].IndexOf('(') != -1)
                {   
                    // get the name of array
                    command += "\"" + fields[i].Substring(0, fields[i].Trim().IndexOf('[')) + "\" : [";
                    docFields.Add(getString(fields[i].Substring(fields[i].IndexOf('(')).Replace(")","")));
                   
                    // getting fields in the embedded document 
                    while (fields[i].IndexOf(')') == -1) 
                    {
                        i++;
                        docFields.Add(getString(fields[i]));
                    }
                    
                    // getting the values of embedded document and add fields and values into json string
                    while (values[cnt-1].IndexOf(']')==-1 && cnt <= values.Length)
                    {
                        if (command.EndsWith("}") == true) command += ",";
                        command += "{";
                        for(int j = 0; j < docFields.Count; j++)
                        {
                            command+= " \"" + docFields[j].ToString() + "\" : \"" + getString(values[cnt]) + "\"";
                            cnt++;
                            if (j < docFields.Count - 1)
                            {
                                command = command + ",";
                            }
                        }
                        command += "}";
                    }
                    command += "]";
                }
                
                // check if the field is an embedded document
                if (fields[i].IndexOf('[') == -1 && fields[i].IndexOf('(') != -1)
                {
                    command += "\"" + fields[i].Substring(0, fields[i].Trim().IndexOf('(')) + "\" : { \""
                        + fields[i].Substring(fields[i].IndexOf('(') +1) + "\" : " + "\"" + getString(values[cnt]) + "\"";
                    cnt++;
                }
                if (fields[i].Trim().EndsWith(")"))
                {
                    command += "\"" + fields[i].Substring(0,fields[i].Length-1) + "\" : \"" + getString(values[cnt]) + "\" }";
                    cnt++;
                }
                if (i < fields.Length - 1)
                {
                    command = command + ",";
                }
            }
            command = command + "}";
            return command;
        }

        // removing all special characters from a string
        private string getString(string value)
        {   // checking start and end if each value and remove them if it is special
            value = value.Trim();
            if (value.StartsWith("[(") == true) value = value.Substring(2);
            if (value.EndsWith(")]") == true) value = value.Substring(0,value.Length-2);
            if (value.StartsWith("[") == true) value = value.Substring(1);
            if (value.StartsWith("(") == true) value = value.Substring(1);
            if (value.EndsWith(")") == true) value = value.Substring(0, value.Length - 1);
            if (value.EndsWith("]") == true) value = value.Substring(0, value.Length - 1);
            return value;
        }

        // getting collection name
        private string getCollectionName(string InsertCommand)
        {   
            // export collection name using the start and end index
            return InsertCommand.Substring(11, InsertCommand.IndexOf("(")-11).Trim();
        }
        // getting all fields 
        private string[] getFields(string InsertCommand)
        {   
            // exporting all fields
            int start = InsertCommand.IndexOf("(") + 1;
            int end = InsertCommand.ToLower().IndexOf("values") - 1;
            string fieldsStr = InsertCommand.Trim().Substring(start  , end - start -1).Trim();

            // seperate fields using seperator and adding them to an array
            string[] fieldsArray = fieldsStr.Split(',');
            return fieldsArray;
        }
        // getting all values
        private string[] getValues(string InsertCommand)
        {   
            // exporting all values 
            int start = InsertCommand.ToLower().IndexOf("values") + 6;
            int end = InsertCommand.LastIndexOf(")");            
            string valuesStr = InsertCommand.Substring(start, end - start).Trim();

            // seperate values using seperator and adding them to an array
            valuesStr = valuesStr.Substring(1, valuesStr.Length - 1);     
            string[] valuesArray = valuesStr.Split(',');
            return valuesArray;
        }
    }
}
