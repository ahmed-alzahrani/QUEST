using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AmourCard : Card {
    // member variables
    private int bid {get; set;}

    // member funtions

    public AmourCard(string cardType, string cardName, int cardBid){
        type        = cardType;
        name        = cardName;
        bid         = cardBid;
    }

    public int getBid(){
        return this.bid;
    }
}
