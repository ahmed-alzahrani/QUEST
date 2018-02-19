using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AmourCard : Card {
    // member variables
    public int bid {get; set;}
    public int battlePoints {get; set;}

    // member funtions

    public AmourCard(string cardType, string cardName, string texture, int cardBid, int bp){
        type        = cardType;
        name        = cardName;
        texturePath = texture;
        bid         = cardBid;
        battlePoints = bp;
    }

    public int getBid(){
        return this.bid;
    }
}
