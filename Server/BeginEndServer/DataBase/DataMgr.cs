using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BeginEndServer.DataBase
{
    public class DataMgr
    {
        //DataBase
        //public SqlConnPool sqlPool = new SqlConnPool(50);
        //public SqlConn sqlConn;
        public MySqlConnection sqlConn;
        public string dbname;
        public string dbhost;
        public string dbport;
        public string userid;
        public string password;

        public DataMgr() { }

        public void Connect()
        {
            //初始化数据库连接对象
            //sqlConn = sqlPool.Dequeue();
            sqlConn = new MySqlConnection(GetConnStr());
            try
            {
                sqlConn.Open();
                Console.WriteLine("连接至数据库");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

        }

        protected string GetConnStr()
        {
            return $"Database={dbname};Data Source={dbhost};User Id={userid};Password={password};port={dbport}";
        }

        /// <summary>
        /// 判断字符串中的是否有非法字符
        /// 用于防止SQL注入
        /// </summary>
        public bool IsSafeStr(string str)
        {
            bool result = !Regex.IsMatch(str, @"[- | ; | , | \/ | \( | \) | \{ | \} | & | * | + | \' | _]");

            return result;
        }

        //TODO
        //增

        //删

        //改

        //查
    }
}
