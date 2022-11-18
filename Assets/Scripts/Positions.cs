using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;

using UnityEngine;
using System.Net;
using UnityEngine.UI;

public class Positions : MonoBehaviour
{

    public Text text;
    public delegate void ReceiveAction(string message);
    public static event ReceiveAction OnReceived;
    public static event ReceiveAction OnRoutesReceived;
    private ClientWebSocket webSocket = null;
    private ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();

    [SerializeField]
    private string url = "ws://195.134.66.232:6999/broker";

    // class PositionDataType{
    //     public string message_type;
    //     public double latitude;
    //     public double longitude;
    //     public string item;
    //     public string speed;
    //     public string distance;

    //     public void print(){
    //         Debug.Log( message_type + " " + latitude + " " + longitude + " " + item + " " + speed + " " + distance);
    //     }
    // }

    void Start()
    {
        Task connect_one = Connect(url);
    }

    void OnDestroy()
    {
        if (webSocket != null)
        {
            webSocket.Dispose();
        }
        Debug.Log("the socket one is closed");

    }

    public async Task Connect(string url_one)
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        try
        {
            webSocket = new ClientWebSocket();
            Debug.Log("Connecting TO " + url);
            await webSocket.ConnectAsync(new Uri(url), CancellationToken.None);
        }
        catch (Exception e)
        {
            Debug.LogError("EXCEPTION IN CONNECTING TO WEBSOCKET SERVER.");
            Debug.LogError(e);
        }

        Debug.Log("receiving message.");
        await Receive();
        Debug.LogError("THIS SHOULD NOT PRINT.");

    }


    private async Task Send()   // this function is to answer something, currently not called
    {
        string message = "some ACK text if needed";
        var encoded = Encoding.UTF8.GetBytes(message);
        var buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);

        await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    private async Task Receive(int length = 400000)
    {
        ArraySegment<Byte> buffer = new ArraySegment<byte>(new byte[length]);

        while (webSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = null;

            using (var ms = new MemoryStream())
            {
                Debug.Log("start");

                do
                {
                    result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);

                } while (!result.EndOfMessage);
                 

                ms.Seek(0, SeekOrigin.Begin);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    
                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        string message = reader.ReadToEnd();
                        if (message.Contains("New Session with id")){
                            continue;
                        }
                        else if(message.Contains("routes")){
                            OnRoutesReceived(message);
                            continue;
                        }

                        text.text = message;
                        Debug.Log(message);
                        // PositionDataType data = JsonUtility.FromJson<PositionDataType>(message);

                        // // this is needed to have the correct decimal digits!!
                        // data.latitude = Math.Round(data.latitude, 4, MidpointRounding.ToEven);
                        // data.longitude = Math.Round(data.longitude, 4, MidpointRounding.ToEven);

                        // text.text = data.message_type + " " + data.latitude + " " + data.longitude + " " + data.speed+ " " + data.distance;
                        // data.print();

                        //must be delete in case of threads 
                        //await Task.Run(Process);
                        //the call may happen elseware
                        if (OnReceived != null) OnReceived(message);
                    }
                }else if(result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);

                }
            }
        }
    }
}
