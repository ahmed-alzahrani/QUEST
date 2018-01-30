using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class RankCard : Card
{
    // member variables
    public int battlePoints { get; private set; }

    // member functions
    public RankCard(string cardType, string cardName, int bp){
        type            = cardType;
        name            = cardName;
        battlePoints    = bp;
    }
    
    /*
    public int getBattlePoints(){
        return this.battlePoints;
    }
    */
}
