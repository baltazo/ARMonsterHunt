using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocalizedText : MonoBehaviour {

    public string key;

	// Use this for initialization
	void Start () {
        if(GetComponent<TextMeshPro>() != null)
        {
            TextMeshPro textPro = GetComponent<TextMeshPro>();
            textPro.text = LocalizationManager.sharedInstance.GetLocalizedValue(key);
        }
        else
        {
            Text text = GetComponent<Text>();
            text.text = LocalizationManager.sharedInstance.GetLocalizedValue(key);
        }
	}
}
