using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventCard : Card {
    // member variables

    private string description;

    // member functions

    public EventCard(string cardType, string cardName, string cardDescription){
        type        = cardType;
        name        = cardName;
        description = cardDescription;
    }

    public new void display(){
      Debug.Log("Card Type: " + type);
      Debug.Log("Card Name: " + name);
      Debug.Log("Card Description: " + description);
    }
}
