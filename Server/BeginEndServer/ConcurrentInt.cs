using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginEndServer
{
    public class ConcurrentInt
    {
        private int value;

        public ConcurrentInt(int value)
        {
            this.value = value;
        }

        public int Get()
        {
             return value;
        }

        public int Add()
        {
            lock (this)
            {
                value++;
                return value;
            }
        }

        public int Reduce()
        {
            lock (this)
            {
                value--;
                return value;
            }
        }


    }
}
