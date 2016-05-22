using System;
using System.IO;
using System.Text;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

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

    public async void init(string hostIP, int remotePort, int localPort, OscMessageHandler allMessageHandler) {
        AllMessageHandler = allMessageHandler;
    
        Debug.Log("Waiting for a connection...");

        socket = new DatagramSocket();
        socket.MessageReceived += Socket_MessageReceived;

        // Bind to any port
        Debug.Log(String.Format("{0}: UdpSocketClient created. Binding to port {1}.", this, localPort.ToString()));
        try
        {
            await socket.BindEndpointAsync(null, localPort.ToString());
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            Debug.Log(SocketError.GetStatus(e.HResult).ToString());
            return;
        }
    }

    private void Socket_MessageReceived(Windows.Networking.Sockets.DatagramSocket sender,
        Windows.Networking.Sockets.DatagramSocketMessageReceivedEventArgs args)
    {
        Debug.Log("Datagram socket message received");

        //Read the message that was received from the UDP echo client.
        Stream streamIn = args.GetDataStream().AsStreamForRead();
        byte[] msg = ReadToEnd(streamIn);

        ArrayList messages = Osc.PacketToOscMessages(msg, msg.Length);
        foreach (OscMessage om in messages)
        {
            if (AllMessageHandler != null) {
                try
                {
                    AllMessageHandler(om);
                }
                catch (Exception e)
                {
                    Debug.Log(e.ToString());
                    return;
                }               
            }
        }
        
    }

    public static byte[] ReadToEnd(System.IO.Stream stream)
    {
        long originalPosition = 0;

        if(stream.CanSeek)
        {
             originalPosition = stream.Position;
             stream.Position = 0;
        }

        try
        {
            byte[] readBuffer = new byte[4096];

            int totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;

                if (totalBytesRead == readBuffer.Length)
                {
                    int nextByte = stream.ReadByte();
                    if (nextByte != -1)
                    {
                        byte[] temp = new byte[readBuffer.Length * 2];
                        Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                        Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                        readBuffer = temp;
                        totalBytesRead++;
                    }
                }
            }

            byte[] buffer = readBuffer;
            if (readBuffer.Length != totalBytesRead)
            {
                buffer = new byte[totalBytesRead];
                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
            }
            return buffer;
        }
        finally
        {
            if(stream.CanSeek)
            {
                 stream.Position = originalPosition; 
            }
        }
    }

#endif
}
