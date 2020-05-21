using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public struct HeroDeckConfig
{
    public int Hero_ID { get; set; }
    public string Hero_Name { get; set; }
    public int Red { get; set; }
    public int Blue { get; set; }
    public int Green { get; set; }
    public int Purple { get; set; }

    public override string ToString()
    {
        return string.Format("HeroID:{0} | Hero_Name:{1} | Red:{2} | Blue:{3} | Green:{4} | Purple:{5}", Hero_ID, Hero_Name, Red, Blue, Green, Purple);
    }
}

public static class PraseHandler
{
    /// <summary>
    /// 解析怪物配置文件
    /// </summary>
    public static MonsterData[] PraseMonsterConfig()
    {
        string str = Resources.Load<TextAsset>("SimulatorJson/Enemies").text;
        JsonData jsData = JsonMapper.ToObject(str);

        MonsterData[] temp = new MonsterData[jsData.Count];
        for (int i = 0; i < jsData.Count; i++)

        {
            temp[i] = JsonMapper.ToObject<MonsterData>(jsData[i].ToJson());
        }
        Debug.Log("##解析敌人json文件,共【" + jsData.Count + "】种怪物");

        return temp;
    }

    /// <summary>
    /// 解析英雄配置
    /// </summary>
    public static HeroDeckConfig[] PraseHeroConfig()
    {
        string str = Resources.Load<TextAsset>("SimulatorJson/PlayerDeck").text;
        JsonData jsData = JsonMapper.ToObject(str);

        HeroDeckConfig[] temp = new HeroDeckConfig[jsData.Count];
        for (int i = 0; i < jsData.Count; i++)
        {
            temp[i] = JsonMapper.ToObject<HeroDeckConfig>(jsData[i].ToJson());
        }
        Debug.Log("##解析英雄配置文件数:" + jsData.Count);

        return temp;
    }

}
