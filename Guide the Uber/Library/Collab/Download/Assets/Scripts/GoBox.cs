using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBox : MonoBehaviour {

    private bool stopped;
    private GameObject textBox;
    private GameObject scoreBox;

	// Use this for initialization
	void Start () 
    {
        textBox = GameObject.Find("/Canvas/TextBox/Message");
        scoreBox = GameObject.Find("/Canvas/ScoreBox");
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
                scoreBox.GetComponent<ScoreManager>().ChangeScore(-0.5f);
            }
        }
    }
}
