using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BooleanPrompt
{
    //for boolean user check
    public GameObject booleanPanel;
    public Text userMessage;
    public Button yesButton;
    public Button noButton;
    public string buttonResult;

    public bool isActive;

    //Constructor
    public void Setup()
    {
        //Setup UI
        isActive = false;
        buttonResult = "";
        yesButton.onClick.AddListener(()=> buttonResult = "Yes");
        noButton.onClick.AddListener(()=> buttonResult = "No");
    }
    
    //Callbacks
    void Yes() { buttonResult = "Yes"; }
    void No() { buttonResult = "No"; }

    //Activate panel
    public void Activate(string message)
    {
        booleanPanel.SetActive(true);
        isActive = true;
        userMessage.text = message;
        buttonResult = "";
    }

    //Deactivate Panel
    public void Deactivate()
    {
        booleanPanel.SetActive(false);
        isActive = false;
        userMessage.text = "";
        buttonResult = "";
    }



}
