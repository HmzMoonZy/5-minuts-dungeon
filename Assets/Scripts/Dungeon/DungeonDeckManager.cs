using System.Collections.Generic;
using LitJson;

public enum DungeonType
{
    MONSTER,
    FATE,
    BOSS
}


/// <summary>
/// 地城卡组管理器
/// </summary>
public static class DungeonDeckManager
{
    private static Queue<MonsterCard> dungeonDeck = CreateDungeonCards(20);     //地城卡组
    private static Queue<MonsterCard> disCardDeck = new Queue<MonsterCard>();   //弃牌堆
    private static int remaining;                                               //当前卡组剩余数


    //PROP
    public static Queue<MonsterCard> DungeonDeck { get { return dungeonDeck; } }
    public static Queue<MonsterCard> DisCardDeck { get { return disCardDeck; } }
    public static int Remaining { get { return remaining; } set { remaining = value; } }

    /// <summary>
    /// 生成地城卡组
    /// </summary>
    /// <param name="number">生成数量</param>
    private static Queue<MonsterCard> CreateDungeonCards(int number)
    {
        Queue<MonsterCard> deck = new Queue<MonsterCard>();

        //获取模拟服务器返回的json字符串
        string json = ServerSimulator.GetDungeonCofig(number);
        JsonData jsData = JsonMapper.ToObject(json);
        for (int i = 0; i < jsData.Count; i++)
        {
            MonsterCard mc = JsonMapper.ToObject<MonsterCard>(jsData[i].ToString());
            deck.Enqueue(mc);
        }
        remaining = deck.Count;

        return deck;
    }

    public static void DisCard(MonsterCard card)
    {
        disCardDeck.Enqueue(card);
        card.gameObject.SetActive(false);
    }
}
