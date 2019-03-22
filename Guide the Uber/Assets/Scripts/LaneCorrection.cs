using UnityEngine;

public class LaneCorrection : MonoBehaviour {

    private GameObject textBox;
    public Transform previousTrigger;

    private void Start()
    {
        textBox = GameObject.Find("Canvas/TextBox");
    }

    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(textBox.GetComponent<TutorialText>().Failed(previousTrigger, -0.5f));
        
        string text = "Stay on the road!";
        textBox.GetComponent<GameText>().PrintMessage(text);
    }
}
