using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficStartText : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<GameText>().PrintMessage("Follow the yellow path to the destination to practice traffic lights");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
