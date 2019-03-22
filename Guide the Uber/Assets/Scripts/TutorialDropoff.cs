using System.Collections;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class TutorialDropoff: MonoBehaviour {

    private GameObject car;
    public GameObject otherCar;
    public GameObject policeCar;
    private bool insideBox;
    private bool once;

    private void Start()
    {
        car = GameObject.Find("Car");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Uber")
        {
            Debug.Log("In crash trigger");
            insideBox = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Uber")
        {
            Debug.Log("Out of crash trigger");
            insideBox = false;
        }
    }

    void Update()
    {
        // if stopped inside pickup zone
        if (insideBox && car.GetComponent<CarController>().CurrentSpeed < 0.5f && !once)
        {
            once = true;
            GameObject.Find("Canvas/ScoreBox").GetComponent<ScoreManager>().LiveForever = true;

            StartCoroutine(CrashSequence());
        }
    }

    IEnumerator CrashSequence()
    {
        otherCar.SetActive(true);
        otherCar.GetComponent<CarAIControl>().SetTarget(car.transform);
        yield return new WaitForSeconds(5);
        policeCar.SetActive(true);
        policeCar.GetComponent<CarAIControl>().SetTarget(car.transform);
        yield return new WaitForSeconds(5);
        if (!car.GetComponent<ImageRecTrigger>().pulledOver)
        {
            StartCoroutine(car.GetComponent<ImageRecTrigger>().PulledOver());
        }

    }
}
