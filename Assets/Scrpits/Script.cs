using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;
using System.Threading;
using System.Threading.Tasks;

public class Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // using Nakama;

        GetSession();

        // var account = client.GetAccountAsync(session);
        // Debug.Log("User id: '{0}'", account.User.Id);
        // Debug.LogFormat("User username: '{0}'", account.User.Username);
        // Debug.LogFormat("Account virtual wallet: '{0}'", account.Wallet);

        // var socket = client.NewSocket();
        // socket.Connected += () => Debug.Log("Socket connected.");
        // socket.Closed += () => Debug.Log("Socket closed.");
        // socket.ConnectAsync(session);

        // var client = new Client("defaultkey", UnityWebRequestAdapter.Instance);
        // var socket = client.NewSocket();

        // or
        // #if UNITY_WEBGL && !UNITY_EDITOR
        //     ISocketAdapter adapter = new JsWebSocketAdapter();
        // #else
        //     ISocketAdapter adapter = new WebSocketAdapter();
        // #endif
        // var socket = Socket.From(client, adapter);
        // Debug.LogFormat("socket:'{0}'",socket);
    }
    
    async void GetSession(){
        const string scheme = "http";
        const string host = "127.0.0.1";
        const int port = 7350;
        const string serverKey = "defaultkey";
        var client = new Client(scheme, host, port, serverKey);
        
        var deviceId = SystemInfo.deviceUniqueIdentifier;
        var sessionStr =  await client.AuthenticateDeviceAsync(deviceId);
        Debug.Log(sessionStr);
        Debug.Log("debug test");

        const string prefKeyName = "nakama.session";
        ISession session;
        var authToken = PlayerPrefs.GetString(prefKeyName);
        if (string.IsNullOrEmpty(authToken) || (session = Session.Restore(authToken)).IsExpired)
        {
            Debug.Log("Session has expired. Must reauthenticate!");
            return;
        };
        Debug.Log(session);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
