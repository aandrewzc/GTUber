using UnityEngine;

public class Tutorial007: MonoBehaviour {

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
            string text = "Stop inside the green pickup box on the right to pick up the passenger.";
            textBox.GetComponent<GameText>().PrintMessage(text);
        }
    }
}
