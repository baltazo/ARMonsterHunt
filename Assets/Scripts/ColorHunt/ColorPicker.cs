using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour {

    public Text colorStatusText; // Temporary text to give the status of the recent color
    public Image colorScanned; // This is temporary, to find which color has been scanned

    public float secondsToClearScan;
    public float averageTreshold = 0.05f;
    public float chanceToGetAMonster;

    public GameObject summonPanel;
    public Text summonedMonsterText;
    public Image summonedMonsterImage;
    public GameObject inputField;
    public Text monsterNameText;

    private Sprite monsterImage;
    private List<Color> recentColors = new List<Color>();
    private GameObject foundMonster;
    

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

        Debug.Log("RecentColor count: " + recentColors.Count);

        if (recentColors.Count != 0)
        {
            foreach (Color color in recentColors) // This checks the scanned color against the recently scanned ones by substracting each color and checking if the difference is greater than 0.015f
            {
                float r = Mathf.Abs(scannedColor.r - color.r);
                float g = Mathf.Abs(scannedColor.g - color.g);
                float b = Mathf.Abs(scannedColor.b - color.b);

                float average = (r + g + b) / 3;

                Debug.Log("Average is " + average);

                if(average < averageTreshold)
                {
                    colorStatusText.text = "Color already scanned";
                    Debug.Log("Color too similar to a previously scanned color");
                    return;
                }
            }
        }

        Debug.Log("It is a new Color! Summon a monster or an item and add to the recen color list");
        recentColors.Add(scannedColor);
        colorStatusText.text = "New color! Added to list";
        PlayerPrefs.SetFloat("_recordedColorR", scannedColor.r);
        PlayerPrefs.SetFloat("_recordedColorG", scannedColor.g);
        PlayerPrefs.SetFloat("_recordedColorB", scannedColor.b);
        GetScanReward(scannedColor);
    }

    private void GetScanReward(Color scannedColor)
    {
        if(UnityEngine.Random.Range(0, 1f) <= chanceToGetAMonster)
        {
            //Get a monster
            GameObject monsterSummoned = MonsterCollector.sharedInstance.MonsterChooser(scannedColor);
            foundMonster = monsterSummoned;
            summonedMonsterText.text = monsterSummoned.name;

            foreach (Sprite image in MonsterCollector.sharedInstance.monstersImages)
            {
                if (monsterSummoned.name == image.name)
                {
                    monsterImage = image;
                    break;
                }
            }

            summonedMonsterImage.sprite = monsterImage;
            summonPanel.SetActive(true);
            inputField.SetActive(true);
        }
        else
        {
            // Get an item
        }
    }

    public void SetNameAndAddToList(string name)
    {
        Debug.Log("Name entered: " + name);
        monsterNameText.text = name;
        MonsterCollector.sharedInstance.AddMonsterToList(foundMonster, name);
        inputField.SetActive(false);
    }

    private void CheckTimeSinceFirstScan()
    {
        if (!PlayerPrefs.HasKey("_colorTimer")) //If this is the first time, record the time and stop there.
        {
            Debug.Log("==> This is the first time recording a color");
            PlayerPrefs.SetFloat("_colorTimer", Time.realtimeSinceStartup);
            return;
        }

        if(PlayerPrefs.GetFloat("_colortimer") > Time.realtimeSinceStartup) // If the recorded time is higher the the current time, it means the game has restarted, so record again and stop there
        {
            Debug.Log("The recorded time is bigger than the actual time, set a new time");
            PlayerPrefs.SetFloat("_colorTimer", Time.realtimeSinceStartup);
            return;
        }

        float currentTime = Time.realtimeSinceStartup;

        if(currentTime - PlayerPrefs.GetFloat("_colorTimer") >= secondsToClearScan) // If more time has passed than the secondsToClearScan, clear the list and set a new time
        {
            Debug.Log("It has been more than " + secondsToClearScan + " seconds since first scan, clearing list");
            recentColors.Clear();
            PlayerPrefs.SetFloat("_colorTimer", currentTime);
            return;
        }

        if(recentColors.Count == 0) // It has not been 5 minutes, but the list is empty. Add the last recorded color.
        {
            Debug.Log("It has been less than 5 minutes, but the list is empty. Adding the last recorded color");
            float r = PlayerPrefs.GetFloat("_recordedColorR");
            float g = PlayerPrefs.GetFloat("_recordedColorG");
            float b = PlayerPrefs.GetFloat("_recordedColorB");

            Color lastRecordedColor = new Color(r, g, b, 1f);
            recentColors.Add(lastRecordedColor);
        }
        Debug.Log("The recent color list is still active");
    }

    public void ScanAgain()
    {
        summonPanel.SetActive(false);
    }
}
