using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOL : MonoBehaviour {

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        StartCoroutine(TitleScreen()); 
    }

    IEnumerator TitleScreen()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Tutorial");

    }
}
