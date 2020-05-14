using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UILoadingCall : MonoBehaviour
{
    public Image loadingIcon;
    public TextMeshProUGUI tmpText;

    public float rotateSpeed;

    public Action timeOutCallBack;
    public float liveTime;
    private float totalTime;

    private string message;

    private void Update()
    {
        totalTime += Time.deltaTime;

        if (totalTime >= liveTime)
        {
            timeOutCallBack?.Invoke();
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        loadingIcon.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, -rotateSpeed));
    }

    public void Init(string message, Action timeOutCallBack)
    {
        tmpText.text = message;
        this.timeOutCallBack += timeOutCallBack;
    }
}
