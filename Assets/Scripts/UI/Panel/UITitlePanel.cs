using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UITitlePanel : UIPanelBase
{
    private Button findServ_Btn;
    private Button show_Btn;

    #region UIPanelBase 生命周期
    public override void Init(params object[] args)
    {
        base.Init(args);

        skinPath = "TitilePanel";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();

        Transform skinTrans = skin.transform;

        findServ_Btn = skinTrans.Find("FindServerBtn").GetComponent<Button>();
        show_Btn= skinTrans.Find("ShowBtn").GetComponent<Button>();

        //Bind
        findServ_Btn.onClick.AddListener(()=>
        {
            Debug.Log("Game Start!"); 
        });

        show_Btn.onClick.AddListener(()=>
        {
            UIPanelMgr._Instance.OpenPanel<UITestPanel>("");
        });

    }


    #endregion
}
