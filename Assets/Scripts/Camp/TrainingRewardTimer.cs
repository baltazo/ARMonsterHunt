using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TrainingRewardTimer : MonoBehaviour {

    public Training training;

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


    private void Start()
    {
        if (PlayerPrefs.HasKey("_trainingTimer"))
        {
            DisableButton();
            StartCoroutine(CheckTime());
        }
    }
    // Update to make progres tick
    private void Update()
    {
        if (_timerIsReady)
        {
            if (!_timerComplete && PlayerPrefs.GetString("_trainingTimer") != "")
            {
                _value -= Time.deltaTime * 1f / (float)_endTime.TotalSeconds;
                _progress.fillAmount = _value;

                //This is called once only
                if (_value <= 0 && !_timerComplete)
                {
                    // When the timer hits 0, do a validation to make sure no speed hack
                    ValidateTime();
                    _timerComplete = true;
                }
            }
        }
    }

    public void StartTimer()
    {
        PlayerPrefs.SetString("_trainingTimer", TimeManager.sharedInstance.GetCurrentTime());
        PlayerPrefs.SetInt("_trainingDate", TimeManager.sharedInstance.GetCurrentDateNow());
        StartCoroutine(CheckTime());
    }

    // Use to check the current time before completing any task. Use to validate
    private IEnumerator CheckTime()
    {
        DisableButton();
        yield return StartCoroutine(TimeManager.sharedInstance.GetTime());
        UpdateTime();
    }

    // Update the time information with what we got on the internet
    private void UpdateTime()
    {
        if (PlayerPrefs.GetString("_trainingTimer") != "" && PlayerPrefs.GetString("_trainingTimer") != "Standby")
        {
            int _old = PlayerPrefs.GetInt("_trainingDate");
            int _now = TimeManager.sharedInstance.GetCurrentDateNow();

            // Check if a day has passed
            if (_now > _old)
            {
                EnableButton();
                return;
            }
            else if (_now == _old)
            {
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

    private void ConfigTimerSettings()
    {
        _startTime = TimeSpan.Parse(PlayerPrefs.GetString("_trainingTimer"));
        _endTime = TimeSpan.Parse(hours + ":" + minutes + ":" + seconds);
        TimeSpan temp = TimeSpan.Parse(TimeManager.sharedInstance.GetCurrentTime());
        TimeSpan diff = temp.Subtract(_startTime);
        _remainingTime = _endTime.Subtract(diff);
        // Start timer where we left off
        SetProgressWhereWeLeftOff();

        if (diff >= _endTime)
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
        if (timerButton != null) { _progress.fillAmount = _value; }
    }

    private void DisableButton()
    {
        timerButton.interactable = false;
    }

    private void EnableButton()
    {
        timerButton.interactable = true;
    }

    public void RewardClicked()
    {
        PlayerPrefs.SetString("_trainingTimer", "Standby");
        StartCoroutine(CheckTime());
        Debug.Log("===> Claim Button Clicked");
        ClaimReward();
    }

    // Validator
    private void ValidateTime()
    {
        Debug.Log("==> Validating time to make sure no speed hack");
        StartCoroutine(CheckTime());
    }

    private void ClaimReward()
    {
        training.EndTraining();
        _timerIsReady = false;
        PlayerPrefs.DeleteKey("_trainingTimer");
        PlayerPrefs.DeleteKey("_trainingDate");
        gameObject.SetActive(false);
    }

}
