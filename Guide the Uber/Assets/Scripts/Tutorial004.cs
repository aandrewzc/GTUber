using UnityEngine;

public class Tutorial004 : MonoBehaviour {

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
            string text = "Nice job. Continue straight, following the curve of the road.";
            textBox.GetComponent<GameText>().PrintMessage(text);
            textBox.GetComponent<GameText>().Score(1.5f);
        }
    }

}
