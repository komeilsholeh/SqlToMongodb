using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SqlToMongodb
{
    class clsInsert
    {
        private string seperator = ".^arr.";
        public ArrayList getJsonCommand(string InsertCommand)
        {
            ArrayList commandArray = new ArrayList();
            string[] values= getValues(InsertCommand);
            string[] fields = getField(InsertCommand);
            if (values.Length % fields.Length ==0 )
            {
                
                int valueCounter = 0;
                for(int i=0; i< values.Length / fields.Length;i++)
                {
                    string command= "{";
                    for (int j = 0; j < fields.Length; j++)
                    {
                        command = command + "{ \"" + fields[i] + "\" = \"" + values[valueCounter] + "\"}";
                    }
                    command = command + "}";
                }
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
