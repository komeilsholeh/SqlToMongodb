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
        public string getJsonCommand(string InsertCommand)
        {
            string command = "";
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
                        fieldsArray.Add(fld);
                        break;
                    case ",":
                        fieldsArray.Add(fld);
                        if (fld.LastIndexOf(".^arr.") == -1)
                        {
                            fld = "";
                        }
                        else
                        {
                            fld = fld.Substring(0, fld.LastIndexOf(".^arr.") + 6);
                        }
                        break;
                    case "[":
                        fld = fld + ".^arr.";
                        break;
                    case "]":
                        fieldsArray.Add(fld);
                        fld = fld.Substring(0, fld.IndexOf(".^arr.")+6);
                        i++;
                        break;
                    default:
                        fld = fld + fieldsStr.Substring(i, 1);
                        break;

                }
            }

            //char[] seperator =[',', '[', ']'];
            //string[] fieldsarray = fieldsStr.Split(seperator);
            //if (fieldsStr.IndexOf('[') != -1)
            //{
            //    for(int i = 0; i < fieldsarray.Length - 1; i++)
            //    {
            //        int fldpos = fieldsStr.IndexOf(fieldsarray[0]);
            //        if (fieldsStr.Substring(fldpos + fieldsarray[i].Length - 1, 1) == "[")
            //        {


            //        }
            //    }
            //}
            
            return fieldsArray;

        }

        private void getValues(string InsertCommand)
        {
            int startindex = InsertCommand.ToLower().IndexOf("values");
            string values = InsertCommand.Substring(startindex + 6 ,InsertCommand.Length-1);
        }
    }
}
