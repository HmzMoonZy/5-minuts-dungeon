using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BeginEndServer;
using BeginEndServer.Util;
using Server.Dot;
using Server.Dto;
using Server.Logic;

namespace Server
{
    public class Program
    {
        public static BEServer server = new BEServer();

        static void Main(string[] args)
        {
            //开启服务器底层
            server.Start("127.0.0.1", 9264, 50);

            //实例化应用层
            server.SetApplication(new MsgCenter());

            //连接数据库
            DatabaseMgr._Instance.Connect();

            //开启心跳检查
            MzTimerManager._Instance.AddTask(1, true, () =>
            {
                Sys.CheckHertBeat(server.usingConns, 180);
            });
            MzTimerManager._Instance.Start();


            //接收服务器指令
            while (true)
            {
                string command = Console.ReadLine();
                switch (command)
                {
                    case "quit":
                        return;

                    case "send":
                        send(server);
                        break;

                    case "":
                        break;
                    default:
                        Console.WriteLine("无法识别的指令");
                        break;
                }
            }
        }

        private static void send(BEServer server)
        {
            Console.WriteLine("***********************");
            Console.WriteLine("*******广播 模式********");
            Console.WriteLine("***********************");
            while (true)
            {
                string msg = Console.ReadLine();
                switch (msg)
                {
                    case "quit":
                        return;

                    default:
                        SystemHandler.Broadcast(null, server.usingConns, msg);
                        break;
                }
            }
        }
    }

}

