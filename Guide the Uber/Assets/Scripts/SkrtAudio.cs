using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class SkrtAudio : MonoBehaviour {

    public AudioClip SkrtSkrt;
    private AudioSource source;
    private float steering;

    // Use this for initialization
    void Start () {
        source = gameObject.AddComponent<AudioSource>();
        source.clip = SkrtSkrt;
    }
	
	// Update is called once per frame
	void Update () {
        steering = GetComponent<CarUserControl>().h;

        if (steering >= 1 || steering <= -1)
        {
            source.Play();
        }
    }
}
