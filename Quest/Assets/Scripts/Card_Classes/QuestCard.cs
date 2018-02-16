using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestCard : Card {
    // member variables
    private int stages {get; set;}
    private string foe {get; set;}
    private iStoryQuest quest;

    // member functions
    public QuestCard(string cardType, string cardName, string texture, int questStages, string questFoe){
        type        = cardType;
        name        = cardName;
        texturePath = texture;
        stages      = questStages;
        foe         = questFoe;
        quest       = new iStoryQuest();
    }

    public int getStages(){
        return this.stages;
    }

    public bool hasFoe(){
        return (this.foe != "");
    }
}
