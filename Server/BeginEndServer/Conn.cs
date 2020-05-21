using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using BeginEndServer.Dto;
using System.IO;
using BeginEndServer.Util;

namespace BeginEndServer
{
    /// <summary>
    /// 连接对象类.
    /// 为每个用户(角色)提供了网络传输的相关方法.
    /// 同时监控每个用户的连接情况.
    /// </summary>
    public class Conn
    {

        /// <summary>
        /// 用于连接的Socket
        /// </summary>
        public Socket socket;

        /// <summary>
        /// 使用状态
        /// </summary>
        public bool isUse;

        /// <summary>
        ///缓存数组字节数
        /// </summary>
        public const int BUFFER_SIZE = 1024;

        /// <summary>
        /// 读缓存区
        /// </summary>
        public byte[] readBuffer = new byte[BUFFER_SIZE];

        /// <summary>
        /// 包头,用于存储序列化后的数据长度(4字节)
        /// </summary>
        public byte[] packetHead = new byte[sizeof(int)];

        /// <summary>
        /// 读缓存区的尾指针, 表示缓存区中准备解析的数据长度.
        /// </summary>
        public int bufferCount = 0;

        /// <summary>
        /// 心跳时间
        /// </summary>
        public long hertBeatTime = long.MinValue;

        /// <summary>
        /// 用于该链接的角色对象
        /// </summary>
        public PlayerBase player;

        //处理信息完毕委托
        public delegate void ProcessMsgCb(Conn conn, Message msg);
        public ProcessMsgCb processComplite;

        //断开连接委托
        public delegate void ConnDisconnectDelegate(Conn conn);
        public ConnDisconnectDelegate connDisconnect;

        public Conn()
        {
            readBuffer = new byte[BUFFER_SIZE];
        }

        /// <summary>
        /// 初始化对应的Socket及缓存状态, 将状态设为正在使用.
        /// </summary>
        /// <param name="socket"></param>
        public void Init(Socket socket)
        {
            this.socket = socket;
            isUse = true;
            bufferCount = 0;
            hertBeatTime = Sys.GetUtcTimeStamp();
        }

        public void ProcessPacket()
        {
            int msgLenth;
            byte[] msgBuff = MzTransmitter.UnPacket(readBuffer, bufferCount, out msgLenth);

            if (msgBuff != null)
            {
                Message msg = MzTransmitter.DeserializableMessage(msgBuff);

                processComplite(this, msg);


                //清楚解析完毕的信息缓存,将剩余消息复制到缓冲区首.
                int count = bufferCount - sizeof(int) - msgLenth;
                Array.Copy(readBuffer, sizeof(int) + msgLenth, readBuffer, 0, count);
                bufferCount = count;

                if (bufferCount > 0)
                    ProcessPacket();
            }
        }

        #region Send发送数据

        public void Send(Message msg)
        {
            byte[] msgBuff = MzTransmitter.SerializableMessage(msg);

            byte[] packet = MzTransmitter.ToPacket(msgBuff);
            try
            {
                socket.BeginSend(packet, 0, packet.Length, SocketFlags.None, null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Send(int opCode, int subCode, object parm)
        {
            Message msg = new Message(opCode, subCode, parm);
            Send(msg);
        }
        #endregion


        public int BufferRemain()
        {
            return BUFFER_SIZE - bufferCount;
        }

        public string GetRemoteAddress()
        {
            if (!isUse)
                return "该Socket尚未连接";
            return socket.RemoteEndPoint.ToString();
        }


        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if (!isUse)
                return;

            lock (this)
            {
                Console.WriteLine("断开连接" + GetRemoteAddress());
                player = null;
                isUse = false;
                connDisconnect(this);
                socket.Close();
            }

        }
    }
}
