using System.Collections;
using System.Collections.Generic;

public class ConnMgr
{
    public static Connection servConn = new Connection();    //连接游戏服务器的Conn
    //public static Connection alibabaConn = new Connection();    //其他平台的Conn

    public static void  Update()
    {
        servConn.Update();
        //alibabaConn.Update();
    }

    /// <summary>
    /// 心跳数据Msg.
    /// </summary>
    public static Message HertBeat()
    {
        Message hb = new Message(Protocol.OpCode.SYSTEM, Protocol.SystemSubCode.HERTBEAT, null);
        return hb;
    }

    /// <summary>
    /// 注册请求Msg
    /// </summary>
    public static Message Register(AccountInfo ai)
    {
        Message hb = new Message(Protocol.OpCode.ACCOUNT, Protocol.AccountSubCode.SignUp, ai);
        return hb;
    }

    /// <summary>
    /// 登录请求Msg
    /// </summary>
    public static Message Login(AccountInfo ai)
    {
        Message hb = new Message(Protocol.OpCode.ACCOUNT, Protocol.AccountSubCode.LogIn, ai);
        return hb;
    }

    /// <summary>
    /// 角色数据请求Msg
    /// </summary>
    public static Message RequestData()
    {
        return new Message(Protocol.OpCode.GAME, Protocol.GameSubCode.RequestData, null);
    }

    public static Message CreateData(string data)
    {
        return new Message(Protocol.OpCode.GAME, Protocol.GameSubCode.CreateData, data);
    }
}
