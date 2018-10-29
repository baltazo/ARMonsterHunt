using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardTimer : MonoBehaviour {

    // UI
    //public Text timeLabel; // only use if timer has a label
    public Button timerButton; // used to enbable / disable button when needed
    public Image _progress;

    // Time Elements
    public int hours; // To set the hours
    public int minutes; // To set the minutes
    public int seconds; // To set the seconds
    private bool _timerComplete = false;
    private bool _timerIsReady;
    private TimeSpan _startTime;
    private TimeSpan _endTime;
    private TimeSpan _remainingTime;

    // Progress filter
    private float _value = 1f;

    // Reward to claim
    public int rewardToEarn;

	// Use this for initialization
	void Start () {
        if (PlayerPrefs.GetString("_timer") == "")
        {
            Debug.Log("==> Enableing button");
            EnableButton();
        }
        else
        {
            DisableButton();
            StartCoroutine(CheckTime());
        }
	}
	

    // Update the time information with what we got on the internet
    private void UpdateTime()
    {
        if(PlayerPrefs.GetString("_timer") == "Standby")
        {
            PlayerPrefs.SetString("_timer", TimeManager.sharedInstance.GetCurrentTime());
            PlayerPrefs.SetInt("_date", TimeManager.sharedInstance.GetCurrentDateNow());
        }
        else if(PlayerPrefs.GetString("_timer")!="" && PlayerPrefs.GetString("_timer") != "Standby")
        {
            int _old = PlayerPrefs.GetInt("_date");
            int _now = TimeManager.sharedInstance.GetCurrentDateNow();

            // Check if a day has passed
            if(_now > _old)
            {
                Debug.Log("Day has passed");
                EnableButton();
                return;
            } 
            else if(_now == _old)
            {
                Debug.Log("Same Day - Configuring Now");
                ConfigTimerSettings();
                return;
            }
            else
            {
                Debug.Log("Error with date");
                return;
            }
        }
        Debug.Log("Day has passed - Configuring Now");
        ConfigTimerSettings();
    }

    //Setting up and configuring values
    //Update the time info from the internet
    private void ConfigTimerSettings()
    {
        _startTime = TimeSpan.Parse(PlayerPrefs.GetString("_timer"));
        _endTime = TimeSpan.Parse(hours + ":" + minutes + ":" + seconds);
        TimeSpan temp = TimeSpan.Parse(TimeManager.sharedInstance.GetCurrentTime());
        TimeSpan diff = temp.Subtract(_startTime);
        _remainingTime = _endTime.Subtract(diff);
        // Start timer where we left off
        SetProgressWhereWeLeftOff();

        if(diff >= _endTime)
        {
            _timerComplete = true;
            EnableButton();
        }
        else
        {
            _timerComplete = false;
            DisableButton();
            _timerIsReady = true;
        }
    }

    private void SetProgressWhereWeLeftOff()
    {
        float ah = 1f / (float)_endTime.TotalSeconds;
        float bh = 1f / (float)_remainingTime.TotalSeconds;
        _value = ah / bh;
        if(timerButton != null) { _progress.fillAmount = _value; }
    }

	// Enable button
    private void EnableButton()
    {
            timerButton.interactable = true;
            //timeLabel.text = "CLAIM REWARD";
    }

    // Disable button
    private void DisableButton()
    {
            timerButton.interactable = false;
            //timeLabel.text = "NOT READY";
    }

    // Use to check the current time before completing any task. Use to validate
    private IEnumerator CheckTime()
    {
        DisableButton();
        //timeLabel.text = "Checking the time...";
        Debug.Log("===> Checking for new time");
        yield return StartCoroutine(TimeManager.sharedInstance.GetTime());
        UpdateTime();
        Debug.Log("===> Time check complete");
    }

    public void RewardClicked()
    {
        Debug.Log("===> Claim Button Clicked");
        ClaimReward();
        PlayerPrefs.SetString("_timer", "Standby");
        StartCoroutine(CheckTime());
    }

    // Update to make progres tick
    private void Update()
    {
        if (_timerIsReady)
        {
            if(!_timerComplete && PlayerPrefs.GetString("_timer") != "")
            {
                _value -= Time.deltaTime * 1f / (float)_endTime.TotalSeconds;
                _progress.fillAmount = _value;

                //This is called once only
                if(_value <= 0 && !_timerComplete)
                {
                    // When the timer hits 0, do a validation to make sure no speed hack
                    ValidateTime();
                    _timerComplete = true;
                }
            }
        }
    }

    // Validator
    private void ValidateTime()
    {
        Debug.Log("==> Validating time to make sure no speed hack");
        StartCoroutine(CheckTime());
    }

    private void ClaimReward()
    {
        Debug.Log("RewardClaimed");
    }


}
