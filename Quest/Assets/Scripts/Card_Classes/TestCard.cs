using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestCard : Card {
    // member variables
    public int minimum {get; set;}

    // member functions

    public TestCard(string cardType, string cardName, string texture, int min){
        type         = cardType;
        name         = cardName;
        texturePath  = texture;
        minimum      = min;
    }

    public int getMinimum(){
        return this.minimum;
    }
}
