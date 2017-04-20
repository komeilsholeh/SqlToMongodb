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
       
        public string InsertDocument(string InsertCommand)
        {
            //setValuesFields(InsertCommand);
            MongodbConnection mc = new MongodbConnection();
            string mongoCommand = setValuesFields(InsertCommand);


            ////  BsonDocument doc= BsonDocument.Parse(mongoCommand[i].ToString());
              MongoDB.Bson.BsonDocument document = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(mongoCommand);

              mc.connect();
              mc.collection.Insert(document);
            return mongoCommand;
        }

        private string getJsonCommand(string InsertCommand)
        {         
            string[] values= getValues(InsertCommand);
            string[] fields = getFields(InsertCommand);
            string command = "";
            int valueCounter = 0;
            char[] seperators = { '[', ']', '(', ')' };

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

        public string setValuesFields(string InsertCommand)
        {
            string[] values = getValues(InsertCommand);
            string[] fields = getFields(InsertCommand);

            int start = InsertCommand.IndexOf("(") + 1;
            int end = InsertCommand.ToLower().IndexOf("values") - 1;
            string fieldsStr = InsertCommand.Trim().Substring(start, end - start - 1).Trim();

            start = InsertCommand.ToLower().IndexOf("values") + 6;
            end = InsertCommand.LastIndexOf(")");
            string valuesStr = InsertCommand.Substring(start, end - start).Trim();
            valuesStr = valuesStr.Substring(1, valuesStr.Length - 1);

            ArrayList docFields = new ArrayList();

            string command = "{";
            int cnt = 0;
            char[] seperators = { '[', ']', '(', ')' };
            string value;
            for (int i = 0; i < fields.Length; i++)
            {

                // getting fields
                if(fields[i].IndexOfAny(seperators) == -1)
                {
                    command += "\"" + getString(fields[i]) + "\" : \"" + getString(values[cnt]) + "\"";
                    cnt++;
                }

                // getting arrays
                if (fields[i].Trim().EndsWith("[]") == true)
                {
                    command += "\"" + fields[i].Substring(0,fields[i].Trim().IndexOf('[')) + "\" : [";
                    do
                    {
                        if (command.EndsWith("[") == false) command += ",";
                        value = getString(values[cnt]);
                        command += " \"" + value + "\"";
                        cnt++;
                    } while (values[cnt-1].IndexOf(']') == -1);
                    command += " ]";
                }

                // getting document arrays 
                if (fields[i].IndexOf('[') != -1 && fields[i].IndexOf('(') != -1)
                {
                    command += "\"" + fields[i].Substring(0, fields[i].Trim().IndexOf('[')) + "\" : [";
                    docFields.Add(getString(fields[i].Substring(fields[i].IndexOf('(')).Replace(")","")));
                    while (fields[i].IndexOf(')') == -1)
                    {
                        i++;
                        docFields.Add(getString(fields[i]));
                    }
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

                // getting subdocuments
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

        private string getString(string value)
        {
            value = value.Trim();
            if (value.StartsWith("[(") == true) value = value.Substring(2);
            if (value.EndsWith(")]") == true) value = value.Substring(0,value.Length-2);
            if (value.StartsWith("[") == true) value = value.Substring(1);
            if (value.StartsWith("(") == true) value = value.Substring(1);
            if (value.EndsWith(")") == true) value = value.Substring(0, value.Length - 1);
            if (value.EndsWith("]") == true) value = value.Substring(0, value.Length - 1);
            return value;
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
