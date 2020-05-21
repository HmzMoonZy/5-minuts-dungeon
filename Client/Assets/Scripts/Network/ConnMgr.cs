using System.Collections;
using System.Collections.Generic;

public static class ConnMgr
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
        return new Message(OpCode.SYSTEM, SystemSubCode.HERTBEAT, null);
    }

    /// <summary>
    /// 注册请求Msg
    /// </summary>
    public static Message Register(AccountInfoProtocol ai)
    {
        return new Message(OpCode.ACCOUNT, AccountSubCode.SignUp, ai);
    }

    /// <summary>
    /// 登录请求Msg
    /// </summary>
    public static Message Login(AccountInfoProtocol ai)
    {
        return new Message(OpCode.ACCOUNT, AccountSubCode.LogIn, ai);
    }

    /// <summary>
    /// 角色数据请求Msg
    /// </summary>
    public static Message RequestData()
    {
        return new Message(OpCode.GAME, GameSubCode.RequestData, null);
    }

    public static Message CreateData(string data)
    {
        return new Message(OpCode.GAME, GameSubCode.CreateData, data);
    }

    public static Message GetRoomList()
    {
        return new Message(OpCode.GAME, GameSubCode.GetRoomList, null);
    }

    public static Message CreateRoom(string roomName)
    {
        return new Message(OpCode.GAME, GameSubCode.CreateRoom, roomName);
    }

    public static Message EnterRoom(int roomId)
    {
        return new Message(OpCode.GAME, GameSubCode.EnterRoom, roomId);
    }

    public static Message GetRoomInfo()
    {
        return new Message(OpCode.GAME, GameSubCode.GetRoomInfo, null);
    }

    public static Message GetLeaveRoom()
    {
        return new Message(OpCode.GAME, GameSubCode.LeaveRoom, null);
    }
}
