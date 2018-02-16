using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FoeCard : Card {
    // member variables
    public int minBP {get; set;}
    public int maxBP {get; set;}

    // member functions
    protected FoeCard() {}

    public FoeCard(string cardType, string cardName, string texture, int min, int max) {
        type        = cardType;
        name        = cardName;
        texturePath = texture;
        minBP       = min;
        maxBP       = max;
    }

    public int getMinBP(){
        return this.minBP;
    }

    public int getMaxBP(){
        return this.maxBP;
    }
}
