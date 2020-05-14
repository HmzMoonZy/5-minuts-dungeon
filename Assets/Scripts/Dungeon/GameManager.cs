using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    PLAYING,
    END,
    OVER
}

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance;

    private Transform dungeonCardPos;
    private Transform playerHandCardPos;

    private MonsterCard currDungeon;
    private MonsterCard prevDungeon;
    private Queue<PlayerCardData> playerdeck;

    private HandCard selectCard;

    public int HeroID;
    public int handCardCount;
    public GameState gameState;

    public int playerdeckRemaining { get; private set; }

    private void Awake()
    {
        _Instance = this;
    }

    private void Start()
    {
        HeroID = 1;
        playerdeck = PlayerDeckManager.GetPlayerDeck(HeroID);

        currDungeon = null;
        prevDungeon = null;
        gameState = GameState.PLAYING;

        dungeonCardPos = GameObject.Find("Canvas/Panel/DungeonCardPanel").GetComponent<Transform>();
        playerHandCardPos = GameObject.Find("Canvas/Panel/PlayerPanel/HandCards").GetComponent<Transform>();

        for (int i = 0; i < 5; i++)
        {
            DrawCard();
        }

    }

    private void Update()
    {
        if (gameState == GameState.PLAYING && currDungeon == null)
        {
            ShowNextCard(DungeonDeckManager.DungeonDeck);
        }

        if (gameState == GameState.PLAYING && currDungeon.GetHelthPoint() <= 0)
        {
            DungeonDeckManager.DisCard(currDungeon);
            ShowNextCard(DungeonDeckManager.DungeonDeck);
        }

        if (gameState == GameState.PLAYING)
        {

        }


        //自动补牌
        if (handCardCount < 5 && playerdeck.Count > 0)
        {
            DrawCard();
        }
        //************ T e s t ************//

        if (Input.GetKeyDown(KeyCode.R))
        {
            OutCard(CardType.RED_SWORD);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            OutCard(CardType.BLUE_SHIELD);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            OutCard(CardType.GREEN_BOW);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            OutCard(CardType.PURPLE_MAGIC);
        }
    }

    private void DrawCard()
    {
        GameObject card = Instantiate(CardViews._Instance.HandCardModel, playerHandCardPos);
        HandCard handCard = card.GetComponent<HandCard>();

        if (playerdeck.Count >=0)
        {
            PlayerCardData data = playerdeck.Dequeue();
            handCard.Init(data.type);
            handCard.ShowCard();
            playerdeckRemaining = playerdeck.Count;
        }
        handCardCount += 1;


    }

    /// <summary>
    /// 展示下一张地城卡
    /// </summary>
    private void ShowNextCard(Queue<MonsterCard> deck)
    {
        if (DungeonDeckManager.Remaining > 0)
        {
            GameObject dungeonCard = Instantiate(CardViews._Instance.EnemyCardModel, dungeonCardPos);
            MonsterCard card = dungeonCard.GetComponent<MonsterCard>();

            MonsterCard nextCrad = deck.Dequeue();
            DungeonDeckManager.Remaining -= 1;

            card.DungeonCard_Id = nextCrad.DungeonCard_Id;
            card.DungeonCard_Name = nextCrad.DungeonCard_Name;
            card.Slot_Red = nextCrad.Slot_Red;
            card.Slot_Blue = nextCrad.Slot_Blue;
            card.Slot_Green = nextCrad.Slot_Green;
            card.Slot_Purple = nextCrad.Slot_Purple;
            card.InitSlots();

            if (currDungeon == null)
            {
                currDungeon = card;
            }

            else
            {
                prevDungeon = currDungeon;
                currDungeon = card;
            }
        }

        else
        {
            Debug.Log("地城卡组已空");
        }
    }

    public void SetSelectHandCard(HandCard handCard)
    {
        selectCard = handCard;
    }

    public void OutCard()
    {
        List<SlotController> temp = currDungeon.GetSlotControllers();
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i].type == selectCard.Type && !temp[i].isBorke)
            {
                switch (selectCard.Type)
                {
                    case CardType.RED_SWORD:
                        currDungeon.Slot_Red -= 1;
                        break;

                    case CardType.BLUE_SHIELD:
                        currDungeon.Slot_Red -= 1;
                        break;

                    case CardType.GREEN_BOW:
                        currDungeon.Slot_Red -= 1;
                        break;

                    case CardType.PURPLE_MAGIC:
                        currDungeon.Slot_Red -= 1;
                        break;
                }

                temp[i].Broke();
                break;
            }
        }

        Destroy(selectCard.gameObject);
        handCardCount -= 1;
    }

    public void OutCard(CardType type)
    {
        List<SlotController> temp = currDungeon.GetSlotControllers();
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i].type == type && !temp[i].isBorke)
            {
                switch (type)
                {
                    case CardType.RED_SWORD:
                        currDungeon.Slot_Red -= 1;
                        break;

                    case CardType.BLUE_SHIELD:
                        currDungeon.Slot_Red -= 1;
                        break;

                    case CardType.GREEN_BOW:
                        currDungeon.Slot_Red -= 1;
                        break;

                    case CardType.PURPLE_MAGIC:
                        currDungeon.Slot_Red -= 1;
                        break;
                }

                temp[i].Broke();
                break;
            }

        }

    }
}
