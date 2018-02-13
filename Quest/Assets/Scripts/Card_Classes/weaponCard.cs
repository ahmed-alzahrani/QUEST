using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponCard : Card{
    // member variable
    private int battlePoints;

    // member functions

    public WeaponCard(string cardType, string cardName, string texture, int bp) {
        type           = cardType;
        name           = cardName;
        battlePoints   = bp;
        texturePath         = texture;
    }
    public int getBattlePoints(){
        return this.battlePoints;
    }
}
