using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class KeyboardInputPrompt
{
    // keyboard input
    public GameObject inputPanel;
    public Text userMessage;
    public InputField KeyboardInput;

    public bool isActive;

    //Activate panel
    public void Activate(string message)
    {
        inputPanel.SetActive(true);
        userMessage.text = message;
        KeyboardInput.text = "";
        isActive = true;
    }

    //Deactivate panel
    public void Deactivate()
    {
        inputPanel.SetActive(false);
        userMessage.text = "";
        KeyboardInput.text = "";
        isActive = false;
    }

}
