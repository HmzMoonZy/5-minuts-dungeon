using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIRegisterPanel : UIPanelBase
{
    private TMP_InputField usernameInput;
    private TMP_InputField passwordInput;
    private TMP_InputField repeatPasswordInput;

    private Button accept_Btn;
    private Button back_Btn;

    public override void Init(params object[] args)
    {
        base.Init(args);

        skinPath = "RegisterPanel";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();

        Transform skinTrans = skin.transform;

        //Bind
        usernameInput = skinTrans.Find("Panel/Username/InputField").GetComponent<TMP_InputField>();
        passwordInput = skinTrans.Find("Panel/Password/InputField").GetComponent<TMP_InputField>();
        repeatPasswordInput = skinTrans.Find("Panel/Repeat/InputField").GetComponent<TMP_InputField>();
        accept_Btn = skinTrans.Find("Panel/Register_Btn").GetComponent<Button>();
        back_Btn = skinTrans.Find("Panel/Close_Btn").GetComponent<Button>();

        //AddListener
        accept_Btn.onClick.AddListener(onRegister);
        back_Btn.onClick.AddListener(Close);
    }

    private void onRegister()
    {
        //遮罩
        UIPanelMgr._Instance.OpenPanel<UILoadingPanel>("", "Try connect the game server");

        //前端检测
        if (usernameInput.text == "" || passwordInput.text ==""|| repeatPasswordInput.text == "")
        {
            UIPanelMgr._Instance.OpenPanel<UIPromptTips>("", "userName or paasword is null !");
            UIPanelMgr._Instance.ClosePanel("UILoadingPanel");
            return;
        }
        if (passwordInput.text != repeatPasswordInput.text)
        {
            UIPanelMgr._Instance.OpenPanel<UIPromptTips>("", "The passwords entered do not match!");
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
        ConnMgr.servConn.Send(ConnMgr.Register(ai), onRegisterCallBack);
    }

    private void onRegisterCallBack(Message msg)
    {
        int result = (int)msg.Param;

        if (result == 1)
        {
            Debug.Log("注册成功!");
            UIPanelMgr._Instance.ClosePanel(this.ToString());
        }
        else if (result == -1)
        {
            UIPanelMgr._Instance.OpenPanel<UIPromptTips>("", "Contains illegal characters!");
        }
        else if (result == 0)
        {
            UIPanelMgr._Instance.OpenPanel<UIPromptTips>("", "The user name has been registered");
        }
        else if (result == -2)
        {
            UIPanelMgr._Instance.OpenPanel<UIPromptTips>("", "Server Error!");
        }

        UIPanelMgr._Instance.ClosePanel("UILoadingPanel");
    }
}
