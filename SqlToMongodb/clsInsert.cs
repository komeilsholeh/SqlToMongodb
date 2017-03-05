using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlToMongodb
{
    class clsInsert
    {
        public string getJsonCommand(string InsertCommand)
        {
            string command = "";
            string[] fields = getFields(InsertCommand);
            for (int i = 0; i < fields.Length; i++)
            {
                command = command + fields[i] + "/";
            }

            return command;
        }
        private string getCollectionName(string InsertCommand)
        {
             return InsertCommand.Substring(11, InsertCommand.IndexOf("(")-11).Trim();
        }

        private string[] getFields(string InsertCommand)
        {
            int start = InsertCommand.IndexOf("(") + 1;
            int end = InsertCommand.IndexOf(")");
            string fieldsStr = InsertCommand.Substring(start,end-start);
            
            char[] seperator =[',', '[', ']'];
            string[] fieldsarray = fieldsStr.Split(seperator);
            for (int i = 0; i < fieldsarray.Length - 1; i++)
            {

            }
            string str;
            
            while (i > fieldsStr.Length - 1)
            {
                str = fieldsStr.Substring(i, fieldsStr.IndexOf(seperator));


            }
            


            string[] command = fields.Split(',');           
            return command;
        }

        private void getValues(string InsertCommand)
        {
            int startindex = InsertCommand.ToLower().IndexOf("values");
            string values = InsertCommand.Substring(startindex + 6 ,InsertCommand.Length-1);
        }
    }
}
