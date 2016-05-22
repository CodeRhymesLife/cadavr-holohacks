
using UnityEngine;

public class OSCReceiver : MonoBehaviour
{
    public string RemoteIP = "127.0.0.1"; //127.0.0.1 signifies a local host (if testing locally
    public int SendToPort = 9000; //the port you will be sending from
    public int ListenerPort = 8002; //the port you will be listening on
    public Transform controller;
    public Catheter objectToControl;

    //VARIABLES YOU WANT TO BE ANIMATED
    private int yRot = 0; //the rotation around the y axis

    void Start()
    {
        //Initializes on start up to listen for messages
        //make sure this game object has both UDPPackIO and OSC script attached
#if UNITY_EDITOR
        UDPPacketIO udp = GetComponent<UDPPacketIO>();
        udp.init(RemoteIP, SendToPort, ListenerPort);
        Osc handler = GetComponent<Osc>();
        handler.init(udp);
        handler.SetAllMessageHandler(AllMessageHandler);
#endif

#if !UNITY_EDITOR
    DatagramSocketIO datagram = GetComponent<DatagramSocketIO>();
    datagram.init(RemoteIP, SendToPort, ListenerPort, AllMessageHandler);

#endif
    }


    //These functions are called when messages are received
    //Access values via: oscMessage.Values[0], oscMessage.Values[1], etc

    public void AllMessageHandler(OscMessage oscMessage)
    {


        var msgString = Osc.OscMessageToString(oscMessage); //the message and value combined
        string msgAddress = oscMessage.Address; //the message parameters
        var msgValue = oscMessage.Values[0]; //the message value
        Debug.Log("Entire msg: " + msgString); //log the message and values coming from OSC
        Debug.Log("Msg address: " + msgAddress);
        Debug.Log("Msg value: " + msgValue.ToString());

        var msgValueBool = msgValue.ToString() == "1" ? true : false;
        Debug.Log("Message value bool: " + msgValueBool);

        const string button1 = "/1/push1"; // Up
        const string button2 = "/1/push2"; // Down
        const string button3 = "/1/push3"; // Right
        const string button4 = "/1/push4"; // Left

        //FUNCTIONS YOU WANT CALLED WHEN A SPECIFIC MESSAGE IS RECEIVED
        switch (msgAddress)
        {
            case button1:
                objectToControl.MoveUp = msgValueBool;
                Debug.Log("Moving Up");
                break;

            case button2:
                objectToControl.MoveDown = msgValueBool;
                Debug.Log("Moving down");
                break;

            case button3:
                objectToControl.RotateRight = msgValueBool;
                break;

            case button4:
                objectToControl.RotateLeft = msgValueBool;
                break;

            default:
                Debug.Log("Oops, got a message we didnt expect");
                break;
        }
    }
}

