using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AllyCard : Card{
    // member variables

    private int battlePoints {get; set;}
    private string special {get; set;}

    // member functions
    public AllyCard(string cardType, string cardName, int bp, string specialSkill){
        type            = cardType;
        name            = cardName;
        battlePoints    = bp;
        special         = specialSkill;

    }

    public int getBattlePoints(){
        return this.battlePoints;
    }

    public bool hasSpecial(){
        return (this.special != "");
    }

    // static void executeSpecial() {} execute a specific card's given special ability
}
