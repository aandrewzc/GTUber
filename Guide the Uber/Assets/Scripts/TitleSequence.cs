using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSequence : MonoBehaviour {

    public RawImage logo;
    public Button btnTutorial;
    public Button btnGame;
    public Button btnTest;

    private float alpha;
    private float increment;
    private float waitTime;
    private Color orig;

    private void Start()
    {
        alpha = 0;
        increment = 0.02f;
        waitTime = 0.0001f;
        orig = logo.color;

        btnTutorial.onClick.AddListener(StartTutorial);
        btnGame.onClick.AddListener(StartGame);
        btnTest.onClick.AddListener(StartTestScene);

        StartCoroutine(TitleScreen());
    }

    IEnumerator TitleScreen()
    {
        while (alpha < 1)
        {
            alpha += increment;
            logo.color = new Color(orig.r, orig.g, orig.b, alpha);
            yield return new WaitForSeconds(waitTime);
        }

        btnTutorial.gameObject.SetActive(true);
        btnGame.gameObject.SetActive(true);
        //btnTest.gameObject.SetActive(true);
    }

    public void StartTutorial()
    {
        Debug.Log("button clicked");
        SceneManager.LoadScene("Tutorial");
    }

    public void StartGame()
    {
        Debug.Log("button clicked");
        SceneManager.LoadScene("RaceArea01");
    }
    public void StartTestScene()
    {
        Debug.Log("button clicked");
        SceneManager.LoadScene("OffRoad");
    }
}
