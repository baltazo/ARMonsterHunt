using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour {

    public static LocalizationManager sharedInstance;

    public Dictionary<string, string> localizedText;

    public bool isFrench = false;

    private bool isReady = false;
    private string missingTextString = "Localized Text Not Found";

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (sharedInstance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadLocalizedText("localizedText_en.json");
    }

    public void LoadLocalizedText(string fileName)
    {
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string dataAsJson;


        if (File.Exists(filePath))
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                WWW reader = new WWW(filePath);
                while (!reader.isDone) { }

                dataAsJson = reader.text;
            }
            else
            {
                dataAsJson = File.ReadAllText(filePath);
            }

            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            Debug.Log("Data loaded, dictionnary contains: " + localizedText.Count + " entries");
        }
        else
        {
            Debug.LogError("Cannot find localized text file");
        }

        isReady = true;
    }

    public void SetLanguage(bool french)
    {
        isFrench = french;
    }

    public string GetLocalizedValue(string key)
    {
        string result = missingTextString;

        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }

        return result;
    }

    public bool GetIsReady()
    {
        return isReady;
    }

}
