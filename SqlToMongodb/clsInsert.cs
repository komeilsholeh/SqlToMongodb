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
            string command = getCollectionName(InsertCommand);
            //string[] fields= getFields(InsertCommand);
            //for(int i=0; i<fields.Length; i++)
            //{
            //    command = command + fields[i] + "/";
            //}
          
            return command;
        }
        private string getCollectionName(string InsertCommand)
        {
             return InsertCommand.Substring(11, InsertCommand.IndexOf("(")-11).Trim();
        }

        private string[] getFields(string InsertCommand)
        {

            int start = InsertCommand.IndexOf("(")+1;
            int end = InsertCommand.IndexOf(")");

            for(; start< end; start++)
            {
                
            }
            string fields = "";
            int j = InsertCommand.ToLower().IndexOf("(");
            while (!command[i].ToLower().Equals("values"))
            {
                fields = fields + command[i].Trim();
                i++;
            }


            if (fields.Substring(0,1).Equals("(") && fields.Substring(fields.Length - 1).Equals(")"))
            {
                fields = fields.Substring(1, fields.Length - 2);
            }

            command = fields.Split(',');           
            return command;
        }

        private void getValues(string InsertCommand)
        {
            int startindex = InsertCommand.ToLower().IndexOf("values");
            string values = InsertCommand.Substring(startindex + 6 ,InsertCommand.Length-1);
        }
    }
}
