using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UILoadingPanel : UIPanelBase
{
    //GameObject
    private GameObject loadingToken;
    private TextMeshProUGUI messageText;

    //RotateSpeed
    public float rotateSpeed = 1.5f;

    //LiveTime
    public float liveTime = 10;
    private float totalTime;

    //timeOutCallBack
    public Action timeOutCallBack;

    //message
    private string message;

    /// <summary>
    /// 初始化 Loading 面板
    /// </summary>
    /// <param name="args">0: 显示的信息 1: timeOutCallBack</param>
    public override void Init(params object[] args)
    {
        base.Init(args);

        skinPath = "LoadingPanel";
        layer = PanelLayer.Mask;
    }


    public override void OnShowing()
    {
        base.OnShowing();

        Transform skinTrans = skin.transform;

        for (int i = 0; i < args.Length; i++)
        {
            if (i == 0)
            {
                message = args[0] as string;
            }

            else if (i == 1)
            {
                timeOutCallBack = args[1] as Action;
            }
        }

        if (timeOutCallBack == null)
            timeOutCallBack = () => { UIPanelMgr._Instance.OpenPanel<UIPromptTips>("", "Time Out!"); };


        loadingToken = skinTrans.Find("LoadingIcon").gameObject;
        messageText = skinTrans.Find("Message").GetComponent<TextMeshProUGUI>();
    }

    public override void OnShowed()
    {
        base.OnShowed();
        messageText.text = message;
    }

    public override void Update()
    {
        totalTime += Time.deltaTime;

        if (totalTime >= liveTime)
        {
            timeOutCallBack?.Invoke();
            Close();
        }
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        if (loadingToken != null)
        {
            loadingToken.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, -rotateSpeed));
        }
    }
}
