using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeginEndServer;
using BeginEndServer.Util;

namespace Server.Logic
{

    public class SystemHandler : IMessageHandler
    {
        private static SystemHandler instance;
        private SystemHandler() { }
        public static SystemHandler _Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SystemHandler();
                }
                return instance;
            }
        }

        public void ProcessData(Conn conn, int subCode, object parm)
        {
            switch (subCode)
            {
                case SystemSubCode.HERTBEAT:
                    conn.hertBeatTime = Sys.GetUtcTimeStamp();
                    break;

                case SystemSubCode.BROADCAST:
                    string broadcast = parm as string;
                    Console.WriteLine("广播消息: " + broadcast);
                    Broadcast(conn, Program.server.usingConns, broadcast);
                    break;

            }

        }
        /// <summary>
        /// 广播信息
        /// </summary>
        /// <param name="masterConn">信息源</param>
        /// <param name="list">广播列表</param>
        /// <param name="message">广播信息</param>
        public static void Broadcast(Conn masterConn, List<Conn> list, string message)
        {
            if (list.Count <= 0)
            {
                Console.WriteLine("没有正在连接的用户");
                return;
            }
            if (message == null || message == "")
            {
                return;
            }

            string broadcastStr = null;
            if (masterConn == null)
                broadcastStr = message;
            else
                broadcastStr = masterConn.GetRemoteAddress() + ":" + message;

            Message msg = new Message(OpCode.SYSTEM, SystemSubCode.BROADCAST, broadcastStr);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Send(msg);
            }

        }


    }
}
