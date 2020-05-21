using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

public static class MzTransmitter
{
    /// <summary>
    /// 加密字符串
    /// base64 -> md5
    /// </summary>
    public static string Encode(string str)
    {
        byte[] utf8buffer = Encoding.UTF8.GetBytes(str);
        string encode = Convert.ToBase64String(utf8buffer);
        Console.WriteLine("base64: " + encode);
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(encode);
        byte[] hash = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        Console.WriteLine(sb.ToString());
        return sb.ToString();
    }


    #region Tcp拆装包

    /// <summary>
    /// 在字节数组首部添加4字节数据,用于指示该字节数组长度
    /// </summary>
    public static byte[] ToPacket(byte[] data)
    {
        byte[] packet;

        //MemoryStream拥有较高的性能,需要byte[]作为它和其它类型的转换
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                //写入数据长度(包头)
                bw.Write(data.Length);
                //写入数据(包尾)
                bw.Write(data);

                packet = new byte[(int)ms.Length];
                Buffer.BlockCopy(ms.GetBuffer(), 0, packet, 0, (int)ms.Length);
            }
        }

        return packet;

    }

    /// <summary>
    /// 将包头+包尾格式的 Packet 拆包, 并更新至缓存
    /// 通常为序列化后的 MzMessage 及其长度.
    /// </summary>
    /// <returns>拆包后的包尾</returns>
    public static byte[] UnPacket(ref List<byte> dataCache)
    {
        if (dataCache.Count < 4)    //包头丢失
                                    //throw new Exception("包头丢失,数据长度不足4字节.");
            return null;

        using (MemoryStream ms = new MemoryStream(dataCache.ToArray()))
        {
            using (BinaryReader br = new BinaryReader(ms))
            {
                //从当前流中读取 4 字节有符号整数，并使流的当前位置(Position)提升 4 个字节。
                int dataLenth = br.ReadInt32();                         //包头给定长度
                int actualLenth = (int)(ms.Length - ms.Position);       //实际长度.

                if (dataLenth > actualLenth)                            //Like:96(ms.Lenth) - 32(position) = 64(dataLenth)
                                                                        //throw new Exception("数据长度不足包头指定长度,数据丢包.");
                    return null;

                byte[] data = br.ReadBytes(dataLenth);

                //更新缓存区(ref)
                dataCache.Clear();
                dataCache.AddRange(br.ReadBytes(actualLenth));      //从position开始写入

                return data;
            }
        }

    }

    public static byte[] UnPacket(byte[] packet, int bufferCount, out int msgLenth)
    {
        msgLenth = 0;

        if (packet.Length <= 0)
            return null;

        //解析包头
        byte[] head = new byte[4];
        Array.Copy(packet, head, sizeof(int));
        msgLenth = BitConverter.ToInt32(head, 0);
        if (bufferCount < msgLenth + 4)
            return null;

        //解析数据
        byte[] data = new byte[msgLenth];
        Array.Copy(packet, 4, data, 0, msgLenth);
        return data;
    }
    #endregion

    #region 序列化 MzMessage 类的相关方法
    /// <summary>
    /// 序列化MzMessage
    /// </summary>
    /// <param name="msg">需要序列化的 MzMessage</param>
    /// <param name="E">序列化 Value 的委托</param>
    public static byte[] MzMessageToBuffer(Message msg)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(msg.OpCode);   //写入操作码
                bw.Write(msg.SubCode);  //写入子操作吗
                if (msg.Param != null)  //写入参数值
                {
                    //序列化object类型的参数
                    byte[] paramBuffer = ObjectToBuffer(msg.Param);
                    bw.Write(paramBuffer);
                }

                int lenth = (int)ms.Length;
                byte[] msgBuffer = new byte[lenth];
                Buffer.BlockCopy(ms.GetBuffer(), 0, msgBuffer, 0, lenth);
                return msgBuffer;
            }
        }
    }

    /// <summary>
    /// 序列化Message
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static byte[] SerializableMessage(Message msg)
    {
        byte[] msgBuff;
        using (MemoryStream mStream = new MemoryStream())
        {
            using (BinaryWriter bw = new BinaryWriter(mStream))
            {

                bw.Write(msg.OpCode);
                bw.Write(msg.SubCode);
                if (msg.Param != null)
                {
                    byte[] temp = serializableObject(msg.Param);
                    bw.Write(temp);
                }
                int len = (int)mStream.Length;
                msgBuff = new byte[len];
                Buffer.BlockCopy(mStream.GetBuffer(), 0, msgBuff, 0, len);
            }


        }
        return msgBuff;
    }
    /// <summary>
    /// 反序列化 MzMessager
    /// </summary>
    /// <param name="msgBuffer">MzMessage 信息流</param>
    /// <param name="D">反序列化参数 Value 的委托</param>
    public static Message BufferToMzMessage(byte[] msgBuffer)
    {
        Message msg = new Message();

        using (MemoryStream ms = new MemoryStream(msgBuffer))
        {
            using (BinaryReader br = new BinaryReader(ms))
            {
                //int:4Byte
                msg.OpCode = br.ReadInt32();
                msg.SubCode = br.ReadInt32();

                if (ms.Length > ms.Position)
                {
                    //反序列化object类型的参数
                    byte[] paramBuffer = br.ReadBytes(((int)(ms.Length - ms.Position)));
                    msg.Param = BufferToObject(paramBuffer);
                }
            }
        }

        return msg;
    }

    public static Message DeserializableMessage(byte[] msgBuff)
    {
        Message msg = new Message();

        using (MemoryStream ms = new MemoryStream(msgBuff))
        {
            using (BinaryReader br = new BinaryReader(ms))
            {
                //int:4Byte
                msg.OpCode = br.ReadInt32();    //4
                msg.SubCode = br.ReadInt32();   //4

                if (ms.Length > ms.Position)
                {
                    //反序列化object类型的参数
                    byte[] paramBuff = br.ReadBytes(((int)(ms.Length - ms.Position)));
                    msg.Param = deserializableObject(paramBuff);
                }

                return msg;
            }
        }
    }

    #endregion

    #region 序列化object类的方法, 暂时用于序列化MzMessage中的parm

    /// <summary>
    /// 序列化object对象
    /// </summary>
    private static byte[] serializableObject(object obj)
    {
        using (MemoryStream mStream = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(mStream, obj);  //将对象序列化给流
            byte[] paramBuff = new byte[mStream.Length];
            Buffer.BlockCopy(mStream.GetBuffer(), 0, paramBuff, 0, (int)mStream.Length);

            return paramBuff;
        }
    }

    /// <summary>
    /// 反序列化object
    /// </summary>
    private static object deserializableObject(byte[] objBuffer)
    {
        using (MemoryStream mStream = new MemoryStream(objBuffer))
        {
            BinaryFormatter bf = new BinaryFormatter();
            return bf.Deserialize(mStream);
        }
    }

    /// <summary>
    /// C#语言自带的序列化方法,效率低
    /// 暂只用于传递param
    /// </summary>
    public static byte[] ObjectToBuffer(object obj)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, obj);  //将对象序列化给流
            byte[] paramBuffer = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, paramBuffer, 0, (int)ms.Length);

            return paramBuffer;
        }
    }

    /// <summary>
    /// 反序列化object
    /// </summary>
    public static object BufferToObject(byte[] objBuffer)
    {
        using (MemoryStream ms = new MemoryStream(objBuffer))
        {
            BinaryFormatter bf = new BinaryFormatter();
            return bf.Deserialize(ms);
        }
    }
    #endregion
}
