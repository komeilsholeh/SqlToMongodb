using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlToMongodb
{
    class clsInsert
    {
 
        private string getCollectionName(string InsertCommand)
        {
            string[] command = InsertCommand.Split(' ');
            return command[2];
        }

        private string[] getFields(string InsertCommand)
        {
            string[] command = InsertCommand.Split(' ');
            
            int i = 3;
            string fields = "";

            while (!command[i].ToLower().Equals("values"))
            {
                fields = fields + command[i].Trim();
                i++;
            }

            if (fields.Substring(0).Equals('(') && fields.Substring(fields.Length - 1).Equals(')'))
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
