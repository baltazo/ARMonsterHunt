using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameInput : MonoBehaviour {

    public ColorPicker colorPicker;

	// Use this for initialization
	void Start () {
        EndInput();
	}
	
	private void SubmitName(string name)
    {
        colorPicker.SetNameAndAddToList(name);
        gameObject.GetComponent<InputField>().text = "";
    }

    public void EndInput()
    {
        var input = gameObject.GetComponent<InputField>();
        var submitEvent = new InputField.SubmitEvent();
        submitEvent.AddListener(SubmitName);
        input.onEndEdit = submitEvent;
    }
}
