using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; //实现拖拽功能的命名空间

/// <summary>
/// 手牌类型枚举
/// </summary>
public enum CardType
{
    NULL = -1,
    RED_SWORD,
    BLUE_SHIELD,
    GREEN_BOW,
    PURPLE_MAGIC
}

/// <summary>
/// 玩家手牌类
/// </summary>
public class HandCard : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler//, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CardType type;      //卡牌类型:唯一数值

    private Image bg_img;
    private Image icon_img;
    private GameObject cardBack;
    private Outline outline;

    private Color normal = new Color(1, 92 / 255f, 76 / 255f, 0);
    private Color hightLigt = new Color(1, 92 / 255f, 76 / 255f, 1);
    //PROP
    public CardType Type { get { return type; } }

    private void Awake()
    {
        bg_img = transform.Find("bg").GetComponent<Image>();
        icon_img = transform.Find("Icon").GetComponent<Image>();
        cardBack = transform.Find("CardMask").gameObject;
    }

    private void Start()
    {
        outline = gameObject.GetComponent<Outline>();
        outline.effectColor = normal;
    }

    public HandCard Init(CardType type)
    {
        this.type = type;

        switch (this.type)
        {
            case CardType.RED_SWORD:
                bg_img.sprite = CardViews._Instance.Bg_Red;
                icon_img.sprite = CardViews._Instance.Icon_Sword;
                break;

            case CardType.BLUE_SHIELD:
                bg_img.sprite = CardViews._Instance.Bg_Blue;
                icon_img.sprite = CardViews._Instance.Icon_Shield;
                break;

            case CardType.GREEN_BOW:
                bg_img.sprite = CardViews._Instance.Bg_Green;
                icon_img.sprite = CardViews._Instance.Icon_Bow;
                break;

            case CardType.PURPLE_MAGIC:
                bg_img.sprite = CardViews._Instance.Bg_Purple;
                icon_img.sprite = CardViews._Instance.Icon_Magic;
                break;

            default:
                break;
        }

        return this;
    }

    public void ShowCard()
    {
        cardBack.SetActive(false);
    }

    ///// <summary>
    ///// 开始拖拽
    ///// </summary>
    //void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    //{
    //    Debug.Log("OnBeginDrag");
    //}

    ///// <summary>
    ///// 拖拽中
    ///// </summary>
    //void IDragHandler.OnDrag(PointerEventData eventData)
    //{
    //    Debug.Log("OnDrag...");
    //}

    ///// <summary>
    ///// 结束拖拽
    ///// </summary>
    //void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    //{
    //    Debug.Log("OnEndDrag");
    //}

    /// <summary>
    /// 指针进入
    /// </summary>
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        transform.localPosition += new Vector3(0, 30, 0);
    }

    /// <summary>
    /// 指针离开
    /// </summary>
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        transform.localPosition += new Vector3(0, -30, 0);
        outline.effectColor = normal;
    }

    /// <summary>
    /// 点击
    /// </summary>
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (outline.effectColor.a != 1)
        {
            outline.effectColor = hightLigt;
        }
        else
        {
            outline.effectColor = normal;
        }

        GameManager._Instance.SetSelectHandCard(this);
        GameManager._Instance.OutCard();
    }
}
