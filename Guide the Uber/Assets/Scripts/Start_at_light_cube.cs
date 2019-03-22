using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class Start_at_light_cube : MonoBehaviour {

    public GameObject greenLight;

    void OnTriggerStay(Collider other)
    {
        // Debug.Log("collision of" + other);
        if (greenLight.GetComponent<Light>().enabled && other.tag == "AI")
        {
            // Debug.Log("car start");
            other.transform.parent.transform.parent.GetComponent<CarAIControl>().m_Driving = true;
        }
    }
}
