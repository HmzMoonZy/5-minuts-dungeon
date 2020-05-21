using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomListPanel : UIPanelBase
{
    private Transform grid;
    private GameObject roomItem;

    private Button logout_btn;
    private Button refresh_btn;

    //private TextMeshProUGUI win;
    //private TextMeshProUGUI lose;

    public override void Init(params object[] args)
    {
        base.Init(args);

        skinPath = "RoomListPanel";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();

        //TODO AssetBundle
        roomItem = Resources.Load<GameObject>("RoomItem");

        Transform skinTrans = skin.transform;
        grid = skinTrans.Find("LeftPanel/RoomList/ScrollRect/Grid");
        logout_btn = skinTrans.Find("userInfo/LogoutBtn").GetComponent<Button>();
        refresh_btn = skinTrans.Find("LeftPanel/RefreshBtn").GetComponent<Button>();


        //UIPanelMgr._Instance.OpenPanel<UILoadingPanel>();
        refresh_btn.onClick.AddListener(delegate { ConnMgr.servConn.Send(ConnMgr.GetRoomList()); });

        //开启监听
        ConnMgr.servConn.msgDist.AddListener(ConnMgr.GetRoomList().Token, processRoomList);
        ConnMgr.servConn.Send(ConnMgr.GetRoomList());
    }

    public override void OnClosing()
    {
        base.OnClosing();
        ConnMgr.servConn.msgDist.DelListenner(ConnMgr.GetRoomList().Token, processRoomList);
    }

    private void processRoomList(Message msg)
    {
        ClearRoomList();

        if (msg.Param == null)
        {
            UIPanelMgr._Instance.OpenPanel<UIPromptTips>("","No room now!");
            return;
        }

        RoomListProtocol rlp = msg.Param as RoomListProtocol;
        List<RoomProtocal> list = rlp.list;


        for (int i = 0; i < list.Count; i++)
        {
            GameObject item = Instantiate(roomItem, grid, false);
            RoomItem ri = item.AddComponent<RoomItem>();
            ri.Init(list[i].roomid,list[i].roomName, list[i].current, list[i].status);

        }
    }

    private void ClearRoomList()
    {
        RoomItem[] items = grid.GetComponentsInChildren<RoomItem>();
        for (int i = 0; i < items.Length; i++)
        {
            Destroy(items[i].gameObject);
        }
    }
}
