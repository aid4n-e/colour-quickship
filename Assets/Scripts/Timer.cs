using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    float timeLeft = 60;
    public bool timerOn = false;

    public TextMesh timerText;
    public RoundGenerator roundGenerator;

    void Update()
    {
        if(timerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimer(timeLeft);
            }
            if (timeLeft <= 0)
            {
                roundGenerator.StartCoroutine(roundGenerator.Failure());
                roundGenerator.state = -2;
                timerOn = false;
            }
        }


    }


    void UpdateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:0} : {1:00}", minutes, seconds);
    }

    public void ResetTimer(int newTimeLeft)
    {
        timeLeft = newTimeLeft;
        timerOn = false;
    }
}
