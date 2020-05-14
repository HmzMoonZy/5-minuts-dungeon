using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class PlayerSocket
{
    public Socket socket;

    //接收数据缓存(packet)
    private static byte[] receiveCache;
    //拆包后的数据
    private static List<byte> receiveData;
    //数据处理状态
    private static bool isParsing;
    //消息队列
    private static Queue<Message> msgQueue;


    public PlayerSocket()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        receiveCache = new byte[1024];
        receiveData = new List<byte>();
        isParsing = false;
        msgQueue = new Queue<Message>();
    }

    /// <summary>
    /// 尝试连接服务器,并开始异步接收数据
    /// </summary>
    public void Connect(string ipaddress, int port, Action sucCallback)
    {
        IPEndPoint serverPoint = new IPEndPoint(IPAddress.Parse(ipaddress), port);
        try
        {
            socket.Connect(serverPoint);
            //BeginReceive(buffer:用于存储接收到的数据, buffer开始计数的位置, 最大计数位置, SocketFlags值的按位组合, callback, callback的参数)
            socket.BeginReceive(receiveCache, 0, receiveCache.Length, SocketFlags.None, receiveCallbcak, socket);

            sucCallback();
        }
        //Call
        catch (Exception e) { Debug.Log(e.Message); }
    }



    #region 接收数据
    private void receiveCallbcak(IAsyncResult ar)
    {
        try
        {
            //接收到的是封装好的packet, 将它放进缓存中再依次解析
            //=> 解析需要时间,防止连续收到信息时解析出错.
            int packetLenth = socket.EndReceive(ar);        //接收到的数据大小
            byte[] packet = new byte[packetLenth];
            Buffer.BlockCopy(receiveCache, 0, packet, 0, packetLenth);
            receiveData.AddRange(packet);

            if (!isParsing)
                processReceive();

        }
        catch (Exception e) { Debug.LogError(e.Message); }
    }

    private static void processReceive()
    {
        isParsing = true;

        byte[] msgbuffer = MzTransmitter.UnPacket(ref receiveData);
        if (msgbuffer != null)
        {
            Message msg = MzTransmitter.BufferToMzMessage(msgbuffer);
            msgQueue.Enqueue(msg);
            startExcuteMsg();
        }
        else
        {
            isParsing = false;
            return;
        }

        //processReceive();
    }

    private static void startExcuteMsg()
    {
        Message msg = msgQueue.Dequeue();
        Debug.Log(msg.OpCode);
    }
    #endregion

    #region 发送数据
    /// <summary>
    /// 发送一条MzMessage类型的消息.
    /// </summary>
    public void Send(Message msg)
    {
        startSendMsg(msg);
    }
    /// <summary>
    /// 发送一条消息.
    /// </summary>
    public void Send(int opCode, int subCode, object param)
    {
        Message msg = new Message(opCode, subCode, param);
        startSendMsg(msg);
    }

    /// <summary>
    /// 将数据存入消息队列
    /// </summary>
    private void startSendMsg(Message msg)
    {
        if (msg != null)
        {
            byte[] msgdata = MzTransmitter.MzMessageToBuffer(msg);  //序列化msg信息
            byte[] packet = MzTransmitter.ToPacket(msgdata);        //将msg信息装包

            try
            {
                socket.Send(packet);

            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
    #endregion

}
