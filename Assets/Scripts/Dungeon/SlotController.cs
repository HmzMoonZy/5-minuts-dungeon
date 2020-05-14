using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    public CardType type;

    private Image image;
    private Color broke = new Color(70 / 255f, 70 / 255f, 70 / 255f, 105 / 255f);

    public bool isBorke;

    private void Awake()
    {
        isBorke = false;
        image = GetComponent<Image>();
    }

    public void Broke()
    {
        isBorke = true;
        image.color = broke;
    }
}
