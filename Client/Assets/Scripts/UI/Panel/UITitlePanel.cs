using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UITitlePanel : UIPanelBase
{
    //playerData
    private TextMeshProUGUI nickName;
    private TextMeshProUGUI coinNum;

    private Button findServ_Btn;
    private Button createRoom;
    private Button logoutBtn;

    private PlayerDataProtocol pdp = null;
    #region UIPanelBase 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);

        skinPath = "TitilePanel";
        layer = PanelLayer.Panel;

        pdp = args[0] as PlayerDataProtocol;
    }

    public override void OnShowing()
    {
        base.OnShowing();

        Transform skinTrans = skin.transform;

        //Bind
        findServ_Btn = skinTrans.Find("FindServerBtn").GetComponent<Button>();
        createRoom = skinTrans.Find("CreateRoomBtn").GetComponent<Button>();
        logoutBtn = skinTrans.Find("LogoutBtn").GetComponent<Button>();
        nickName = skinTrans.Find("PlayerInfo/nickname").GetComponent<TextMeshProUGUI>();
        coinNum = skinTrans.Find("PlayerInfo/coin").GetComponent<TextMeshProUGUI>();

        //AddListener
        findServ_Btn.onClick.AddListener(()=>
        {
            UIPanelMgr._Instance.OpenPanel<UIRoomListPanel>("");
        });

        createRoom.onClick.AddListener(()=>
        {
            UIPanelMgr._Instance.OpenPanel<UICreateRoomPanel>("");
        });

        logoutBtn.onClick.AddListener(()=>
        {
            UIPanelMgr._Instance.OpenPanel<UICreateRoomPanel>("");
        });
    }

    public override void OnShowed()
    {
        base.OnShowed();

        nickName.text = pdp.nickName;
        coinNum.text = "coin : " + pdp.coin;
    }

    #endregion
}
