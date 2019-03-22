using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameText : MonoBehaviour {

    private GameObject textBox;
    private GameObject scoreBox;

    public GameObject warningText;

    public bool done;

    private void Awake() 
    {
        done = false;
        textBox = GameObject.Find("Canvas/TextBox/Message");
        scoreBox = GameObject.Find("Canvas/ScoreBox");
    }

    public void DisplayMessage(string message)
    {
        textBox.GetComponent<Text>().text = message;
    }

    public void PrintMessage(string message, bool priority = false)
    {
        if (!priority && !done)
        {
            return;
        }
        StartCoroutine(Print(message));
    }

    IEnumerator Print(string message)
    {
        done = false;
        for (int i = 0; i <= message.Length; i++)
        {
            textBox.GetComponent<Text>().text = message.Substring(0, i);
            if (i > 0 && message[i-1] == '.')
            {
                yield return new WaitForSeconds(0.1f);
            }
            else if (i > 0 && message[i - 1] == ' ')
            {
                yield return new WaitForSeconds(0.02f);
            }
            else
            {
                yield return new WaitForSeconds(0.005f);
            }
        }
        yield return new WaitForSeconds(1.5f);
        done = true;
    }

    public void Score(float points)
    {
        scoreBox.GetComponent<ScoreManager>().ChangeScore(points);
    }
}
