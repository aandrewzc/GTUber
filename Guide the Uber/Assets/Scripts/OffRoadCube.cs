using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffRoadCube : MonoBehaviour {

    private GameObject textBox;

    private void Start()
    {
        textBox = GameObject.Find("Canvas/TextBox");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Uber")
        {
            textBox.GetComponent<GameText>().PrintMessage("You went off the road!!");
            textBox.GetComponent<GameText>().Score(-5);
        }
    }
}
