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
    private Button show_Btn;

    PlayerDataProtocol pdp = null;
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
        show_Btn = skinTrans.Find("ShowBtn").GetComponent<Button>();
        nickName = skinTrans.Find("PlayerInfo/nickname").GetComponent<TextMeshProUGUI>();
        coinNum = skinTrans.Find("PlayerInfo/coin").GetComponent<TextMeshProUGUI>();

        //AddListener
        findServ_Btn.onClick.AddListener(()=>
        {
            Debug.Log("Game Start!"); 
        });

        show_Btn.onClick.AddListener(()=>
        {
            UIPanelMgr._Instance.OpenPanel<UITestPanel>("");
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
