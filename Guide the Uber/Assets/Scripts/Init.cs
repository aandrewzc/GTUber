using UnityEngine;

public class Init : MonoBehaviour {

    private GameObject udp = null;

    private void Awake()
    {
        if (GameObject.Find("_udp") == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("_preload");
        }
        udp = GameObject.Find("_udp");

        if (udp != null)
        {
            udp.GetComponent<UnityInputManager>().userCar = GameObject.Find("/Car");
        }

    }
}
