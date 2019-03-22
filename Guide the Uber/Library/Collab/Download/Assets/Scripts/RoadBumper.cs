using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoadBumper : MonoBehaviour {

    private GameObject textBox;
    private GameObject scoreBox;

    void Start()
    {
        textBox = GameObject.Find("/Canvas/TextBox/Message");
        scoreBox = GameObject.Find("/Canvas/ScoreBox");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Uber")
        {
            textBox.GetComponent<Text>().text = "Stay on the road!";
            scoreBox.GetComponent<ScoreManager>().ChangeScore(-0.5f);
        }
    }
}
