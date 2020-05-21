using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginEndServer.DataBase
{
    public class SqlConn
    {
        public MySqlConnection sqlConn;

        public bool isRun = false;

        public void Open()
        {
            sqlConn.Open();
            isRun = true;
        }

        public void Close()
        {
            sqlConn.Close();
            isRun = false;
        }
    }
}
