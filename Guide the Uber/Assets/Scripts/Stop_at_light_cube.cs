using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class Stop_at_light_cube : MonoBehaviour {

    public GameObject redLight;

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log("collision of" + other);
        if (redLight.GetComponent<Light>().enabled && other.tag == "AI")
        {
            // Debug.Log("car stopped");
            other.transform.parent.transform.parent.GetComponent<CarAIControl>().m_Driving = false;
        }
    }

}
