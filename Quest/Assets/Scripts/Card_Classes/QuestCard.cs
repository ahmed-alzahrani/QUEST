using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestCard : Card {
    // member variables
    private int stages {get; set;}
    private string foe {get; set;}

    // member functions
    public QuestCard(string cardType, string cardName, int questStages, string questFoe){
        type        = cardType;
        name        = cardName;
        stages      = questStages;
        foe         = questFoe;
    }

    public int getStages(){
        return this.stages;
    }

    public bool hasFoe(){
        return (this.foe != "");
    }
}
