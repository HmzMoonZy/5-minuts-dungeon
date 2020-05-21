/// <summary>
/// 最终传输和解析的类
/// </summary>
public class Message
{
    /// <summary>
    /// 每种消息的唯一标识
    /// </summary>
    public string Token { get { return OpCode.ToString() + SubCode; } }

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

    public Message() { }

    public Message(int opCode, int subCode, object param)
    {
        this.OpCode = opCode;
        this.SubCode = subCode;
        this.Param = param;
    }

}
public struct OpCode
{
    public const int SYSTEM = 0;
    public const int ACCOUNT = 1;
    public const int GAME = 2;
}

public struct SystemSubCode
{
    public const int HERTBEAT = 0;
    public const int BROADCAST = 1;
}

public struct AccountSubCode
{
    public const int SignUp = 0;
    public const int LogIn = 1;
    public const int LogOut = 2;
}
public struct GameSubCode
{
    public const int RequestData = 0;
    public const int CreateData = 1;
    public const int GetAchieve = 2;
    public const int GetRoomList = 3;
    public const int CreateRoom = 4;
    public const int EnterRoom = 5;
    public const int LeaveRoom = 6;
    public const int GetRoomInfo = 7;
}