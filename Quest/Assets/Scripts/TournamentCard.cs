using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class TournamentCard : Card {
    // member variables

    private int shields;

    // member functions

    public TournamentCard(string cardType, string cardName, int cardShields){
        
        type        = cardType;
        name        = cardName; 
        shields     = cardShields;
    }

    public int getShields(){
        return this.shields;
    }
}
