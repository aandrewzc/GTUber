using UnityEngine;

public class Tutorial001 : MonoBehaviour {

    public GameObject textBox;
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
            string text = "To brake, take your foot off the pedal. Try stopping just before the crosswalk ahead.";

            textBox.GetComponent<GameText>().PrintMessage(text);
        }
    }

}
