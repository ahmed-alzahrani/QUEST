using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class Mordred: FoeCard
{
    // member variables

    // member functions
    public Mordred(){
        type    = "Foe";
        name    = "Mordred";
        minBP   = 30;
        maxBP   = 0;
    }

    // public void sacrifice(){} special ability
}
