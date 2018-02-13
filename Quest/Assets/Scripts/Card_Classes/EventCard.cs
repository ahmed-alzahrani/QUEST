using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventCard : Card
{
    // member variables
    public string description { get; set; }
    public iStory effect;


    // member functions

    public EventCard(string cardType, string cardName, string cardDescription, iStory cardEvent)
    {
        type = cardType;
        name = cardName;
        description = cardDescription;
        effect = cardEvent;
    }

    public new void display()
    {
        Debug.Log("Card Type: " + type);
        Debug.Log("Card Name: " + name);
        Debug.Log("Card Description: " + description);
    }
}
