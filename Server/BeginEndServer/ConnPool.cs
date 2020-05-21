using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginEndServer
{
    public class ConnPool
    {
        private Queue<Conn> pool;
        public Queue<Conn> Pool { get { return pool; } }

        public int Count { get { return pool.Count; } }

        public ConnPool(int max)
        {
            pool = new Queue<Conn>(max);
        }

        public void EnQueue(Conn conn)
        {
            pool.Enqueue(conn);
        }

        public Conn Dequeue()
        {
            return pool.Dequeue();
        }

    }
}
