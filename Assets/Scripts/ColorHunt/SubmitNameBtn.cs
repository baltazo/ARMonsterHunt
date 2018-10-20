using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitNameBtn : MonoBehaviour {

    public NameInput nameInput;

    public void Submit()
    {
        nameInput.EndInput();
    }

}
