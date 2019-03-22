using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class StopBox : MonoBehaviour {

    private GameObject car;
    public GameObject goBox;
    private bool insideBox;

    private void Start()
    {
        car = GameObject.Find("Car");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Uber")
        {
            Debug.Log("In stop box");
            insideBox = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Uber")
        {
            Debug.Log("Out of stop box");
            insideBox = false;
        }
    }

    void Update()
    {
        // if car stops within the box, it is allowed to continue driving
        if (insideBox && car.GetComponent<CarController>().CurrentSpeed < 0.1)
        {
            goBox.GetComponent<GoBox>().CarStopped();
        }
    }
}
