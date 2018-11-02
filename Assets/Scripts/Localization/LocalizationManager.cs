using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

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

        if (Application.platform == RuntimePlatform.Android)
        {

            StartCoroutine(LoadLocalizedTextOnAndroid(fileName));
            return;
        }

         
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName); 
        string dataAsJson;


        if (File.Exists(filePath))
        {

            dataAsJson = File.ReadAllText(filePath);

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

    private IEnumerator LoadLocalizedTextOnAndroid(string fileName)
    {
        string filePath;

        filePath = Path.Combine(Application.streamingAssetsPath + Path.DirectorySeparatorChar, fileName);

        string dataAsJson;

        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();

            dataAsJson = www.downloadHandler.text;
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
