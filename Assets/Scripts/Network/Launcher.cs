using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

/// <summary>
/// 登陆器
/// </summary>
public class Launcher : MonoBehaviour
{
    public GameObject mask;
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;

    private UILoadingCall loadingCall;

    public Button loginBtn;
    public Button exitBtn;

    public PlayerSocket socket;
    void Start()
    {
        socket = new PlayerSocket();
        UIManager._Instance.LoadingCall("Checking...", () => { Debug.LogError("检查服务器失败!"); });

        socket.Connect("127.0.0.1", 9264, () => { UIManager._Instance.Clear(); });


        loginBtn.onClick.AddListener(delegate
        {
            
        });
    }

}
