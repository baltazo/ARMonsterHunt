using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour {

    public Image colorScanned; // This is temporary, to find which color has been scanned
    public int minutesToNewScan;

    private List<Color> recentColors = new List<Color>();
    private TimeSpan _startTime;
    private TimeSpan _endTime;

    public void GetColor()
    {
        StartCoroutine(RecordColor());
    }

    IEnumerator RecordColor()
    {
        yield return new WaitForEndOfFrame();
        Texture2D screenShot = ScreenCapture.CaptureScreenshotAsTexture(); // Takes a screenshot of the screen,

        Color color = screenShot.GetPixel(Screen.width / 2, Screen.height / 2); // Gets the color of the center pixel

        colorScanned.color = color;
        CheckColor(color);

        Debug.Log("Color Scanned : " + color);

        UnityEngine.Object.Destroy(screenShot);

    }

    private void CheckColor(Color scannedColor)
    {

        CheckTimeSinceFirstScan();

        if (recentColors.Count != 0)
        {
            foreach (Color color in recentColors) // This checks the scanned color against the recently scanned ones by substracting each color and checking if the difference is greater than 0.015f
            {
                float r = Mathf.Abs(scannedColor.r - color.r);
                float g = Mathf.Abs(scannedColor.g - color.g);
                float b = Mathf.Abs(scannedColor.b - color.b);

                float average = (r + g + b) / 3;

                Debug.Log("Average is " + average);

                if(average < 0.015)
                {
                    Debug.Log("Color too similar to a previously scanned color");
                    return;
                }
            }
        }

        Debug.Log("It is a new Color! Summon a monster or an item and add to the recen color list");
        recentColors.Add(scannedColor);
        
    }

    //Setting up and configuring values
    //Update the time info from the internet
    private void CheckTimeSinceFirstScan()
    {
        int _now = TimeManager.sharedInstance.GetCurrentDateNow();

        if (PlayerPrefs.GetInt("_colorDate") != _now)
        { // If the last recorded dat isn't today, assume at least a day passed and clear the list
            recentColors.Clear();
            recentColors.TrimExcess();
            PlayerPrefs.SetInt("_colorDate", _now);
            return;
        }

        // If a day didn't pass, check how long it has been since first scan, if longer than X minutes, clear the list
        if (PlayerPrefs.HasKey("_colorTimer"))
        {
            _startTime = TimeSpan.Parse(PlayerPrefs.GetString("_colorTimer"));
            _endTime = TimeSpan.Parse(0 + ":" + minutesToNewScan + ":" + 0);
            TimeSpan temp = TimeSpan.Parse(TimeManager.sharedInstance.GetCurrentTime());
            TimeSpan diff = temp.Subtract(_startTime);

            if (diff >= _endTime)
            {
                recentColors.Clear();
                Debug.Log(minutesToNewScan + " minutes have passed, clearing list");
                PlayerPrefs.SetString("_colorTimer", TimeManager.sharedInstance.GetCurrentTime());
            }
        }
        else
        {
            PlayerPrefs.SetString("_colorTimer", TimeManager.sharedInstance.GetCurrentTime());
        }
        
    }
}
