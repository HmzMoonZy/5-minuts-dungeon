using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerCardData
{
    public CardType type;
}

/// <summary>
/// 
/// </summary>
public class PlayerDeckManager
{
    private static Queue<PlayerCardData> playerDeck = new Queue<PlayerCardData>();
    private static int remaining;                                               //当前卡组剩余数

    public static Queue<PlayerCardData> PlayerDeck { get { return playerDeck; } }

    public static Queue<PlayerCardData> GetPlayerDeck(int heroID)
    {
        int[] config = ServerSimulator.GetHeroDeck(heroID);

        for (int i = 0; i < config.Length; i++)
        {
            PlayerCardData temp = new PlayerCardData();
            switch (config[i])
            {
                case 0:
                    temp.type = CardType.RED_SWORD;
                    playerDeck.Enqueue(temp);
                    break;

                case 1:
                    temp.type = CardType.BLUE_SHIELD;
                    playerDeck.Enqueue(temp);
                    break
                        ;
                case 2:
                    temp.type = CardType.GREEN_BOW;
                    playerDeck.Enqueue(temp);
                    break;

                case 3:
                    temp.type = CardType.PURPLE_MAGIC;
                    playerDeck.Enqueue(temp);
                    break;

                default:
                    Debug.LogError("The Config Erro!");
                    break;
            }

            Debug.Log(temp);
        }

        remaining = playerDeck.Count;

        return playerDeck;
    }
}
