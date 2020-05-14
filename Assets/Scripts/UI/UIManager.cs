using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager _Instance;

    private List<GameObject> awakeList;

    [Header("全局UI")]
    public GameObject LoadingWindow;
    public GameObject TimeOutWindow;


    public enum GlobalWindow
    {
        LoadingCall
    }


    private void Awake()
    {
        _Instance = this;

        //GolobalUI
        LoadingWindow = Resources.Load<GameObject>("GlobalUI/LoadingCall");

        awakeList = new List<GameObject>();

    }

    public void LoadingCall(string message, Action timeOut)
    {
        GameObject window = Instantiate(LoadingWindow, FindObjectOfType<Canvas>().transform);
        window.gameObject.SetActive(true);
        window.GetComponent<UILoadingCall>().Init(message, timeOut);
        awakeList.Add(window);
    }

    public void Clear()
    {
        for (int i = 0; i < awakeList.Count; i++)
        {
            Destroy(awakeList[i]);
        }
        awakeList.Clear();
    }


    //TODO
    //public void CallWindow(GlobalWindow window, Action callBack)
    //{
    //    GameObject panel = Instantiate<GameObject>(LoadingWindow, FindObjectOfType<Canvas>().transform);
    //    panel.SetActive(true);
    //}

    ////TODO 一个窗口UI的管理类
    //public void CallMask()
    //{
    //    LoadingWindow = Instantiate<GameObject>(mask, FindObjectOfType<Canvas>().transform).GetComponent<UILoadingCall>();
    //    LoadingWindow.gameObject.SetActive(true);
    //    LoadingWindow.SetMessage(null);
    //}
}
