using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RankCard : Card {
    // member variables
    private int battlePoints {get; set;}

    // member functions
    public RankCard(string cardType, string cardName, string texture, int bp)
    {
        type            = cardType;
        name            = cardName;
        texturePath     = texture;
        battlePoints    = bp;
    }

    public int getBattlePoints()
    {
        return this.battlePoints;
    }

    public new void display()
    {
        Debug.Log("Type: " + type);
        Debug.Log("Name: " + name);
        Debug.Log("Battle Points: " + battlePoints);
    }
}
