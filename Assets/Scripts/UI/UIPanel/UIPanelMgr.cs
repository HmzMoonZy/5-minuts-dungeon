using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelLayer { Panel, Tips, Mask }

/// <summary>
/// 面板管理器,负责:
///     层级管理;打开面板;关闭面板
/// </summary>
public class UIPanelMgr : MonoBehaviour
{
    public static UIPanelMgr _Instance;

    private GameObject canvas;

    public Dictionary<string, UIPanelBase> dict;            //开启的面板

    private Dictionary<PanelLayer, Transform> layerDict;    //层级字典: Mask > Tips > Panel



    void Awake()
    {
        _Instance = this;
        InitLayer();
        dict = new Dictionary<string, UIPanelBase>();
    }

    private void InitLayer()
    {
        canvas = FindObjectOfType<Canvas>().gameObject;
        if (canvas == null)
        {
            Debug.LogError("canvas is null!", gameObject);
        }

        layerDict = new Dictionary<PanelLayer, Transform>();

        foreach (PanelLayer pl in Enum.GetValues(typeof(PanelLayer)))
        {
            string name = pl.ToString();    //Panel Tips
            Transform trans = canvas.transform.Find(name);      //找到Canvas下所有节点
            layerDict.Add(pl, trans);
        }
    }

    public void OpenPanel<T>(string skinPath, params object[] args) where T : UIPanelBase
    {
        string name = typeof(T).ToString();
        Debug.Log("打开:" + name);
        //若该面板已经打开则return
        if (dict.ContainsKey(name))
            return;

        //注册dict, 表示正在打开
        UIPanelBase panel = canvas.AddComponent<T>();
        panel.Init(args);
        dict.Add(name, panel);

        //TODO 加载皮肤 P175
        skinPath = (skinPath != "" ? skinPath : panel.skinPath);
        GameObject skin = Resources.Load<GameObject>(skinPath);
        if (skin == null)
            Debug.LogError("skin is null, the path: " + skinPath);

        //生成面板
        panel.skin = Instantiate(skin);
        Transform skinTrans = panel.skin.transform;
        PanelLayer layer = panel.layer;
        Transform parent = layerDict[layer];
        skinTrans.SetParent(parent, false);

        //触发生命周期
        panel.OnShowing();
        panel.OnShowed();
    }

    public void ClosePanel(string name)
    {
        if (!dict.ContainsKey(name))
            return;

        UIPanelBase panel = (UIPanelBase)dict[name];

        panel.OnClosing();
        dict.Remove(name);
        panel.OnClosed();

        Destroy(panel.skin);
        Destroy(panel);
    }

    public void CloseAllPanel()
    {
        foreach (string key in dict.Keys)
        {

        }
    }

}
