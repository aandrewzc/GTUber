using UnityEngine;

public class TrafficLightMaxTrigger : MonoBehaviour {

    public GameObject redLight;
    private GameObject textBox;

    private void Start()
    {
        textBox = GameObject.Find("Canvas/TextBox");
    }

    void OnTriggerEnter(Collider other)
    {
        if (redLight.GetComponent<Light>().enabled && other.tag == "Uber")
        {
            textBox.GetComponent<GameText>().PrintMessage("You ran a red light!!");
            textBox.GetComponent<GameText>().Score(-5);
        }
    }
}
