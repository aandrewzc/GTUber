using UnityEngine;

public class StopSignTrigger : MonoBehaviour {

    public GameObject textBox;
    public Transform previousTrigger;
    public GameObject correctBox;

    private void Start()
    {
        textBox = GameObject.Find("Canvas/TextBox");
    }

    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(textBox.GetComponent<TutorialText>().Failed(previousTrigger, -1.5f));

        correctBox.GetComponent<Tutorial002>().TaskFailed();

        string text = "Stop BEFORE the crosswalk. Try stepping off the pedal sooner this time.";
        textBox.GetComponent<GameText>().PrintMessage(text);
    }
}
