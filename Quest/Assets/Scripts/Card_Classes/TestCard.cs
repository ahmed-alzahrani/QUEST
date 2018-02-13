using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestCard : Card {
    // member variables
    private int minimum {get; set;}

    // member functions

    public TestCard(string cardType, string cardName, int min){
        type         = cardType;
        name         = cardName;
        minimum      = min;
    }

    public int getMinimum(){
        return this.minimum;
    }
}
