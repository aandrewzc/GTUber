using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class TutorialText : MonoBehaviour {

    public GameObject textBox;
    public GameObject car;
    public GameObject mirrorArrow;
    public GameObject scoreArrow;
    public GameObject speedArrow;

    public GameObject warningText;
    public GameObject scoreBox;

    public bool done;
    private bool once, twice, thrice, fourth;
    private string text1, text2, text3, text4;

    public bool tutorial;

    void Start() 
    {
        done = false;
        once = twice = thrice = fourth = false;

        text1 = "Welcome to the tutorial. Here you will learn how to become an Uber driver. Let's review the features on your display.";
        text2 = "The top of your screen shows a view behind your car, similar to a rear view mirror. Use this to check for people and cars behind you.";
        text3 = "Your score is shown in the top right corner. Bad driving will cause stars to fade. If the score reaches zero, the game is over.";
        text4 = "The bottom left corner displays your current speed. Be sure to follow all speed limits. To begin driving, step on the gas pedal.";

        PrintMessage(text1);
    }

    void Update()
    {
        if (tutorial)
        {
            if (done && !once)
            {
                once = true;
            }
            else if (done && once && !twice)
            {
                twice = true;
                mirrorArrow.SetActive(true);
                PrintMessage(text2);
            }
            else if (done && once && twice && !thrice)
            {
                thrice = true;
                mirrorArrow.SetActive(false);
                scoreArrow.SetActive(true);
                PrintMessage(text3);
            }
            else if (done && once && twice && thrice && !fourth)
            {
                fourth = true;
                scoreArrow.SetActive(false);
                speedArrow.SetActive(true);
                PrintMessage(text4);
            }
            else if (done && thrice)
            {
                speedArrow.SetActive(false);
                car.GetComponent<CarUserControl>().enabled = true;
            }
        }
        else
        {
            car.GetComponent<CarUserControl>().enabled = true;
        }
    }

    public void DisplayMessage(string message)
    {
        textBox.GetComponent<Text>().text = message;
    }

    public void PrintMessage(string message)
    {
        done = false;
        StartCoroutine(Print(message));
    }

    IEnumerator Print(string message)
    {
        for (int i = 0; i <= message.Length; i++)
        {
            textBox.GetComponent<Text>().text = message.Substring(0, i);
            if (i > 0 && message[i-1] == '.')
            {
                yield return new WaitForSeconds(0.1f);
            }
            else if (i > 0 && message[i - 1] == ' ')
            {
                yield return new WaitForSeconds(0.02f);
            }
            else
            {
                yield return new WaitForSeconds(0.005f);
            }
        }
        yield return new WaitForSeconds(1.5f);
        done = true;
    }

    public IEnumerator Failed(Transform previousTrigger, float points)
    {
        car.transform.rotation = new Quaternion(previousTrigger.rotation.x, previousTrigger.rotation.y, previousTrigger.rotation.z, previousTrigger.rotation.w);
        car.transform.position = new Vector3(previousTrigger.position.x, previousTrigger.position.y, previousTrigger.position.z);

        car.GetComponent<Rigidbody>().velocity = Vector3.zero;
        car.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        Score(points);
        warningText.SetActive(true);
        yield return new WaitForSeconds(2);
        warningText.SetActive(false);
    }

    public void Score(float points)
    {
        scoreBox.GetComponent<ScoreManager>().ChangeScore(points);
    }
}
