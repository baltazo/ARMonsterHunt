using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageButtons : MonoBehaviour {

    public Button frenchBtn;
    public Button englishBtn;

    public void ButtonClicked(string language)
    {
        if (language == "f")
        {
            frenchBtn.interactable = false;
            englishBtn.interactable = true;
        }
        else if (language == "e")
        {
            frenchBtn.interactable = true;
            englishBtn.interactable = false;
        }
    }

}
