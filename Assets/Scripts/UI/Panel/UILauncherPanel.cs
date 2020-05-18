using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

/// <summary>
/// 登陆器
/// </summary>
public class UILauncherPanel : UIPanelBase
{
    private TMP_InputField usernameInput;
    private TMP_InputField passwordInput;

    private Button login_Btn;
    private Button signup_Btn;
    private Button news_Btn;


    public override void Init(params object[] args)
    {
        base.Init(args);

        skinPath = "LauncherPanel";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();

        Transform skinTrans = skin.transform;

        usernameInput = skinTrans.Find("UserPanel/Username/InputField").GetComponent<TMP_InputField>();
        passwordInput = skinTrans.Find("UserPanel/Password/InputField").GetComponent<TMP_InputField>();
        login_Btn = skinTrans.Find("UserPanel/StartBtn").GetComponent<Button>();
        signup_Btn = skinTrans.Find("UserPanel/SignupBtn").GetComponent<Button>();
        news_Btn = skinTrans.Find("UserPanel/NewsBtn").GetComponent<Button>();

        //Bind
        login_Btn.onClick.AddListener(onLoginClick);

        signup_Btn.onClick.AddListener(delegate { UIPanelMgr._Instance.OpenPanel<UIRegisterPanel>(""); });

        news_Btn.onClick.AddListener(onNewsClick);
    }


    private void onLoginClick()
    {
        UIPanelMgr._Instance.OpenPanel<UILoadingPanel>("", "Try connect the game server");

        //前端检测
        if (usernameInput.text == "" || passwordInput.text == "")
        {
            UIPanelMgr._Instance.OpenPanel<UIPromptTips>("", "userName or paasword is temp !");
            UIPanelMgr._Instance.ClosePanel("UILoadingPanel");
            return;
        }

        //连接检测
        if (ConnMgr.servConn.status != Connection.Status.Conneted)
        {
            ConnMgr.servConn.Connect();
        }

        //发送注册请求并设置回调函数
        AccountInfo ai = new AccountInfo(usernameInput.text, MzTransmitter.Encode(passwordInput.text));
        ConnMgr.servConn.Send(ConnMgr.Login(ai), onLoginCallBack);
    }

    private void onLoginCallBack(Message msg)
    {
        int result = (int)msg.Param;

        if (result == 1)
        {
            Debug.Log("登录成功!");

            ConnMgr.servConn.Send(ConnMgr.RequestData(), requestDataCallBack);

        }
        else if (result == -1)
        {
            UIPanelMgr._Instance.OpenPanel<UIPromptTips>("", "the username or password is erro!");
        }

        UIPanelMgr._Instance.ClosePanel("UILoadingPanel");
    }

    private void requestDataCallBack(Message msg)
    {
        PlayerDataProtocol pdp = msg.Param as PlayerDataProtocol;

        //第一次进入游戏, 创建人物
        if (pdp == null)
            UIPanelMgr._Instance.OpenPanel<UICreatePlayerPanel>("");

        //进入游戏
        else
        {
            //UIPanelMgr._Instance.CloseAllPanel();
            //UIPanelMgr._Instance.ClosePanel(
            UIPanelMgr._Instance.OpenPanel<UITitlePanel>("", pdp);
        }
    }

    //TODO
    private void onNewsClick()
    { }
}
