using System;
using BeginEndServer;
using BeginEndServer.Util;

namespace Server.Logic
{

    //TODO  可以使用反射
    public class MsgCenter : IApplication
    {
        void IApplication.OnConnect(object obj)
        {

        }

        void IApplication.OnDisconnect(Conn conn)
        {
        
        }

        void IApplication.OnReceive(Conn conn, Message msg)
        {
            switch (msg.OpCode)
            {
                case OpCode.SYSTEM:
                    SystemHandler._Instance.ProcessData(conn, msg.SubCode, msg.Param);
                    break;

                case OpCode.ACCOUNT:
                    AccountHandler._Instance.ProcessData(conn, msg.SubCode, msg.Param);
                    break;

                case OpCode.GAME:
                    GameHandler._Instance.ProcessData(conn, msg.SubCode, msg.Param);
                    break;
            }

        }
    }
}
