using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour {

    private GameController gameController;

	void Start () {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	public void BackToRanch()
    {
        gameController.ChangeScene("Ranch");
    }
}
