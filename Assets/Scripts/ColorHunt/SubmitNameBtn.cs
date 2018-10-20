using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SubmitNameBtn : MonoBehaviour {

    public NameInput nameInput;
    public InputField inputField;

    private Button button;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
    }

    private void Update()
    {
        if(String.IsNullOrEmpty(inputField.text) || inputField.text.Trim().Length == 0)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    public void Submit()
    {
        nameInput.EndInput();
    }

}
