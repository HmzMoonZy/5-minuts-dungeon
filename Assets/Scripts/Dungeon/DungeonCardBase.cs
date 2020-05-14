using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DungeonCardType
{
    NULL = -1,
    BOSS,
    FATE,
    MONSTER
}

/// <summary>
/// 地城卡牌基类 子类为Boss Fate Monster
/// </summary>
public class DungeonCardBase :MonoBehaviour
{
    //public DungeonCardType DungeonCard_Type { get; set; }
    public int DungeonCard_Id { get; set; }
    public string DungeonCard_Name { get; set; }


    protected void FixedUI()
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();

        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
    }

}
