using UnityEngine;

public class RoadBumper : MonoBehaviour {

    private GameObject textBox;

    private void Start()
    {
        textBox = GameObject.Find("/Canvas/TextBox");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Uber")
        {
            textBox.GetComponent<GameText>().PrintMessage("Stay on the road!");
            textBox.GetComponent<GameText>().Score(-0.5f);
            textBox.GetComponent<ErrorCounter>().roadBumperErrors++;
        }
    }
}
