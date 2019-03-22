using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour {

    private GameObject udp;
    private GameObject userCar;
    private GameObject speedBox;
    private GameObject textBox;

    private GameObject reverseIndicator;
    private GameObject wheelIndicator;

    public int speedLimit;
    private Color fastColor;
    private Color normalColor;

    private bool speedingMessage;

    private void Start () 
    {
        udp = GameObject.Find("_udp");
        userCar = GameObject.Find("Car");
        speedBox = GameObject.Find("Canvas/Speedometer/SpeedDisplay");
        textBox = GameObject.Find("Canvas/TextBox");

        reverseIndicator = GameObject.Find("Canvas/Speedometer/ReverseIndicator");
        wheelIndicator = GameObject.Find("Canvas/Speedometer/WheelIndicator");

        speedLimit = 35;
        fastColor = new Color(1, 0, 0, 1);  //red
        normalColor = new Color(1, 1, 1, 1);  //white
    }

    private void Update () 
    {
        float speed = userCar.GetComponent<CarController>().CurrentSpeed;
        speedBox.GetComponent<Text>().text = "" + (int)speed;

        // change color of speed if over speed limit
        if (speed > speedLimit)
        {
            speedBox.GetComponent<Text>().color = fastColor;

            if (textBox.GetComponent<GameText>().done)
            {
                string text = "Slow down! The speed limit is " + speedLimit + " mph.";
                textBox.GetComponent<GameText>().DisplayMessage(text);
                textBox.GetComponent<ErrorCounter>().speedLimitErrors++;
                speedingMessage = true;
            }
        }
        else
        {
            speedBox.GetComponent<Text>().color = normalColor;
            if (speedingMessage)
            {
                textBox.GetComponent<GameText>().DisplayMessage(" ");
                speedingMessage = false;
            }
        }

        // indicate if driving in reverse
        if (userCar.GetComponent<CarUserControl>().r)
        {
            reverseIndicator.SetActive(true);
        }
        else
        {
            reverseIndicator.SetActive(false);
        }

        // indicate wheel position
        if (udp.GetComponent<UnityInputManager>().wheelUp)
        {
            wheelIndicator.GetComponent<Text>().text = "Wheel UPRIGHT";
        } 
        else if (udp.GetComponent<UnityInputManager>().wheelBack)
        {
            wheelIndicator.GetComponent<Text>().text = "Wheel FLAT";
        }
    }
}
