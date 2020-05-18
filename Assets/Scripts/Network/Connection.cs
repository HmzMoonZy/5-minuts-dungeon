using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

/// <summary>
/// 用于连接服务器的连接对象
/// </summary>
public class Connection
{
    /// <summary>
    /// 用于连接的Socket
    /// </summary>
    private Socket socket;

    /// <summary>
    ///缓存数组字节数
    /// </summary>
    private const int BUFFER_SIZE = 1024;

    //readBuffer
    /// <summary>
    /// 读缓存区
    /// </summary>
    private byte[] readBuffer = new byte[BUFFER_SIZE];
    /// <summary>
    /// 读缓存区的尾指针, 表示缓存区中准备解析的数据长度.
    /// </summary>
    private int bufferCount = 0;
    /// <summary>
    /// 包头,用于存储序列化后的数据长度(4字节)
    /// </summary>
    private byte[] packetHead = new byte[sizeof(int)];

    //Protocol

    public MsgDistribution msgDist = new MsgDistribution();

    //HertBeat
    private float hertBeatTime = 30;
    private float lastHertBeat = 0;

    public enum Status { None, Conneted };
    public Status status = Status.None;

    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <param name="cb">回调函数</param>
    /// <returns></returns>
    public bool Connect(string ipAddress, int prot, Action cb = null)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), prot);
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endPoint);

            socket.BeginReceive(readBuffer, bufferCount, BUFFER_SIZE - bufferCount, SocketFlags.None, receiveCallback, readBuffer);

            cb?.Invoke();

            status = Status.Conneted;

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("连接服务器失败:" + e.Message);
            UIPanelMgr._Instance.OpenPanel<UIPromptTips>("", "Connected Erro: " + e.Message);
            return false;
        }
    }
    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <param name="cb">连接成功后的回调函数</param>
    /// <returns></returns>
    public bool Connect(Action cb = null)
    {
        return Connect(NetWorkConst.GameServerIpAddress, NetWorkConst.GameServerPortNumber, cb);
    }

    public void Update()
    {
        msgDist.Update();

        //心跳
        if (status == Status.Conneted)
        {
            if (Time.time - lastHertBeat >= hertBeatTime)
            {
                Send(ConnMgr.HertBeat());
                lastHertBeat = Time.time;
            }
        }
    }

    private void receiveCallback(IAsyncResult ar)
    {
        try
        {
            int count = socket.EndReceive(ar);
            bufferCount += count;

            processPacket();

            socket.BeginReceive(readBuffer, bufferCount, BUFFER_SIZE - bufferCount, SocketFlags.None, receiveCallback, readBuffer);
        }
        catch (Exception e)
        {
            Debug.LogError("接收数据错误:" + e.Message);
            throw;
        }
    }

    private void processPacket()
    {
        int msgLenth;
        byte[] msgBuff = MzTransmitter.UnPacket(readBuffer, bufferCount, out msgLenth);

        if (msgBuff != null)
        {
            Message msg = MzTransmitter.DeserializableMessage(msgBuff);

            lock (msgDist.msgList) { msgDist.msgList.Add(msg); }

            //将剩余未解析的消息复制到buffer首,并将指针提前
            int count = bufferCount - sizeof(int) - msgLenth;
            Array.Copy(readBuffer, sizeof(int) + msgLenth, readBuffer, 0, count);
            bufferCount = count;

            //继续解析剩余消息
            if (bufferCount > 0)
                processPacket();
        }

    }

    #region Send 发送方法
    /// <summary>
    /// 给服务器发送一个消息
    /// </summary>
    public bool Send(Message msg)
    {
        if (status != Status.Conneted)
        {
            Debug.LogWarning("尚未连接服务器");
            return false;
        }

        byte[] msgBuff = MzTransmitter.SerializableMessage(msg);
        byte[] packet = MzTransmitter.ToPacket(msgBuff);

        socket.Send(packet);

        return true;
    }

    /// <summary>
    /// 分别指定一个 Message 的信息并发送给服务器
    /// </summary>
    public bool Send(int opCode, int subCode, object param)
    {
        Message msg = new Message(opCode, subCode, param);
        return Send(msg);
    }

    /// <summary>
    /// 向服务器发送一个 Message , 并注册一个对应的委托回调.
    /// </summary>
    public bool Send(Message msg, MsgDistribution.ProtolDelegate cb)
    {
        msgDist.AddOnceListener(msg.Token, cb);

        return Send(msg);
    }
    #endregion

    public bool Close()
    {
        try
        {
            socket.Close();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
    }
}
