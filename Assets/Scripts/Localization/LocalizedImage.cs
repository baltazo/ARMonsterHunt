using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedImage : MonoBehaviour {

    public Sprite frenchImage;
    public Sprite englishImage;

    public void Start()
    {
        if (LocalizationManager.sharedInstance.isFrench)
        {
            GetComponent<Image>().sprite = frenchImage;
        }
        else
        {
            GetComponent<Image>().sprite = englishImage;
        }
    }


}
