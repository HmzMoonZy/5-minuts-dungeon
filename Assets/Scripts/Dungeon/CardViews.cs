using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardViews : MonoBehaviour
{
    public static CardViews _Instance;
    public Transform canvas;
    //==============HandCards==============//
    private string path_handCards = "Textures/HandCard/";
    public GameObject HandCardModel { get; private set; }
    //背景
    public Sprite Bg_Blue { get; private set; }
    public Sprite Bg_Red { get; private set; }
    public Sprite Bg_Green { get; private set; }
    public Sprite Bg_Purple { get; private set; }
    //图标
    public Sprite Icon_Sword { get; private set; }
    public Sprite Icon_Shield { get; private set; }
    public Sprite Icon_Magic { get; private set; }
    public Sprite Icon_Bow { get; private set; }

    //==============DungeonCard==============//
    public GameObject EnemyCardModel { get; private set; }
    public GameObject SkillSlot { get; private set; }
    public Sprite[] MonsterSprites { get; private set; }
    private void Awake()
    {
        _Instance = this;

        
        //HandCard;
        HandCardModel = Resources.Load<GameObject>("Cards/handCard");
        Bg_Blue = Resources.Load<Sprite>(path_handCards + "blue_bg");
        Bg_Red = Resources.Load<Sprite>(path_handCards + "red_bg");
        Bg_Green = Resources.Load<Sprite>(path_handCards + "green_bg");
        Bg_Purple = Resources.Load<Sprite>(path_handCards + "purple_bg");
        Icon_Sword = Resources.Load<Sprite>(path_handCards + "sword_icon");
        Icon_Shield = Resources.Load<Sprite>(path_handCards + "shild_icon");
        Icon_Magic = Resources.Load<Sprite>(path_handCards + "magic_icon");
        Icon_Bow = Resources.Load<Sprite>(path_handCards + "bow_icon");

        //MonsterCard;
        EnemyCardModel = Resources.Load<GameObject>("Cards/monsterCard");
        SkillSlot = Resources.Load<GameObject>("Cards/monsterSlot");
        MonsterSprites = Resources.LoadAll<Sprite>("Textures/Enemies");
    }
}
