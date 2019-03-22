using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RideTimer : MonoBehaviour {

    private GameObject timer;

    public static int MinuteCount;
    public static int SecondCount;
    public static float MilliCount;
    public static string MilliDisplay;

    private GameObject MinuteBox;
    private GameObject SecondBox;
    private GameObject MilliBox;

    private bool active;
    private int totalMilli;

    private void Start()
    {
        timer = GameObject.Find("Canvas/Timer");
        MinuteBox = GameObject.Find("Canvas/Timer/MinuteBox");
        SecondBox = GameObject.Find("Canvas/Timer/SecondBox");
        MilliBox = GameObject.Find("Canvas/Timer/MilliBox");

        timer.SetActive(false);
    }

    public bool Timing()
    {
        return active;
    }

    public void StartTimer()
    {
        active = true;
        timer.SetActive(true);
    }

    public int StopTimer ()
    {
        active = false;
        timer.SetActive(false);

        MinuteCount = 0;
        SecondCount = 0;
        MilliCount = 0;

        return totalMilli;
    }

    void Update()
    {
        if (active)
        {
            MilliCount += Time.deltaTime * 10;
            MilliDisplay = MilliCount.ToString("F0");
            MilliBox.GetComponent<Text>().text = "" + MilliDisplay;

            if (MilliCount >= 10)
            {
                MilliCount = 0;
                SecondCount += 1;
            }

            if (SecondCount <= 9)
            {
                SecondBox.GetComponent<Text>().text = "0" + SecondCount + ".";
            }
            else
            {
                SecondBox.GetComponent<Text>().text = "" + SecondCount + ".";
            }

            if (SecondCount >= 60)
            {
                SecondCount = 0;
                MinuteCount += 1;
            }

            if (MinuteCount <= 9)
            {
                MinuteBox.GetComponent<Text>().text = "0" + MinuteCount + ":";
            }
            else
            {
                MinuteBox.GetComponent<Text>().text = "" + MinuteCount + ":";
            }

            totalMilli = (MinuteCount * 600) + (SecondCount * 10) + Mathf.RoundToInt(MilliCount);
        }
    }
}
