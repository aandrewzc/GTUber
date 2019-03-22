using UnityEngine;

public class TrafficLightInit : MonoBehaviour {
    
    private void Start () 
    {
        StartCoroutine(GetComponent<TrafficLightControl>().LightSequence());
    }
}
