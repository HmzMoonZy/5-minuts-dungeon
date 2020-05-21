using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public static class ServerSimulator
{
    /// <summary>
    /// 获取卡组json配置文件
    /// </summary>
    /// <param name="number">初始化的卡组数量</param>
    public static string GetDungeonCofig(int number)
    {
        JsonData jsData = new JsonData();
        MonsterData[] monsters = PraseHandler.PraseMonsterConfig();        //单张怪物牌数组

        Debug.Log("##开始生成地城卡组");
        //生成牌组
        for (int i = 0; i < number; i++)
        {
            int random = Random.Range(0, monsters.Length);
            jsData.Add(JsonMapper.ToJson(monsters[random]));
        }

        return jsData.ToJson();
    }

    public static int[] GetHeroDeck(int heroID)
    {
        HeroDeckConfig hero = PraseHandler.PraseHeroConfig()[heroID - 1];   //HeroID的配置文件
        Debug.Log(hero);
        int[] heroDeck = new int[40];       //英雄卡组
        int index = 0;
        for (int i = 0; i < hero.Red; i++)
        {
            heroDeck[index] = 0;
            index++;
        }

        for (int i = 0; i < hero.Blue; i++)
        {
            heroDeck[index] = 1;
            index++;
        }

        for (int i = 0; i < hero.Green; i++)
        {
            heroDeck[index] = 2;
            index++;
        }

        for (int i = 0; i < hero.Purple; i++)
        {
            heroDeck[index] = 3;
            index++;
        }

        return Shuffle_Fisher_Yates(heroDeck);
    }

    public static T[] Shuffle_Fisher_Yates<T>(T[] deck)
    {
        for (int i = deck.Length - 1; i > 0; i--)
        {
            int random = Random.Range(1, i);
            T temp = deck[i];
            deck[i] = deck[random];
            deck[random] = temp;
        }
        return deck;
    }
}
