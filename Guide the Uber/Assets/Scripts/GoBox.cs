using UnityEngine;

public class GoBox : MonoBehaviour {

    private bool stopped;
    private GameObject textBox;

	private void Start () 
    {
        textBox = GameObject.Find("/Canvas/TextBox");
    }

    public void CarStopped()
    {
        stopped = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Uber")
        {
            if (stopped)
            {
                stopped = false;
            }
            else
            {
                // tally a missed stop sign
                textBox.GetComponent<GameText>().PrintMessage("You ran a stop sign!!");
                textBox.GetComponent<GameText>().Score(-0.5f);
            }
        }
    }
}
