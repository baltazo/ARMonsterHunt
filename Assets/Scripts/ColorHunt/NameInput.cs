using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NameInput : MonoBehaviour {

    public ColorPicker colorPicker;

    private InputField input;


	// Use this for initialization
	void Start () {
        input = gameObject.GetComponent<InputField>();
        EndInput();
	}
	
	private void SubmitName(string name)
    {

        if (String.IsNullOrEmpty(input.text) || input.text.Trim().Length == 0)
        {
            return;
        }

        colorPicker.SetNameAndAddToList(name);
        gameObject.GetComponent<InputField>().text = "";
    }

    public void EndInput()
    {

        var submitEvent = new InputField.SubmitEvent();

        submitEvent.AddListener(SubmitName);
        input.onEndEdit = submitEvent;
    }
}
