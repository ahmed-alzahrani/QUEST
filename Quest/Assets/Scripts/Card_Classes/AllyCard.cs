using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
<<<<<<< HEAD
using UnityEngine.EventSystems;

class AllyCard : Card
{
    // member variables

    public int battlePoints { get; private set; }
    private string special;

    // member functions
    public AllyCard(string cardType, string cardName, int bp, string specialSkill){
        type            = cardType;
        name            = cardName;
        battlePoints    = bp;
        special         = specialSkill;

    }

    /*
    public int getBattlePoints(){
        return this.battlePoints;
    }
    */

    public bool hasSpecial(){
        return (this.special != "");
    }

    // static void executeSpecial() {} execute a specific card's given special ability
}
=======
using UnityEngine.EventSystems;

class AllyCard : Card
{
    // member variables

    public int battlePoints { get; private set; }
    private string special;

    // member functions
    public AllyCard(string cardType, string cardName, int bp, string specialSkill){
        type            = cardType;
        name            = cardName;
        battlePoints    = bp;
        special         = specialSkill;

    }

    /*
    public int getBattlePoints(){
        return this.battlePoints;
    }
    */

    public bool hasSpecial(){
        return (this.special != "");
    }

    // static void executeSpecial() {} execute a specific card's given special ability
}
>>>>>>> master
