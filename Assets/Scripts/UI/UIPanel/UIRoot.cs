using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot : MonoBehaviour
{
    void Start()
    {
        //UIPanelMgr._Instance.OpenPanel<UILoadingPanle>("", "Try");

        UIPanelMgr._Instance.OpenPanel<UILauncherPanel>("");
    }

}
