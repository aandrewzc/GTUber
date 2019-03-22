using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialEnd : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene("RaceArea01");
    }
}
