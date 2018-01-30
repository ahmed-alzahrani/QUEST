using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
<<<<<<< HEAD
using UnityEngine.EventSystems;

class WeaponCard : Card
{
    // member variable
    // set access modifiers for other class using what is inside the curly brackets it is effectively like using a private variable with a public getter
    public int battlePoints { get; private set; }

    // member functions

    public WeaponCard(string cardType, string cardName, int bp)
    {
        this.type           = cardType;
        this.name           = cardName;
        this.battlePoints   = bp;
    }

    /*
    public int getBattlePoints(){
        return this.battlePoints;
    }
    */
}
=======
using UnityEngine.EventSystems;

class WeaponCard : Card
{
    // member variable
    // set access modifiers for other class using what is inside the curly brackets it is effectively like using a private variable with a public getter
    public int battlePoints { get; private set; }

    // member functions

    public WeaponCard(string cardType, string cardName, int bp)
    {
        this.type           = cardType;
        this.name           = cardName;
        this.battlePoints   = bp;
    }

    /*
    public int getBattlePoints(){
        return this.battlePoints;
    }
    */
}
>>>>>>> master
