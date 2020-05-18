using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

/// <summary>
/// 登陆器
/// </summary>
public class UICreatePlayerPanel : UIPanelBase
{
    private TMP_InputField nickNameInput;
    private Text message;
    private Button accept_Btn;



    public override void Init(params object[] args)
    {
        base.Init(args);

        skinPath = "CreatePlayerPanel";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();

        Transform skinTrans = skin.transform;

        //Bind
        nickNameInput = skinTrans.Find("NickNameInput").GetComponent<TMP_InputField>();
        message = skinTrans.Find("Message").GetComponent<Text>();
        accept_Btn = skinTrans.Find("acceptBtn").GetComponent<Button>();

        //AddListener
        accept_Btn.onClick.AddListener(onAcceptClick);

    }

    private void onAcceptClick()
    {
        UIPanelMgr._Instance.OpenPanel<UILoadingPanel>("", "Try connect the game server");

        //前端检测
        if (nickNameInput.text == "")
        {
            UIPanelMgr._Instance.OpenPanel<UIPromptTips>("", "inputlabel is temp!");
            return;
        }

        //连接检测
        if (ConnMgr.servConn.status != Connection.Status.Conneted)
        {
            ConnMgr.servConn.Connect();
        }

        ConnMgr.servConn.Send(ConnMgr.CreateData(nickNameInput.text), onAcceptClickCallbabck);

    }

    private void onAcceptClickCallbabck(Message msg)
    {
        PlayerDataProtocol pdp = msg.Param as PlayerDataProtocol;
        //int result = (int)msg.Param;

        if (pdp != null)
        {
            //UIPanelMgr._Instance.CloseAllPanel();
            UIPanelMgr._Instance.OpenPanel<UITitlePanel>("", pdp);
        }
        else
            UIPanelMgr._Instance.OpenPanel<UIPromptTips>("", "Create player erro");

        UIPanelMgr._Instance.ClosePanel("UILoadingPanel");
    }
}
