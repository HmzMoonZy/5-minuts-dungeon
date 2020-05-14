using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LitJson;

public class MonsterCard : DungeonCardBase
{
    public int Slot_Red { get; set; }
    public int Slot_Blue { get; set; }
    public int Slot_Green { get; set; }
    public int Slot_Purple { get; set; }

    private Transform slot_parent;
    private List<SlotController> slotList = new List<SlotController>();

    public void InitSlots()
    {
        base.FixedUI();

        slot_parent = transform.Find("SkillPanel").GetComponent<Transform>();
        transform.Find("EnemyPanel/Enemy_Icon").GetComponent<Image>().sprite = CardViews._Instance.MonsterSprites[DungeonCard_Id - 1];

        if (Slot_Red != 0)
        {
            for (int i = 0; i < Slot_Red; i++)
            {
                GameObject slot = Instantiate(CardViews._Instance.SkillSlot, slot_parent);
                slot.GetComponent<Image>().sprite = CardViews._Instance.Bg_Red;
                slot.GetComponent<SlotController>().type = CardType.RED_SWORD;
                slotList.Add(slot.GetComponent<SlotController>());
            }
        }

        if (Slot_Blue != 0)
        {
            for (int i = 0; i < Slot_Blue; i++)
            {
                GameObject slot = Instantiate(CardViews._Instance.SkillSlot, slot_parent);
                slot.GetComponent<Image>().sprite = CardViews._Instance.Bg_Blue;
                slot.GetComponent<SlotController>().type = CardType.BLUE_SHIELD;
                slotList.Add(slot.GetComponent<SlotController>());
            }
        }

        if (Slot_Green != 0)
        {
            for (int i = 0; i < Slot_Green; i++)
            {
                GameObject slot = Instantiate(CardViews._Instance.SkillSlot, slot_parent);
                slot.GetComponent<Image>().sprite = CardViews._Instance.Bg_Green;
                slot.GetComponent<SlotController>().type = CardType.GREEN_BOW;
                slotList.Add(slot.GetComponent<SlotController>());
            }
        }

        if (Slot_Purple != 0)
        {
            for (int i = 0; i < Slot_Purple; i++)
            {
                GameObject slot = Instantiate(CardViews._Instance.SkillSlot, slot_parent);
                slot.GetComponent<Image>().sprite = CardViews._Instance.Bg_Purple;
                slot.GetComponent<SlotController>().type = CardType.PURPLE_MAGIC;
                slotList.Add(slot.GetComponent<SlotController>());
            }
        }


        Debug.Log("#生成卡牌 ID:" + DungeonCard_Id);
    }


    public int GetHelthPoint()
    {
        return Slot_Red + Slot_Blue + Slot_Green + Slot_Purple;
    }

    public List<SlotController> GetSlotControllers()
    {
        return slotList;
    }


}
