using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITopPanelManager : MonoBehaviour
{
    public TextMeshProUGUI deckRemaining_text;

    private  void Update()
    {
        deckRemaining_text.text = DungeonDeckManager.Remaining.ToString();
    }
}
