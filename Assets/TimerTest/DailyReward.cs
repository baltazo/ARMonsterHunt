using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour {

    public Button timerButton;
    public Text timeLabel;
    public string startTime;
    public string endTime;

    private double tcounter;
    private TimeSpan eventStartTime;
    private TimeSpan eventEndTime;
    private TimeSpan currentTime;
    private TimeSpan _remainingTime;
    private string timeFormat;
    private bool timerset;
    private bool countIsReady;
    private bool countIsReady2;

	// Use this for initialization
	void Start () {
        eventStartTime = TimeSpan.Parse(startTime);
        eventEndTime = TimeSpan.Parse(endTime);
        StartCoroutine(CheckTime());
	}

    private IEnumerator CheckTime()
    {
        Debug.Log("===> Checking the time");
        timeLabel.text = "Checking the time";
        yield return StartCoroutine(TimeManager.sharedInstance.GetTime());
        UpdateTime();
        Debug.Log("===> Time check completed");
    }

    private void UpdateTime()
    {
        currentTime = TimeSpan.Parse(TimeManager.sharedInstance.GetCurrentTime());
        timerset = true;
    }

	
	// Update is called once per frame
	void Update () {
        if (timerset)
        {
            if(currentTime >= eventStartTime && currentTime <= eventEndTime)
            { // This means the event as already started and players can click and join
                _remainingTime = eventEndTime.Subtract(currentTime);
                tcounter = _remainingTime.TotalMilliseconds;
                countIsReady2 = true;
            }
            else if(currentTime < eventStartTime)
            { // This means the event has not started yet for today
                _remainingTime = eventStartTime.Subtract(currentTime);
                tcounter = _remainingTime.TotalMilliseconds;
                countIsReady = true;
            }
            else
            { //The event has already passed
                DisableButton("Event is over, come back tomorrow");
            }
        }
        
        if(countIsReady)
        {
            StartCountdown();
        }

        if (countIsReady2)
        {
            StartCountdown2();
        }
	}

    // Setup the time format string
    public string GetRemainingTime(double x)
    {
        TimeSpan tempB = TimeSpan.FromMilliseconds(x);
        timeFormat = string.Format("{0:D2}:{1:D2}:{2:D2}", tempB.Hours, tempB.Minutes, tempB.Seconds);
        return timeFormat;
    }

    private void StartCountdown()
    {
        timerset = false;
        tcounter -= Time.deltaTime * 1000;
        DisableButton("Starting Soon: " + GetRemainingTime(tcounter));

        if(tcounter <= 0)
        {
            countIsReady = false;
            ValidateTime();
        }
    }

    private void StartCountdown2()
    {
        timerset = false;
        tcounter -= Time.deltaTime * 1000;
        EnableButton("Event Started! Time Remaining: " + GetRemainingTime(tcounter));

        if(tcounter <= 0)
        {
            countIsReady2 = false;
            ValidateTime();
        }
    }

    // Enable button function
    private void EnableButton(string x)
    {
        timerButton.interactable = true;
        timeLabel.text = x;
    }

    // Disable button function
    private void DisableButton(string x)
    {
        timerButton.interactable = false;
        timeLabel.text = x;
    }

    //Validator
    private void ValidateTime()
    {
        Debug.Log("==> Validating time to make sure ne speed hack!");
        StartCoroutine(CheckTime());
    }

}
