using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

abstract class Card
{
    // member variables
    protected string type { get; set; }
    protected string name { get; set; }

    // member functions

    protected Card() {}

    // 2 param constructor
    public Card(string cardType, string cardName)
    {
        type = cardType;
        name = cardName;
    }

    /*
    public string getType(){
        return this.type;
    }

    public string getName(){
        return this.name;
    }
    */

    public void display()
    {
        Debug.Log("Card Type: " + type);
        Debug.Log("Card Name: " + name);
    }
}
