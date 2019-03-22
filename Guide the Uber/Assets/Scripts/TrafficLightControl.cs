using System.Collections;
using UnityEngine;

public class TrafficLightControl : MonoBehaviour {

    public GameObject north;
    public GameObject south;
    public GameObject east;
    public GameObject west;

    private float red_time;
    private float yellow_time;
    private float green_time;

    // Use this for initialization
    void Start () 
    {
        red_time = 0.5f;
        yellow_time = 1.5f;
        green_time = 5;
    }

    public void SetTimes(float r, float y, float g)
    {
        red_time = r;
        yellow_time = y;
        green_time = g;
    }

    public IEnumerator LightSequence()
    {
        while (true)
        {
            // north-south red, east-west red
            ChangeLight(north, "red");
            ChangeLight(south, "red");
            yield return new WaitForSeconds(red_time);

            // north-south red, east-west green
            ChangeLight(east, "green");
            ChangeLight(west, "green");
            yield return new WaitForSeconds(green_time);

            // north-south red, east-west yellow
            ChangeLight(east, "yellow");
            ChangeLight(west, "yellow");
            yield return new WaitForSeconds(yellow_time);

            // north-south red, east-west red
            ChangeLight(east, "red");
            ChangeLight(west, "red");
            yield return new WaitForSeconds(red_time);

            // north-south green, east-west red
            ChangeLight(north, "green");
            ChangeLight(south, "green");
            yield return new WaitForSeconds(green_time);

            // north-south yellow, east-west red
            ChangeLight(north, "yellow");
            ChangeLight(south, "yellow");
            yield return new WaitForSeconds(yellow_time);
        }
    }

    private void ChangeLight(GameObject trafficLight, string color)
    {
        if (trafficLight != null)
        {
            switch (color)
            {
                case "red":
                    trafficLight.transform.GetChild(0).GetComponent<Light>().enabled = true;
                    trafficLight.transform.GetChild(1).GetComponent<Light>().enabled = false;
                    trafficLight.transform.GetChild(2).GetComponent<Light>().enabled = false;

                    trafficLight.transform.GetChild(3).GetComponent<Light>().enabled = true;
                    trafficLight.transform.GetChild(4).GetComponent<Light>().enabled = false;
                    trafficLight.transform.GetChild(5).GetComponent<Light>().enabled = false;
                    break;

                case "yellow":
                    trafficLight.transform.GetChild(0).GetComponent<Light>().enabled = false;
                    trafficLight.transform.GetChild(1).GetComponent<Light>().enabled = true;
                    trafficLight.transform.GetChild(2).GetComponent<Light>().enabled = false;

                    trafficLight.transform.GetChild(3).GetComponent<Light>().enabled = false;
                    trafficLight.transform.GetChild(4).GetComponent<Light>().enabled = true;
                    trafficLight.transform.GetChild(5).GetComponent<Light>().enabled = false;
                    break;

                case "green":
                    trafficLight.transform.GetChild(0).GetComponent<Light>().enabled = false;
                    trafficLight.transform.GetChild(1).GetComponent<Light>().enabled = false;
                    trafficLight.transform.GetChild(2).GetComponent<Light>().enabled = true;

                    trafficLight.transform.GetChild(3).GetComponent<Light>().enabled = false;
                    trafficLight.transform.GetChild(4).GetComponent<Light>().enabled = false;
                    trafficLight.transform.GetChild(5).GetComponent<Light>().enabled = true;
                    break;
            }
        }
    }
}
