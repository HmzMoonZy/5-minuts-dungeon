using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginEndServer
{
    public interface IApplication
    {
        void OnConnect(object obj);

        void OnReceive(Conn conn, Message msg);

        void OnDisconnect(Conn obj);
    }
}
