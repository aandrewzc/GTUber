using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffRoadStartText : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<GameText>().PrintMessage("Drive along this road without going off the sides.");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
