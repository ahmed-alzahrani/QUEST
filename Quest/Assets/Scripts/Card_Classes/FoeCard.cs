using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class FoeCard : Card
{
    // member variables

    protected int minBP { get; set; }
    protected int maxBP { get; set; }

    // member functions

    protected FoeCard() {}

    public FoeCard(string cardType, string cardName, int min, int max)
    {
        type        = cardType;
        name        = cardName;
        minBP       = min;
        maxBP       = max;
    }

    /*
    public int getMinBP(){
        return this.minBP;
    }

    public int getMaxBP(){
        return this.maxBP;
    }
    */
}