using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    //TODO 实现序列化委托
    //public delegate byte[] EncodeValueDelegate(object value);
    //public delegate object DecodeValueDelegate(byte[] buffer);

    public class Message
    {
        /// <summary>
        /// 操作码
        /// </summary>
        public int OpCode { get; set; }

        /// <summary>
        /// 子操作
        /// </summary>
        public int SubCode { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public object Param { get; set; }

        /// <summary>
        /// 序列化参数的方法
        /// 默认为 MzTransmitter.ObjectToBuffer;
        /// 除非服务端与客户端处理语言均使用C#自带的序列化方法,否则必须覆盖这个委托
        /// </summary>
        //public EncodeValueDelegate EncodeValue = MzTransmitter.ObjectToBuffer;

        /// <summary>
        /// 反序列化参数的方法
        /// 默认为 MzTransmitter.BufferToObject;
        /// 除非服务端与客户端处理语言均使用C#自带的反序列化方法,否则必须覆盖这个委托
        /// </summary>
        //public DecodeValueDelegate DecodeValue = MzTransmitter.BufferToObject;

        public Message() { }

        public Message(int opCode, int subCode, object param)
        {
            this.OpCode = opCode;
            this.SubCode = subCode;
            this.Param = param;
        }
    }
