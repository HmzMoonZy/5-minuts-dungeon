using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlayerPanelManager : MonoBehaviour
{
    private Transform InfoPanel;
    private TextMeshProUGUI remaining_text;
    private void Start()
    {
        InfoPanel = transform.Find("InfoPanel");
        remaining_text = InfoPanel.Find("Remaining/Number").GetComponent<TextMeshProUGUI>();
    }

    private  void Update()
    {
        remaining_text.text = GameManager._Instance.playerdeckRemaining.ToString();
    }
}
