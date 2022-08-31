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
                Debug.Log($"{nameof(UIManager)}�ν��Ͻ��� �̹� �����մϴ�. �����մϴ�.");
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
            UIMessage($"��Ʈ {PortField.text}�� ������ �����մϴ�.");
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
            UIMessage($"{IPField.text}:{PortField.text}������ �������Դϴ�.");
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

    // Ŭ���̾�Ʈ ���� ����� NetworkManager�κ��� ȣ��
    public void BackToMain()
    {
        UIMessage("������ ������ϴ�. �κ�� ���ư��ϴ�.");
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
        UIMessage("���� �����߽��ϴ�.");
    }











    // Popup �޽��� ����, ȣ��
    void UIMessage(string msg)
    {
        if (CurrentPopupMsg != null)
            Destroy(CurrentPopupMsg.gameObject); // ���� �˾� ����� �ı�
        CurrentPopupMsg = Instantiate(PopupMessage, PopupPosition, Quaternion.identity, canvas.transform).GetComponent<PopupMessage>();
        CurrentPopupMsg.SetText(msg);
    }
    void NotMaded()
    {
        UIMessage("���� �������� ���� ����Դϴ�.");
    }
}
