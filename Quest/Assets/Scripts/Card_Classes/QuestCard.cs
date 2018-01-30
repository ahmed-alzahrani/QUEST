<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class QuestCard : Card
{
    // member variables

    public int stages { get; private set; }
    private string foe;

    // member functions

    public QuestCard(string cardType, string cardName, int questStages, string questFoe)
    {
        type        = cardType;
        name        = cardName;
        stages      = questStages;
        foe         = questFoe;
    }

    /*
    public int getStages(){
        return this.stages;
    }
    */

    public bool hasFoe(){
        return (this.foe != "");
    }
}
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class QuestCard : Card
{
    // member variables

    public int stages { get; private set; }
    private string foe;

    // member functions

    public QuestCard(string cardType, string cardName, int questStages, string questFoe)
    {
        type        = cardType;
        name        = cardName;
        stages      = questStages;
        foe         = questFoe;
    }

    /*
    public int getStages(){
        return this.stages;
    }
    */

    public bool hasFoe(){
        return (this.foe != "");
    }
}
>>>>>>> master
