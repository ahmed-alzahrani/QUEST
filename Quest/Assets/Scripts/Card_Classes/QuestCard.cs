using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class QuestCard : Card {
    // member variables

    private int stages;
    private foe string;

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
