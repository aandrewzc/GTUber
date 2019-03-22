using UnityEngine;

public class RoadMedian : MonoBehaviour {

    private GameObject textBox;

    private void Start()
    {
        textBox = GameObject.Find("/Canvas/TextBox");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Uber")
        {
            textBox.GetComponent<GameText>().PrintMessage("Don't drive into oncoming traffic!");
            textBox.GetComponent<GameText>().Score(-0.5f);
            textBox.GetComponent<ErrorCounter>().roadBumperErrors++;
        }
    }
}
