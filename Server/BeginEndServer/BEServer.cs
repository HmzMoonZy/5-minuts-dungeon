using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using BeginEndServer.Util;

namespace BeginEndServer
{
    public class BEServer
    {
        // 监听端口(Accept)的Socket
        public Socket listenSoc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public ConnPool connPool;

        public int maxConn;
        public List<Conn> usingConns;

        private IApplication application;
        public void SetApplication(IApplication app) { this.application = app; }


        /// <summary>
        /// 开启服务器, 对指定端口进行监听.
        /// </summary>
        /// <param name="ip">ip地址的点分十进制</param>
        /// <param name="prot">需要监听的端口号</param>
        /// <param name="maxClient">最大连接客户端数量</param>
        public void Start(string ip, int prot, int maxConn)
        {
            //初始化连接池
            this.maxConn = maxConn;
            usingConns = new List<Conn>();
            connPool = new ConnPool(maxConn);
            for (int i = 0; i < maxConn; i++)
            {
                connPool.EnQueue(new Conn());
            }

            //尝试开启服务器
            try
            {
                //绑定服务器
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), prot);//需要监听的节点
                listenSoc.Bind(endPoint);           //绑定监听端口
                listenSoc.Listen(maxConn);          //等待队列的最大数量

                listenSoc.BeginAccept(beginAcceptCb, null);
                Console.WriteLine("服务器启动成功!");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

        }

        /// <summary>
        ///  listenSoc.BeginAccept()的回调函数.
        ///  1.给新连接分配Socket.
        ///  2.异步接收客户端数据
        /// </summary>
        private void beginAcceptCb(IAsyncResult ar)
        {
            try
            {
                application.OnConnect(null);
                Socket socket = listenSoc.EndAccept(ar);    //为该客户端开辟一个新连接
                //Console.WriteLine("新用户进入服务器, 正在分配连接...");

                if (connPool.Count > 0)
                {
                    Conn conn = connPool.Dequeue();
                    conn.Init(socket);
                    conn.processComplite += application.OnReceive;
                    conn.connDisconnect += connDisConnect;
                    usingConns.Add(conn);

                    Console.WriteLine("分配成功! 客户端地址为:" + conn.GetRemoteAddress());
                    Console.WriteLine("当前连接池剩余:" + connPool.Count);

                    //开始接收数据
                    conn.socket.BeginReceive(conn.readBuffer, conn.bufferCount, conn.BufferRemain(), SocketFlags.None, beginReceveCb, conn);
                }
                else
                {
                    //TODO 用户断开连接,通知客户端
                    socket.Close();
                    Console.WriteLine("连接池空");
                }

                //尾递归
                listenSoc.BeginAccept(beginAcceptCb, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// BeginReceive()的回调
        /// 1.接收并处理消息
        /// 2.等待信号断开连接
        /// </summary>
        private void beginReceveCb(IAsyncResult ar)
        {
            Conn conn = ar.AsyncState as Conn;

            try
            {
                int count = conn.socket.EndReceive(ar);  //接收的数据长度
                conn.bufferCount += count;
                if (count <= 0)
                {
                    //TODO 断开连接
                    Console.WriteLine("断开连接");
                    return;
                }

                //交由Conn自己解析数据
                conn.ProcessPacket();

                //尾递归
                conn.socket.BeginReceive(conn.readBuffer, conn.bufferCount, conn.BufferRemain(), SocketFlags.None, beginReceveCb, conn);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void connDisConnect(Conn conn)
        {
            application.OnDisconnect(conn);
            //更新使用列表
            lock (usingConns)
            {
                usingConns.Remove(conn);
            }
            connPool.EnQueue(conn);

        }

        /// <summary>
        /// 关闭服务器
        /// </summary>
        public void Close()
        {
            //通知所有Conn关闭连接
            foreach (Conn conn in connPool.Pool)
            {
                if (conn.isUse)
                {
                    conn.Disconnect();
                }
            }
        }



    }
}
