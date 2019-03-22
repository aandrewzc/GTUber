using UnityEngine;

public class TrafficLightTutorial : MonoBehaviour {

    private GameObject textBox;
    public Transform previousTrigger;
    public GameObject redLight;

    private void Start()
    {
        textBox = GameObject.Find("Canvas/TextBox");
    }

    void OnTriggerEnter(Collider other)
    {
        if (redLight.GetComponent<Light>().enabled)
        {
            StartCoroutine(textBox.GetComponent<TutorialText>().Failed(previousTrigger, -1.5f));

            string text = "Don't run a red light! Wait to go until the light turns green.";
            textBox.GetComponent<GameText>().PrintMessage(text);
        }
    }
}
