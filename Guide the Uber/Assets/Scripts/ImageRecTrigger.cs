using System.Collections;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.SceneManagement;

public class ImageRecTrigger : MonoBehaviour {

    private GameObject udp;
    private GameObject textBox;
    public GameObject police;

    public bool verified;
    public bool retry;

    public bool pulledOver = false;

    private void Start()
    {
        udp = GameObject.Find("_udp");
        textBox = GameObject.Find("Canvas/TextBox");
        police = GameObject.Find("Police");
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");

        if (collision.transform.tag == "Police" && !pulledOver)
        {
            // disable car controls 
            //GetComponent<CarUserControl>().enabled = false;
            udp.GetComponent<UnityInputManager>().stopped = true;

            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            StartCoroutine(PulledOver());
            pulledOver = true;
        }
        else if (collision.transform.tag == "AI" && !pulledOver)
        {
            Debug.Log("AI car collision");
            textBox.GetComponent<GameText>().PrintMessage("Don't crash into other cars!");

            PoliceChase();
        }
    }

    public void PoliceChase()
    {
        Transform target = GameObject.Find("Car/PoliceTarget").transform;
        police.transform.rotation = new Quaternion(target.rotation.x, target.rotation.y, target.rotation.z, target.rotation.w);
        police.transform.position = new Vector3(target.position.x, target.position.y, target.position.z);
        police.GetComponent<CarAIControl>().SetTarget(transform);
    }
    

    public IEnumerator PulledOver()
    {

        // inform user of sobriety test
        string text = "You've been pulled over for a sobriety test. For calibration, ensure that the green box on your shirt completely fills the blue box on the screen.";
        textBox.GetComponent<GameText>().PrintMessage(text, true);

        while (!textBox.GetComponent<GameText>().done)
        {
            yield return new WaitForSeconds(0.1f);
        }

        text = "Then for the next test, walk away from the camera in a straight line until prompted to walk forward.";
        textBox.GetComponent<GameText>().PrintMessage(text, true);

        // image test (send police message to python)
        udp.GetComponent<UnityInputManager>().SendUDP("police");


        //***********************
        //testing only
        //verified = true;
        //***********************

        // wait for response
        while (!verified) 
        {
            if(retry)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }

        if(verified)
        {
            text = "Drunk Driving Test Passed!";
            textBox.GetComponent<GameText>().PrintMessage(text);
        }
        else
        {
            text = "Drunk Driving Test Failed! Did you have too much to drink?";
            textBox.GetComponent<GameText>().PrintMessage(text);
            textBox.GetComponent<GameText>().Score(-4);
            textBox.GetComponent<ErrorCounter>().imageErrors++;
        }

        verified = false;

        // re-enable controls on the car
        //GetComponent<CarUserControl>().enabled = true;
        udp.GetComponent<UnityInputManager>().stopped = false;
        pulledOver = false;

        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            police.GetComponent<CarAIControl>().SetTarget(police.transform);
        }

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            text = "You've completed the tutorial! Now you can play the full game.";
            textBox.GetComponent<GameText>().PrintMessage(text);
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene("RaceArea01");
        }
    }

}
