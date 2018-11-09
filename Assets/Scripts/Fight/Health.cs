using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public Image healthProgress;
    public Sprite[] healthBars;
    private GameObject firstPersonCamera;

    private void Start()
    {
        firstPersonCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update () {
        transform.LookAt(firstPersonCamera.transform);
	}

    public void UpdateHealthBar(int level)
    {


        if (level == 0)
        {
            healthProgress.sprite = healthBars[9];
        }
        else if (level == 10)
        {
            healthProgress.sprite = healthBars[0];
        }
        else
        {
            healthProgress.sprite = healthBars[level];
        }
    }

    public void ToggleHealthDisplay()
    {
        gameObject.SetActive(true);
    }

    public void NoMoreLife()
    {
        healthProgress.enabled = false;
    }
}
