using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITestPanel : UIPanelBase
{
    private Button close_Btn;


    public override void Init(params object[] args)
    {
        base.Init(args);

        skinPath = "TestPanel";
        layer = PanelLayer.Panel;
    }

    public override void OnShowing()
    {
        base.OnShowing();

        Transform skinTrans = skin.transform;
        close_Btn = skinTrans.Find("CloseBtn").GetComponent<Button>();
        close_Btn.onClick.AddListener(Close);
    }
}
