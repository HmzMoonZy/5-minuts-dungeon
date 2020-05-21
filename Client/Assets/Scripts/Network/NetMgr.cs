using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMgr : MonoBehaviour
{
    public static NetMgr _Instance;

    private void Awake()
    {
        _Instance = this; 
    }

    void Update()
    {
        ConnMgr.Update();
    }
}
