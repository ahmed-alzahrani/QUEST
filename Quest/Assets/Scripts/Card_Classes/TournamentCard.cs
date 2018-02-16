using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TournamentCard : Card {
    // member variables
    private int shields {get; set;}
    private iStoryTournament tournament;

    // member functions

    public TournamentCard(string cardType, string cardName, string texture, int cardShields){

        type        = cardType;
        name        = cardName;
        texturePath = texture;
        shields     = cardShields;
        tournament  = new iStoryTournament();
    }

    public int getShields(){
        return this.shields;
    }
}
