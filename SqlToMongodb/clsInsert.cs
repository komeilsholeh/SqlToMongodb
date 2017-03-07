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
        public string getJsonCommand(string InsertCommand)
        {
            string command = "";
            getValues(InsertCommand);
            ArrayList fields = getFields(InsertCommand);
            for (int i = 0; i < fields.Count; i++)
            {
                command = command + fields[i] + "/";
            }

            return command;
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

        private void getValues(string InsertCommand)
        {
            int start = InsertCommand.ToLower().IndexOf("values") + 6;
            int end = InsertCommand.LastIndexOf(")") + 1;
            
            string valuesStr = InsertCommand.Substring(start, end - start).Trim();

     
            string[] valuesArray = valuesStr.Split(',');
        }
    }
}
