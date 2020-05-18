using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelBase : MonoBehaviour
{
    public string skinPath;
    public GameObject skin;
    public PanelLayer layer;
    public object[] args;

    #region 生命周期
    
    /// <summary>
    /// 初始化面板.
    /// 通过 UIPanelMgr 打开一个UIPanel时首先触发这个方法.
    /// 通常在这个方法中设置 skin 及 layer 等基础信息.
    /// </summary>
    public virtual void Init(params object[] args)
    {
        this.args = args;
    }

    /// <summary>
    ///通过 UIPanelMgr 打开一个 UIPanel.
    ///在Panel生成前,触发这个方法.
    ///通常在这个方法中获取各个组件对象并绑定按钮监听.
    /// </summary>
    public virtual void OnShowing() { }

    /// <summary>
    ///通过 UIPanelMgr 打开一个UIPanel.
    ///Panel生成后,触发这个方法.
    ///通常在这个方法中实现一些动画效果.
    /// </summary>
    public virtual void OnShowed() { }

    /// <summary>
    ///UIPanel生成后每帧调用这个方法
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    ///UIPanel生成后,以固定帧数调用这个方法
    /// </summary>
    public virtual void LateUpdate() { }

    /// <summary>
    ///通过 UIPanelMgr 关闭一个UIPanel.
    ///Panel关闭前,触发这个方法.
    /// </summary>
    public virtual void OnClosing() { }

    /// <summary>
    ///通过 UIPanelMgr 关闭一个UIPanel.
    ///Panel关闭后,触发这个方法.
    /// </summary>
    public virtual void OnClosed() { }

    #endregion

    protected virtual void Close()
    {
        string name = this.GetType().ToString();
        UIPanelMgr._Instance.ClosePanel(name);
    }
}
