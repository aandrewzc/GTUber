using UnityEngine;

public class RoadBlockTrigger : MonoBehaviour {

    private GameObject textBox;
    public Transform previousTrigger;

    private void Start()
    {
        textBox = GameObject.Find("Canvas/TextBox");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Uber")
        {
            StartCoroutine(textBox.GetComponent<TutorialText>().Failed(previousTrigger, -1.5f));

            string text = "Don't crash into the police cars! Shift to reverse to find an alternate route.";
            textBox.GetComponent<GameText>().PrintMessage(text);
        }
    }
}
