using System.Collections;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityStandardAssets.Characters.ThirdPerson;

public class PassengerDropoff : MonoBehaviour {

    public GameObject passenger;
    public GameObject path;
    public GameObject pickup;
    public Transform target;

    private GameObject uber;
    private GameObject textBox;
    private GameObject timer;

    public float time_limit;

    private bool insideDropoff;
    private float total_speed;
    private float avg_speed;
    private int count;

    void Start()
    {
        uber = GameObject.Find("Car");
        textBox = GameObject.Find("Canvas/TextBox");
        timer = GameObject.Find("Canvas/Timer");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Uber")
        {
            Debug.Log("In drop off zone");
            insideDropoff = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Uber")
        {
            Debug.Log("Out of drop off zone");
            insideDropoff = false;
        }
    }

    void Update()
    {
        if (passenger.GetComponent<AICharacterControl>().status == "riding")
        {
            total_speed += uber.GetComponent<CarController>().CurrentSpeed;
            count++;
            avg_speed = total_speed / count;

            if (insideDropoff && uber.GetComponent<CarController>().CurrentSpeed < 1 && timer.GetComponent<RideTimer>().Timing())
            {
                StartCoroutine(DropOff());
            }
        }
    }

    IEnumerator DropOff()
    {
        int time = timer.GetComponent<RideTimer>().StopTimer();
        float distance = avg_speed * time/(10*60*60);
        string distance_string, time_string;

        // miles vs feet
        if (distance > 0.2)
        {
            distance_string = distance.ToString("F2") + " miles";
        }
        else
        {
            distance_string = (distance * 5280).ToString("F0") + " feet";
        }

        // minutes and seconds
        if (time > 600)
        {
            time_string = time / 600 + " minutes and " + (time % 600 / 10) + " seconds.";

        }
        else
        {
            time_string = time / 10 + " seconds.";
        }


        float stars;
        float ride_time = time / 10;
        if (ride_time < time_limit)
        {
            stars = 3.0f;
            textBox.GetComponent<GameText>().Score(stars);
        }
        else if (ride_time < time_limit * 1.1)
        {
            stars = 2.5f;
            textBox.GetComponent<GameText>().Score(stars);
        }
        else if (ride_time < time_limit * 1.2)
        {
            stars = 2.0f;
            textBox.GetComponent<GameText>().Score(stars);
        }
        else if (ride_time < time_limit * 1.3)
        {
            stars = 1.5f;
            textBox.GetComponent<GameText>().Score(stars);
        }
        else if (ride_time < time_limit * 1.4)
        {
            stars = 1.0f;
            textBox.GetComponent<GameText>().Score(stars);
        }
        else if (ride_time < time_limit * 1.5)
        {
            stars = 0.5f;
            textBox.GetComponent<GameText>().Score(stars);
        }
        else
        {
            stars = 0f;
            textBox.GetComponent<GameText>().Score(stars);
        }

        // provide feedback from this ride
        string text = "Dropping off " + passenger.name + ". You drove " + distance_string + " in " + time_string + 
            " You earned " + stars.ToString("F1") + " stars on this ride.";
        textBox.GetComponent<GameText>().PrintMessage(text);

        yield return new WaitForSeconds(0.5f);

        // turn off path and pickup location for this passenger
        path.SetActive(false);
        pickup.SetActive(false);
        
        //float CarX = uber.transform.position.x + 1.5f;
        //float CarY = uber.transform.position.y;
        //float CarZ = uber.transform.position.z;
        //passenger.transform.position = new Vector3(CarX, CarY, CarZ);

        // make passenger appear next to the car
        passenger.transform.position = GameObject.Find("Car/Target").transform.position;
        passenger.GetComponent<AICharacterControl>().SetTarget(target);
        passenger.GetComponent<AICharacterControl>().status = "complete";
        passenger.SetActive(true);
        GetComponent<Renderer>().enabled = false;

        textBox.GetComponent<ErrorCounter>().rides_completed++;
    }
}
