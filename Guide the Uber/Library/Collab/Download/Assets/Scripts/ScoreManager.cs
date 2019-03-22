using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour {

    public RawImage five;
    public RawImage four; 
    public RawImage three;
    public RawImage two;
    public RawImage one;

    public GameObject gameOver;

    private RawImage[] stars;
    private float[] alpha;

    private float score;
    private float target;
    private float increment;
    private float waitTime;

    // Use this for initialization
    void Start () 
    {
        stars = new RawImage[5] { one, two, three, four, five };
        alpha = new float[5] { one.color.a, two.color.a, three.color.a, four.color.a, five.color.a };

        increment = 0.05f;
        waitTime = 0.0001f;
        score = 5;
	}

    private void Update()
    {
        if (score <= 0)
        {
            StartCoroutine(EndGame());
        
        }
    }

    IEnumerator EndGame()
    {
        gameOver.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Tutorial");
    }

    public void ChangeScore(float change)
    {
        StartCoroutine(ChangeScoreRoutine(change));
    }

    IEnumerator ChangeScoreRoutine(float change)
    {
        // gain points
        if (change > 0)
        {
            target = score + change;
            if (target > 5)
            {
                target = 5;
            }

            // iterate through stars and increase alpha to cover them
            for (int i = 1; i <= 5; i++)
            {
                // decrease score until target reached or current star is hidden
                while (score < i && score < target)
                {
                    alpha[i-1] -= increment;
                    score += increment;
                    stars[i-1].color = new Color(0, 0, 0, alpha[i-1]);
                    yield return new WaitForSeconds(waitTime);
                }
            }

        }
        // lose points
        else
        {
            // compute new score, minimum of 0
            target = score + change;
            if (target < 0)
            {
                target = 0;
            }

            // iterate through stars and increase alpha to cover them
            for (int i = 4; i >= 0; i--)
            {   
                // decrease score until target reached or current star is hidden
                while (score > i && score > target)
                {
                    alpha[i] += increment;
                    score -= increment;
                    stars[i].color = new Color(0, 0, 0, alpha[i]);
                    yield return new WaitForSeconds(waitTime);
                }
            }
        }
    }
}
