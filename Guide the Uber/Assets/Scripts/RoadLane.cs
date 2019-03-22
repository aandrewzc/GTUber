using UnityEngine;

public class RoadLane : MonoBehaviour {

    private GameObject textBox;
    private float time;
    private bool active;

    private void Start()
    {
        textBox = GameObject.Find("/Canvas/TextBox");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Uber")
        {
            StartTime();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Uber")
        {
            int lane_time = StopTime();
            textBox.GetComponent<ErrorCounter>().laneTime += lane_time;
            
        }
    }

    private void Update()
    {
        if (active)
        {
            time += Time.deltaTime;
        }
    }

    private void StartTime()
    {
        active = true;
    }

    private int StopTime()
    {
        active = false;

        int result = Mathf.FloorToInt(time);
        time = 0;

        return result;
    }
}
