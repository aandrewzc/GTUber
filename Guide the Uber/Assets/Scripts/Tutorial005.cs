using UnityEngine;

public class Tutorial005 : MonoBehaviour {

    private GameObject textBox;
    public GameObject cones;
    public GameObject nextTrigger;
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
            cones.SetActive(false);
            nextTrigger.SetActive(true);

            string text = "Looks like this street is blocked. " +
            	"To toggle reverse, tilt the wheel 90 degrees towards you, then back to upright position. " +
            	"A reverse message will appear in the speedometer.";
            textBox.GetComponent<GameText>().PrintMessage(text);
        }
    }

}
