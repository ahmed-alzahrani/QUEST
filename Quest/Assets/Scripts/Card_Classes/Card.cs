using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

abstract public class Card
{
    // member variables
    public string type { get; set; }
    public string name { get; set; }
    public string texturePath { get; set; }


    // member functions

    protected Card() { }

    // 2 param constructor
    public Card(string cardType, string cardName, string texture)
    {
        type = cardType;
        name = cardName;
        texturePath = texture;
    }

    public string getType()
    {
        return this.type;
    }

    public string getName()
    {
        return this.name;
    }

    public void display()
    {
        Debug.Log("Card Type: " + type);
        Debug.Log("Card Name: " + name);
    }
}
