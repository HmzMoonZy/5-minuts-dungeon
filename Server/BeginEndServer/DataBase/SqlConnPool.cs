using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginEndServer.DataBase
{
    public class SqlConnPool
    {
        private Queue<SqlConn> pool;

        public SqlConnPool(int max)
        {
            pool = new Queue<SqlConn>(max);
            for (int i = 0; i < max; i++)
            {
                pool.Enqueue(new SqlConn());
            }
        }

        public void EnQueue(SqlConn conn)
        {
            pool.Enqueue(conn);
        }

        public SqlConn Dequeue()
        {
            return pool.Dequeue();
        }

        public int GetCount()
        {
            return pool.Count;
        }

    }
}
