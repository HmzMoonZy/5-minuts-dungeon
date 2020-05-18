using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIPromptTips : UIPanelBase
{
    private TextMeshProUGUI tmp;
    private Button button;

    private string info;

    public override void Init(params object[] args)
    {
        base.Init(args);
        skinPath = "PromptTips";
        layer = PanelLayer.Tips;

        info = args[0] as string;
    }

    public override void OnShowing()
    {
        base.OnShowing();
        
        Transform skinTrans = skin.transform;

        tmp = skinTrans.Find("Text").GetComponent<TextMeshProUGUI>();
        button = skinTrans.Find("Button").GetComponent<Button>();

        button.onClick.AddListener(Close);
    }

    public override void OnShowed()
    {
        base.OnShowed();
        tmp.text = info;
    }

}
