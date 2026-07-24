using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class CountdownManager : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;
    [SerializeField] float countdownDurationInSeconds;
    private float startTime, endTime;
    void Start()
    {
        startTime = Time.time;
        endTime = startTime + countdownDurationInSeconds;
        StartCoroutine(UpdateTimerTextOnInterval());
    }

    public void UpdateTimerText()
    {
        TimeSpan timeLeft = TimeSpan.FromSeconds(endTime - Time.time);
        string timeLeftString = timeLeft.ToString();
        string timeLeftDebug = timeLeftString[3].ToString() + timeLeftString[4] + "   " + timeLeftString[6] + timeLeftString[7];
        Debug.Log(timeLeftDebug);
        string timeMinutes = timeLeft.Minutes.ToString();
        string timeSeconds = timeLeft.Seconds.ToString();
        string timerString = "";
        if (timeLeftString[3] == '0')
        {
            timerString = "  ";
        }
        else
        {
            timerString = "<sprite index=[" + timeLeftString[3] + "]>";
        }
        timerString += "<sprite index=[" + timeLeftString[4] + "]>   <sprite index=[" + timeLeftString[6] + "]><sprite index=[" + timeLeftString[7] + "]>";
        timerText.text = timerString;
    }

    public IEnumerator UpdateTimerTextOnInterval()
    {
        while (Time.time < endTime)
        {
            UpdateTimerText();
            yield return new WaitForSeconds(1);
        }
        //time is up! Do apocalypse stuff here
    }

}
