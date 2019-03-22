using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class TutorialText : MonoBehaviour {

    private GameObject car;
    public GameObject mirrorArrow;
    public GameObject scoreArrow;
    public GameObject speedArrow;

    public GameObject warningText;

    private bool once, twice, thrice, fourth;
    private string text1, text2, text3, text4;

    public bool tutorial;

    void Start() 
    {
        car = GameObject.Find("Car");

        once = twice = thrice = fourth = false;

        text1 = "Welcome to the tutorial. Here you will learn how to become an Uber driver. Let's review the features on your display.";
        text2 = "The top of your screen shows a view behind your car, similar to a rear view mirror. Use this to check for people and cars behind you.";
        text3 = "Your score is shown in the top right corner. Bad driving will cause stars to fade. If the score reaches zero, the game is over.";
        text4 = "The bottom left corner displays your current speed. Be sure to follow all speed limits. To begin driving, step on the gas pedal.";

        GetComponent<GameText>().PrintMessage(text1);
    }

    void Update()
    {
        if (tutorial)
        {
            if (GetComponent<GameText>().done && !once)
            {
                once = true;
            }
            else if (GetComponent<GameText>().done && once && !twice)
            {
                twice = true;
                mirrorArrow.SetActive(true);
                GetComponent<GameText>().PrintMessage(text2);
            }
            else if (GetComponent<GameText>().done && once && twice && !thrice)
            {
                thrice = true;
                mirrorArrow.SetActive(false);
                scoreArrow.SetActive(true);
                GetComponent<GameText>().PrintMessage(text3);
            }
            else if (GetComponent<GameText>().done && once && twice && thrice && !fourth)
            {
                fourth = true;
                scoreArrow.SetActive(false);
                speedArrow.SetActive(true);
                GetComponent<GameText>().PrintMessage(text4);
            }
            else if (GetComponent<GameText>().done && thrice)
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

    public IEnumerator Failed(Transform previousTrigger, float points)
    {
        car.transform.rotation = new Quaternion(previousTrigger.rotation.x, previousTrigger.rotation.y, previousTrigger.rotation.z, previousTrigger.rotation.w);
        car.transform.position = new Vector3(previousTrigger.position.x, previousTrigger.position.y, previousTrigger.position.z);

        car.GetComponent<Rigidbody>().velocity = Vector3.zero;
        car.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        GetComponent<GameText>().Score(points);
        warningText.SetActive(true);
        yield return new WaitForSeconds(2);
        warningText.SetActive(false);
    }
}
