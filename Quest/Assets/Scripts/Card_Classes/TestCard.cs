using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class TestCard : Card {
    // member variables

    private int minimum;

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
