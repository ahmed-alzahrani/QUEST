using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TournamentCard : Card {
    // member variables
    private int shields {get; set;}
    private iStoryTournament tourney;

    // member functions

    public TournamentCard(string cardType, string cardName, string texture, int cardShields, iStoryTournament tournament){

        type        = cardType;
        name        = cardName;
        texturePath = texture;
        shields     = cardShields;
    }

    public int getShields(){
        return this.shields;
    }
}
