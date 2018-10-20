using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public static TimeManager sharedInstance = null;
    private string _url = "https://www.arfunstuff.com/arfh/timer.php";
    private string _timedata;
    private string _currentTime;
    private string _currentDate;

    // Make sure there is only one instance of this

    private void Awake()
    {

        if(sharedInstance == null)
        {
            sharedInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(sharedInstance != this)
        {
            Destroy(gameObject);
        }
    }

    //time fether coroutine
    public IEnumerator GetTime()
    {
        Debug.Log("==> step 1. Getting info from internet now!");
        WWW www = new WWW(_url);
        yield return www;
        Debug.Log("==> step2. Got info from the internet!");
        _timedata = www.text;
        string[] words = _timedata.Split('/');
        //timerTestLabel.text = www.text;
        Debug.Log("The date is : " + words[0]);
        Debug.Log("The time is : " + words[1]);

        //setting current time
        _currentDate = words[0];
        _currentTime = words[1];
    }

    private void Start()
    {
        Debug.Log("==> TimeManager script is ready");
        StartCoroutine(GetTime());
    }

    //get the current Date
    public int GetCurrentDateNow()
    {
        string[] words = _currentDate.Split('-');
        int x = int.Parse(words[0] + words[1] + words[2]);
        return x;
    }

    //get the current Time
    public string GetCurrentTime()
    {
        return _currentTime;
    }

}
