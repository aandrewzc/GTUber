using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ErrorCounter : MonoBehaviour {

    public int trafficLightErrors;
    public int stopSignErrors;
    public int roadBumperErrors;
    public int speedLimitErrors;
    public int speechErrors;
    public int imageErrors;

    public int laneTime;

    public int rides_completed;

    private void Start()
    {
        ResetErrors();
        rides_completed = 0;
    }

    public void ResetErrors()
    {
        trafficLightErrors = 0;
        stopSignErrors = 0;
        roadBumperErrors = 0;
        speedLimitErrors = 0;
        speechErrors = 0;
        imageErrors = 0;

        laneTime = 0;
    }


    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "RaceArea01" || SceneManager.GetActiveScene().name == "RaceArea02")
        { 
            if (speedLimitErrors % 49 == 1)
            {
                GameObject.Find("Car").GetComponent<ImageRecTrigger>().PoliceChase();
            }

            int total = trafficLightErrors + stopSignErrors + roadBumperErrors + speechErrors + imageErrors;


            if (rides_completed == 6)
            {
                StartCoroutine(NextLevel());
            }
        }
    }

    public void EndGame()
    {
        if (trafficLightErrors >= roadBumperErrors)
        {
            SceneManager.LoadScene("TrafficLight");

            //StartCoroutine(TrafficSchool());
        }
        else
        {
            SceneManager.LoadScene("OffRoad");

            //StartCoroutine(Offroad());
        }
    }

    //IEnumerator Offroad()
    //{
    //    GetComponent<GameText>().PrintMessage("Your steering could use some work. Try practicing in a new level.");
    //    yield return new WaitForSeconds(3);
    //    SceneManager.LoadScene("OffRoad");
    //}

    //IEnumerator TrafficSchool()
    //{
    //    GetComponent<GameText>().PrintMessage("You're making mistakes at traffic lights. Practice these in a new level.");
    //    yield return new WaitForSeconds(3);
    //    SceneManager.LoadScene("TrafficLight");
    //}

    IEnumerator NextLevel()
    {
        GetComponent<GameText>().PrintMessage("Nice job! You drove all the passengers in this level. Now try something harder...");
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("RaceArea02");
    }
}
