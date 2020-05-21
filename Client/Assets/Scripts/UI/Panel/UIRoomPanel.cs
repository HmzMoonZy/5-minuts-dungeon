using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomPanel : UIPanelBase
{
    private Transform userInfo;
    private Transform opPanel;
    private Transform Hero1;
    private Transform Hero2;

    private Button leaveBtn;
    private Button startBtn;

    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "RoomPanel";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        Transform skinTrans = skin.transform;

        userInfo = skinTrans.Find("userInfo");
        opPanel = skinTrans.Find("OpPanel");
        Hero1 = skinTrans.Find("Hero1");
        Hero2 = skinTrans.Find("Hero2");

        leaveBtn = userInfo.Find("LogoutBtn").GetComponent<Button>();
        startBtn = opPanel.Find("StartBtn").GetComponent<Button>();

        //TODO AddListener
        ConnMgr.servConn.msgDist.AddListener(ConnMgr.GetRoomInfo().Token, getRoomInfoCallBack);
        ConnMgr.servConn.Send(ConnMgr.GetRoomInfo());
        leaveBtn.onClick.AddListener(()=>
        {
            UIPanelMgr._Instance.OpenPanel<UILoadingPanel>("", "Try connect the game server");
            ConnMgr.servConn.Send(ConnMgr.GetLeaveRoom(), leaveRoomCallBack);
        });
        //startBtn.onClick.AddListener();

    }

    public override void OnClosing()
    {
        base.OnClosing();
        ConnMgr.servConn.msgDist.DelListenner(ConnMgr.GetRoomInfo().Token, getRoomInfoCallBack);
    }

    private void leaveRoomCallBack(Message msg)
    {
        
    }

    /// <summary>
    /// 获取房间信息回调
    /// </summary>
    /// <param name="msg"></param>
    private void getRoomInfoCallBack(Message msg)
    {
        List<PlayerDataProtocol> roomInfo = new List<PlayerDataProtocol>();
        roomInfo = msg.Param as List<PlayerDataProtocol>;
        
        if (roomInfo != null)
        {
            if (roomInfo.Count == 1)
            {
                Hero1.Find("Info").GetComponent<TextMeshProUGUI>().text = roomInfo[0].nickName;
                Hero2.Find("Info").GetComponent<TextMeshProUGUI>().text = "Wait Player...";
                Debug.Log(roomInfo[0].nickName);
            }
            if (roomInfo.Count == 2)
            {
                Hero1.Find("Info").GetComponent<TextMeshProUGUI>().text = roomInfo[0].nickName;
                Hero2.Find("Info").GetComponent<TextMeshProUGUI>().text = roomInfo[1].nickName;
                Debug.Log(roomInfo[1].nickName);
            }
        }
        
    }
}
