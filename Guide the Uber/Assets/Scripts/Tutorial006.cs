using UnityEngine;

public class Tutorial006: MonoBehaviour {

    private GameObject textBox;
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

            string text = "Good job. Toggle back to drive using the same motion as before. Then take a right and find your first passenger to pickup.";
            textBox.GetComponent<GameText>().PrintMessage(text);
            textBox.GetComponent<GameText>().Score(1.5f);
        }
    }

}
