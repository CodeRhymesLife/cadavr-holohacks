using System;
using System.IO;
using System.Text;
using System.Collections;
using UnityEngine;

#if !UNITY_EDITOR
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Sockets;
#endif


// UdpPacket provides packetIO over UDP
public class DatagramSocketIO : MonoBehaviour
{
    private OscMessageHandler AllMessageHandler;

#if !UNITY_EDITOR
    DatagramSocket socket;

    // use this for initialization
    void Start()
    {
        // Do nothing. Call init
    }

    public void init(string hostIP, int remotePort, int localPort, OscMessageHandler allMessageHandler) {
        AllMessageHandler = allMessageHandler;
    
        Debug.Log("Waiting for a connection...");

        socket = new DatagramSocket();
        socket.MessageReceived += Socket_MessageReceived;

        Debug.Log(String.Format("{0}: Connecting to {1}.", this, hostIP));
        IAsyncAction connectAction = socket.ConnectAsync(new HostName(hostIP), remotePort.ToString());
        connectAction.AsTask().Wait();
        Debug.Log(String.Format("{0}: Connect Complete. Status {1}, ErrorCode {2}", this, connectAction.Status, 
            connectAction.ErrorCode != null ? connectAction.ErrorCode.Message : "None"));

        Debug.Log("exit start");
    }

    private async void Socket_MessageReceived(Windows.Networking.Sockets.DatagramSocket sender,
        Windows.Networking.Sockets.DatagramSocketMessageReceivedEventArgs args)
    {
        Debug.Log("Datagram socket message received");

        //Read the message that was received from the UDP echo client.
        Stream streamIn = args.GetDataStream().AsStreamForRead();
        StreamReader reader = new StreamReader(streamIn);
        string message = await reader.ReadLineAsync();

        Debug.Log("Datagram socket message: " + message);

        byte[] msg = GetBytes(message);
        ArrayList messages = Osc.PacketToOscMessages(msg, msg.Length);
        foreach (OscMessage om in messages)
        {
            if (AllMessageHandler != null)
                AllMessageHandler(om);
        }

        Debug.Log("MESSAGE: " + message);
    }

    static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

#endif
}
