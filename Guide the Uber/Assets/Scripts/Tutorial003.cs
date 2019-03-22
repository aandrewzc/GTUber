using UnityEngine;

public class Tutorial003 : MonoBehaviour {

    private GameObject textBox;
    public GameObject trafficLight;
    private bool once;

    private void Start()
    {
        textBox = GameObject.Find("Canvas/TextBox");
    }
    void OnTriggerEnter(Collider other)
    {
        if (!once)
        {
            once = true;

            //StartCoroutine(trafficLight.GetComponent<TrafficLightControlR>().LightSequence());
            StartCoroutine(trafficLight.GetComponent<TrafficLightControl>().LightSequence());

            string text = "Be sure to wait for a green light.";
            textBox.GetComponent<GameText>().PrintMessage(text);
        }
    }

}
