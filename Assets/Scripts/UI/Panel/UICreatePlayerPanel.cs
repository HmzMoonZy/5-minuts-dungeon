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

        nickNameInput = skinTrans.Find("NickNameInput").GetComponent<TMP_InputField>();
        message = skinTrans.Find("Message").GetComponent<Text>();
        accept_Btn = skinTrans.Find("acceptBtn").GetComponent<Button>();

        //Bind
        accept_Btn.onClick.AddListener(onAcceptClick);

    }

    //TODO
    private void onAcceptClick()
    {
        //前端检测
        if (nickNameInput.text == "")
        {
            //TODO Show Tips
            message.text = "昵称不能为空";
            return;
        }

        //连接检测
        if (ConnMgr.servConn.status != Connection.Status.Conneted)
        {
            UIPanelMgr._Instance.OpenPanel<UILoadingPanel>("", "Try connect the game server");
            ConnMgr.servConn.Connect(delegate { UIPanelMgr._Instance.ClosePanel("UILoadingPanle"); });
        }

        //TODO 发送创建角色请求
        UIPanelMgr._Instance.OpenPanel<UILoadingPanel>("", "Try connect the game server");
    }

    private void onAcceptClickCallbabck(Message msg)
    {
        int result = (int)msg.Param;

        UIPanelMgr._Instance.ClosePanel("UILoadingPanle");
    }
}
