using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class Hand{
    // member variables

    static int limit = 12;
    private int count;
    private Card[] cards; 

    // member functions
    public Hand(Card[] drawnHand){
        count   = drawnHand.length;
        cards   = drawnHand;
    }

    // public void addCard() {}

    // public void discardCard() {}
}