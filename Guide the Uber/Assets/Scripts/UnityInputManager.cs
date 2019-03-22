using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class UnityInputManager : MonoBehaviour
{
    public GameObject userCar;
    private UdpConnection udpConnection;
    private GameObject pickup;

    public bool wheelBack;
    public bool wheelUp;

    public bool stopped;

    void Start()
    {
        string sendIp = "127.0.0.1";
        int sendPort = 8881;
        int receivePort = 11000;

        udpConnection = new UdpConnection();
        udpConnection.StartConnection(sendIp, sendPort, receivePort);
        userCar = GameObject.Find("Car");
    }

    private void OnApplicationQuit()
    {
        udpConnection.Send("quit");
        udpConnection.Stop();
    }

    public void SendUDP(string message)
    {
        udpConnection.Send(message);
    }

    void Update()
    {
        foreach (var message in udpConnection.getMessages())
        {
            // Example message format: "p:0.12" or "w:-.22,0.42"
            string component = message.Split(':')[0];
            switch (component)
            {
                case "w":
                    if (!stopped)
                    {
                        string temp = message.Split(':')[1];
                        float steering = float.Parse(temp.Split(',')[0]);
                        float tilt = float.Parse(temp.Split(',')[1]);

                        userCar.GetComponent<CarUserControl>().h = steering;

                        if (tilt > -0.2 && tilt < 0.2)
                        {
                            wheelUp = true;
                            if (wheelBack == true)
                            {
                                bool current = userCar.GetComponent<CarUserControl>().r;
                                userCar.GetComponent<CarUserControl>().r = !current;
                            }
                            wheelBack = false;
                        }
                        else if (tilt < -0.9)
                        {
                            wheelUp = false;
                            wheelBack = true;
                        }
                    }
                    else
                    {
                        userCar.GetComponent<CarUserControl>().h = 0;
                    }
                    break;
                case "p":
                    if (!stopped)
                    {
                        float accel = float.Parse(message.Split(':')[1]);
                        userCar.GetComponent<CarUserControl>().v = accel;
                    }
                    else
                    {
                        userCar.GetComponent<CarUserControl>().v = -1;
                    }
                    break;
                case "pickup":
                    Debug.Log(message);
                    string msg = message.Split(':')[1];
                    string pass_name = msg.Split(',')[0];
                    string result = msg.Split(',')[1];

                    pickup = GameObject.Find("Pickup" + pass_name);
                    if (result == "correct")
                    {
                        pickup.GetComponent<PassengerPickup>().verified = true;
                    }
                    else if (result == "incorrect")
                    {
                        pickup.GetComponent<PassengerPickup>().retry = true;
                    }
                    break;
                case "police":
                    string pol = message.Split(':')[1];
                    if(pol == "pass")
                    {
                        userCar.GetComponent<ImageRecTrigger>().verified = true;
                    }
                    else
                    {
                        userCar.GetComponent<ImageRecTrigger>().retry = true;
                    }
                    break;
            }
        }

    }
}
