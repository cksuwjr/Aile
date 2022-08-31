using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using RiptideNetworking.Utils;
using System;
public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;
    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if(_singleton != null)
            {
                Debug.Log($"{nameof(NetworkManager)}인스턴스가 이미 존재합니다. 제거합니다.");
                Destroy(value);
            }   
        }
    }
    public Server Server { get; private set; }

    [SerializeField] private ushort port;
    [SerializeField] ushort MaxClientCount;


    public Client Client { get; private set; }
    [SerializeField] private string ip;


    private void Awake()
    {
        Singleton = this;
        Application.targetFrameRate = 60;
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
    }

    public void ServerStart(string port, int maxclientcount)
    {
        this.port = ushort.Parse(port);
        this.MaxClientCount = (ushort)maxclientcount;


        Server = new Server();
        Server.Start(this.port, MaxClientCount);
        Server.ClientDisconnected += PlayerLeft;
    }
    public void ClientStart(string ip, string port)
    {
        this.ip = ip.ToString();
        this.port = ushort.Parse(port);

        Client = new Client();
        Client.Connected += DidConnect; // 연결될시 DidConnect호출
        Client.ConnectionFailed += FailedToConnect; // 연결 실패시 FailedToConnect호출
        Client.ClientDisconnected += PlayerLeft; // ..
        Client.Disconnected += DidDisconnect; //..

        Client.Connect($"{this.ip}:{this.port}");
    }
    private void FixedUpdate()
    {
        if(Server != null)
            Server.Tick();
        if (Client != null)
            Client.Tick();
    }

    private void OnApplicationQuit()
    {
        if(Server != null)
            if(Server.IsRunning)
                Server.Stop();
        if(Client != null)
            if (Client.IsConnecting)
                Client.Disconnect();
    }
    // Server, Client Method
    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
    }
    // Client Method

    private void DidConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.SendInformation();
    }

    private void FailedToConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();
    }
    private void DidDisconnect(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();

    }
}
