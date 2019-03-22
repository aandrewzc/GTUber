using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class Tutorial002 : MonoBehaviour {

    private GameObject car;
    private GameObject textBox;
    public GameObject stopSignTrigger;

    private bool once;
    private bool insideBox;

    private void Start()
    {
        once = false;
        insideBox = false;

        car = GameObject.Find("Car");
        textBox = GameObject.Find("Canvas/TextBox");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Uber")
        {
            Debug.Log("In Trigger");
            insideBox = true;
        }
    }

    public void TaskFailed()
    {
        insideBox = false;
    }

    void Update()
    {
        if (!once && insideBox && car.GetComponent<CarController>().CurrentSpeed < 0.1)
        {
            once = true;
            stopSignTrigger.SetActive(false);
            Debug.Log("complete");
            string text = "Great job. Continue to the intersection ahead and turn right.";
            textBox.GetComponent<GameText>().PrintMessage(text);
            textBox.GetComponent<GameText>().Score(1.5f);
        }
    }

}
