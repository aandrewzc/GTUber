using System.Collections;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityStandardAssets.Characters.ThirdPerson;

public class PassengerPickup : MonoBehaviour {

    public GameObject passenger;
    public GameObject path;
    public GameObject destination;

    private GameObject uber;
    private GameObject textBox;
    private GameObject udp;
    private GameObject timer;

    public bool verified, retry;
    private bool insidePickup;

    private void Start()
    {
        udp = GameObject.Find("_udp");
        uber = GameObject.Find("Car");
        textBox = GameObject.Find("Canvas/TextBox");
        timer = GameObject.Find("Canvas/Timer");
    }

    void OnTriggerEnter(Collider other)
    {
        // true when Uber enters pickup zone
        if (other.tag == "Uber")
        {
            Debug.Log("In pickup zone");
            insidePickup = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // false when Uber exits pickup zone
        if (other.tag == "Uber")
        {
            Debug.Log("Out of pickup zone");
            insidePickup = false;
        }
    }

    void Update()
    {
        // if stopped inside pickup zone, passenger is waiting, not currently giving a ride
        //Debug.Log(passenger.GetComponent<AICharacterControl>().status);
        if (insidePickup && uber.GetComponent<CarController>().CurrentSpeed < 0.1f && 
            passenger.GetComponent<AICharacterControl>().status == "waiting" && 
            !timer.GetComponent<RideTimer>().Timing())
        {
            // disable car and start pickup routine
            //uber.GetComponent<CarUserControl>().enabled = false;
            udp.GetComponent<UnityInputManager>().stopped = true;

            passenger.GetComponent<AICharacterControl>().status = "speech";
            StartCoroutine(PickUp());
        }
    }

    IEnumerator PickUp()
    {
        // passenger walks to car
        passenger.GetComponent<AICharacterControl>().SetTarget(GameObject.Find("Car/Target").transform);

        // speech test (send pickup message to python)s
        udp.GetComponent<UnityInputManager>().SendUDP("pickup:" + passenger.name);

        // prompt user to speak
        string text = "Verify this passenger's identity by asking: \"Uber for " + passenger.name + "?\"";
        textBox.GetComponent<GameText>().PrintMessage(text);

        // THIS CODE IS FOR DEBUGGING ONLYs
        // ****************************
        //yield return new WaitForSeconds(5);
        //verified = true;
        // ****************************

        // wait for response
        while (!verified) 
        {
            if (retry)
            {
                // display try-again message
                retry = false;
                text = "Try again. Ask: \"Uber for " + passenger.name + "?\"";
                textBox.GetComponent<GameText>().PrintMessage(text);
                textBox.GetComponent<ErrorCounter>().speechErrors++;


            }
            yield return new WaitForSeconds(0.1f);
        }
        verified = false;

        text = "Good job. Now follow the highlighted route to the dropoff location";
        textBox.GetComponent<GameText>().PrintMessage(text);

        // add passenger to car and display path
        passenger.GetComponent<AICharacterControl>().status = "riding";
        passenger.SetActive(false);
        path.SetActive(true);
        destination.SetActive(true);
        //uber.GetComponent<CarUserControl>().enabled = true;
        udp.GetComponent<UnityInputManager>().stopped = false;
        timer.GetComponent<RideTimer>().StartTimer();
    }
}
