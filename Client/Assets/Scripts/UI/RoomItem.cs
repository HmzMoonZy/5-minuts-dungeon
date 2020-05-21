using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Button enterRoom;

    private int roomid;
    private TextMeshProUGUI roomName;
    private TextMeshProUGUI menber;
    private TextMeshProUGUI status;

    private void Awake()
    {
        enterRoom = transform.GetComponent<Button>();

        roomName = transform.Find("RoomNamePanel/RoomName").GetComponent<TextMeshProUGUI>();
        menber = transform.Find("Panel/Menber").GetComponent<TextMeshProUGUI>();
        status = transform.Find("Panel/Status").GetComponent<TextMeshProUGUI>();

        enterRoom.onClick.AddListener(delegate
        {
            UIPanelMgr._Instance.OpenPanel<UILoadingPanel>("","Wait Please..");
            ConnMgr.servConn.Send(ConnMgr.EnterRoom(roomid), processEnterRoom);
        });
    }

    public void Init(int roomid, string roomName, int menber, int status)
    {
        this.roomid = roomid;
        this.roomName.text = roomName;
        this.menber.text = "Menber:" + menber + "/2";
        if (status == 0)
            this.status.text = "Wait";
        else this.status.text = "Gaming";
    }

    private void processEnterRoom(Message msg)
    {
        UIPanelMgr._Instance.ClosePanel("UILoadingPanel");

        if (msg.Param == null)
        {
            UIPanelMgr._Instance.OpenPanel<UIPromptTips>("", "Enter room Err!");
            return;
        }

        UIPanelMgr._Instance.OpenPanel<UIRoomPanel>();

    }


}
