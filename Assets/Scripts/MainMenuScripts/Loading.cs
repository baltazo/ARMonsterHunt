using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour {

    public GameObject playButton;
    public GameObject loadingImg;

	public void StartLoading()
    {
        playButton.SetActive(false);
        loadingImg.SetActive(true);
    }
}
