using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

/// <summary>
/// 登陆器
/// </summary>
public class UICreateRoomPanel : UIPanelBase
{
    private TMP_InputField roomNameInput;
    private Button accept_Btn;



    public override void Init(params object[] args)
    {
        base.Init(args);

        skinPath = "CreateRoomPanel";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();

        Transform skinTrans = skin.transform;

        //Bind
        roomNameInput = skinTrans.Find("RoomNameInput").GetComponent<TMP_InputField>();
        accept_Btn = skinTrans.Find("acceptBtn").GetComponent<Button>();

        //AddListener
        accept_Btn.onClick.AddListener(onAcceptClick);

    }

    private void onAcceptClick()
    {
        UIPanelMgr._Instance.OpenPanel<UILoadingPanel>("", "Try connect the game server");

        //前端检测
        if (roomNameInput.text == "")
        {
            UIPanelMgr._Instance.OpenPanel<UIPromptTips>("", "inputlabel is temp!");
            return;
        }

        //连接检测
        if (ConnMgr.servConn.status != Connection.Status.Conneted)
        {
            ConnMgr.servConn.Connect();
        }

        ConnMgr.servConn.Send(ConnMgr.CreateRoom(roomNameInput.text), acceptClickCallbabck);

    }

    private void acceptClickCallbabck(Message msg)
    {  
        int result = (int)msg.Param;

        UIPanelMgr._Instance.ClosePanel("UILoadingPanel");

        if (result != 1)
        {
            UIPanelMgr._Instance.ClosePanel("UICreateRoomPanel");
            UIPanelMgr._Instance.OpenPanel<UIPromptTips>("", "Create room Erro!");
            UIPanelMgr._Instance.OpenPanel<UITitlePanel>("");
            return;
        }
        UIPanelMgr._Instance.OpenPanel<UIRoomPanel>("");

    }
}
