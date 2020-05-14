using UnityEngine;
using TMPro;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using System;
using System.Text;
using System.Linq;
using System.Collections;

public class NetDemoController : MonoBehaviour
{
    //Component
    [Header("服务器连接面板")]
    public TMP_InputField ipaddress;
    public TMP_InputField port;
    public Button connectBtn;

    [Header("发送信息面板")]
    public TMP_InputField inputMessage;
    public Button sendBtn;
    private string logInfo;
    private bool newMessage = false;

    [Header("消息打印面板")]
    public Text infoText;

    [Header("服务器交互面板")]
    public GameObject demoPanel;            //连接成功后才显示
    public TMP_InputField username;
    public TMP_InputField password;
    public Button signup;
    public Button login;
    public Button logout;
    public Text playerName;
    public Text playerCoin;
    public TMP_InputField changeuser;
    public TMP_InputField changecoin;
    public Button accept;

    //Net
    private IPEndPoint serverPoint = null;
    private Socket socket;
    private const int BUFFER_SIZE = 1024;
    private byte[] readBuffer;
    private byte[] packetHead = new byte[4];
    private int bufferCount = 0;
    private PlayerDataProtocol pdp = new PlayerDataProtocol();

    private void Start()
    {
        demoPanel.SetActive(false);

        //Bind
        connectBtn.onClick.AddListener(connect);
        signup.onClick.AddListener(signUp);
        login.onClick.AddListener(logIn);
        logout.onClick.AddListener(logOut);

        accept.onClick.AddListener(() => {
            pdp.playerName = changeuser.text;
            pdp.coin = int.Parse(changecoin.text);
        });

        sendBtn.onClick.AddListener(delegate
        {
            send(Protocol.OpCode.SYSTEM, Protocol.SystemSubcode.BROADCAST, inputMessage.text);
            
        });
    }

    private void FixedUpdate()
    {
        //异步连接会为每个连接开辟新线程
        //只有主线程才能操作UGUI
        if (newMessage)
        {
            log(logInfo);
            newMessage = false;
        }

    }

    /// <summary>
    /// 连接输入框内输入的地址,并尝试接收消息
    /// </summary>
    private void connect()
    {
        try
        {
            serverPoint = new IPEndPoint(IPAddress.Parse(ipaddress.text), int.Parse(port.text));
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            readBuffer = new byte[BUFFER_SIZE];

            if (serverPoint != null)
            {
                socket.Connect(serverPoint);
                socket.BeginReceive(readBuffer, bufferCount, BUFFER_SIZE - bufferCount, SocketFlags.None, receiveCallbcak, null);
                log("连接成功!" + serverPoint.ToString());
                log("当前客户端地址:" + socket.LocalEndPoint);
                demoPanel.SetActive(true);
                StartCoroutine(AotoHertBeat());
            }
        }
        catch (Exception e)
        {
            log(e.Message);
            serverPoint = null;
            socket.Close();
            socket = null;
        }
    }

    private void signUp()
    {
        string encode = MzTransmitter.Encode(password.text);
        AccountInfo userInfo = new AccountInfo(username.text, encode);
        Message msg = new Message(Protocol.OpCode.ACCOUNT, Protocol.AccountSubcode.SignUp, userInfo);
        send(msg);
    }
    private void logIn()
    {
        string encode = MzTransmitter.Encode(password.text);
        AccountInfo userInfo = new AccountInfo(username.text, encode);
        Message msg = new Message(Protocol.OpCode.ACCOUNT, Protocol.AccountSubcode.LogIn, userInfo);
        send(msg);
    }
    private void logOut()
    {
        Message msg = new Message(Protocol.OpCode.ACCOUNT, Protocol.AccountSubcode.LogOut, pdp);
        send(msg);
        socket.Close();
    }
    /// <summary>
    /// 收到消息后的回调
    /// </summary>
    private void receiveCallbcak(IAsyncResult ar)
    {
        try
        {
            int count = socket.EndReceive(ar);        //接收到的数据大小
            Debug.Log("收到Packet! 包长" + count + "字节");

            if (count > 0)
            {
                bufferCount += count;   //更新尾索引
                processPacket();
            }
            socket.BeginReceive(readBuffer, bufferCount, BUFFER_SIZE - bufferCount, SocketFlags.None, receiveCallbcak, null);
        }
        catch (Exception e) { log(e.Message); }

    }

    private void processPacket()
    {
        int msgLenth;
        byte[] msgBuff = MzTransmitter.UnPacket(readBuffer, bufferCount, out msgLenth);

        if (msgBuff != null)
        {
            Message msg = MzTransmitter.DeserializableMessage(msgBuff);

            #region MsgCenter
            //系统操作
            if (msg.OpCode == Protocol.OpCode.SYSTEM)
            {
                if (msg.SubCode == Protocol.SystemSubcode.BROADCAST)
                {
                    logInfo = msg.Param as string;
                    newMessage = true;
                }
            }
            //用户操作
            else if (msg.OpCode == Protocol.OpCode.ACCOUNT)
            {
                if (msg.SubCode == Protocol.AccountSubcode.SignUp)
                {
                    Protocol.CheckState result = (Protocol.CheckState)msg.Param;
                    if (result == Protocol.CheckState.Success)
                    {
                        logInfo = "注册成功!";
                        newMessage = true;
                    }
                }
                else if (msg.SubCode == Protocol.AccountSubcode.LogIn)
                {
                    if (msg.Param != null)
                    {
                        pdp = msg.Param as PlayerDataProtocol;
                        Debug.Log(pdp.playerName);
                        Debug.Log(pdp.coin);
                        logInfo += "playerName : " + pdp.playerName + "\n" + "playerCoin : " + pdp.coin;
                        newMessage = true;
                    }

                }
            }

            #endregion


            //清楚解析完毕的信息缓存,将剩余消息复制到缓冲区首.
            int count = bufferCount - sizeof(int) - msgLenth;
            Array.Copy(readBuffer, sizeof(int) + msgLenth, readBuffer, 0, count);
            bufferCount = count;

            if (bufferCount > 0)
                processPacket();
        }
        /**
        if (bufferCount < sizeof(int))   //4
            return;

        //拆包
        //Array.Copy(readBuffer, packetHead, 4);
        Array.Copy(receiveCache, packetHead, sizeof(int));

        //解析包头 -> 获取tcp包长度
        msgLenth = BitConverter.ToInt32(packetHead, 0);
        Debug.Log("包头" + msgLenth);
        if (bufferCount < sizeof(int) + msgLenth)
            return; //被分包了

        //TODO 处理消息(data 交由其它对象处理)
        string data = Encoding.UTF8.GetString(receiveCache, sizeof(int), msgLenth);
        Debug.Log(data);
        //清楚解析完毕的信息缓存,将剩余消息复制到缓冲区首.
        int count = bufferCount - sizeof(int) - msgLenth;
        Array.Copy(receiveCache, sizeof(int) + msgLenth, receiveCache, 0, count);
        bufferCount = count;

        logInfo = data;
        newMessage = true;
        if (bufferCount > 0)
            ProcessPacket();
    */
    }
    private void log(string log)
    {
        infoText.text += log + "\n";
    }

    private IEnumerator AotoHertBeat()
    {
        while (true)
        {
            send(Protocol.OpCode.SYSTEM, Protocol.SystemSubcode.HERTBEAT, null);
            yield return new WaitForSeconds(2f);
        }
    }
    #region 封装及Sendmsg
    private void send(Message msg)
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

    private void send(int opCode, int subCode, object parm)
    {
        Message msg = new Message(opCode, subCode, parm);
        send(msg);
    }



    private static byte[] toPacket(string msg)
    {
        byte[] msgBuff = Encoding.UTF8.GetBytes(msg);
        return toPacket(msgBuff);
    }

    private static byte[] toPacket(byte[] msgBuff)
    {
        byte[] headBuff = BitConverter.GetBytes(msgBuff.Length);
        byte[] packet = headBuff.Concat(msgBuff).ToArray();
        return packet;
    }
    #endregion

}
