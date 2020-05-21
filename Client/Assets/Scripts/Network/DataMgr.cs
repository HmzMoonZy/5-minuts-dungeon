using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr : MonoBehaviour
{
    public static DataMgr _Instance;

    private DataMgr() { }

    private void Awake()
    {
        _Instance = this;
    }
}
