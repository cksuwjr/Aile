using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    // Scene: Menu
    [Header("Popup Message")]
    [Space]
    [SerializeField] GameObject PopupMessage;
    [SerializeField] Vector2 PopupPosition = new Vector2(960, 720);

    [Space]
    [Space]

    [Header("Play Lobby")]
    [Space]
    [SerializeField] GameObject UI;

    [SerializeField] InputField PortField;
    [SerializeField] InputField IPField;
    [SerializeField] Button BtnServerOpen;
    bool OpenReady = false;
    [SerializeField] Button BtnConnect;
    bool ConnectReady = false;
    PopupMessage CurrentPopupMsg;

    private static UIManager _singleton;
    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != null)
            {
                Debug.Log($"{nameof(UIManager)}인스턴스가 이미 존재합니다. 제거합니다.");
                Destroy(value);
            }
        }
    }
    private void Awake()
    {
        Singleton = this;   
    }

    // =================  Scene: Menu  ===================
    public void BtnPlay_Clicked()
    {
        SceneManager.LoadScene("PlayLobby");
    }
    public void BtnSetting_Clicked()
    {
        NotMaded();
    }
    public void BtnHelp_Clicked()
    {
        NotMaded();
    }
    public void BtnExit_Clicked()
    {
        NotMaded();
    }
    // ===============  Scene: PlayLobby  =================
    public void BtnServerOpen_Clicked()
    {
        if (!OpenReady)
        {
            PortField.gameObject.SetActive(true);
            BtnConnect.gameObject.SetActive(false);
            OpenReady = true;
        }
        else
        {
            if (PortField.text == "") PortField.text = "7777";
            UIMessage($"포트 {PortField.text}로 서버를 개설합니다.");
            UI.SetActive(false);
            NetworkManager.Singleton.ServerStart(PortField.text, 10);
        }
    }
    public void BtnConnect_Clicked()
    {
        if (!ConnectReady)
        {
            PortField.gameObject.SetActive(true);
            IPField.gameObject.SetActive(true);
            BtnServerOpen.gameObject.SetActive(false);
            ConnectReady = true;
        }
        else
        {
            if (IPField.text == "") { IPField.text = "127.0.0.1"; }
            if (PortField.text == "") { PortField.text = "7777"; }
            UIMessage($"{IPField.text}:{PortField.text}서버에 참여중입니다.");
            UI.SetActive(false);
            NetworkManager.Singleton.ClientStart(IPField.text, PortField.text);
        }
    }
    public void BtnBack_Clicked()
    {
        if (ConnectReady || OpenReady)
        {
            UI.SetActive(true);
            PortField.gameObject.SetActive(false);
            IPField.gameObject.SetActive(false);
            BtnServerOpen.gameObject.SetActive(true);
            BtnConnect.gameObject.SetActive(true);
            ConnectReady = false;
            OpenReady = false;
        }
        else if (!ConnectReady && !OpenReady)
        {
            SceneManager.LoadScene("Menu");
        }
    }

    // 클라이언트 연결 끊길시 NetworkManager로부터 호출
    public void BackToMain()
    {
        UIMessage("연결이 끊겼습니다. 로비로 돌아갑니다.");
        UI.SetActive(true);
        PortField.gameObject.SetActive(false);
        IPField.gameObject.SetActive(false);
        BtnServerOpen.gameObject.SetActive(true);
        BtnConnect.gameObject.SetActive(true);
        ConnectReady = false;
        OpenReady = false;
    }
    public void SendInformation()
    {
        UIMessage("연결 성공했습니다.");
    }











    // Popup 메시지 구현, 호출
    void UIMessage(string msg)
    {
        if (CurrentPopupMsg != null)
            Destroy(CurrentPopupMsg.gameObject); // 기존 팝업 존재시 파괴
        CurrentPopupMsg = Instantiate(PopupMessage, PopupPosition, Quaternion.identity, canvas.transform).GetComponent<PopupMessage>();
        CurrentPopupMsg.SetText(msg);
    }
    void NotMaded()
    {
        UIMessage("아직 구현되지 않은 기능입니다.");
    }
}
