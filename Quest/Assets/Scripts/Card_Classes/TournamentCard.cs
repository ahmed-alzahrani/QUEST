using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
<<<<<<< HEAD
using UnityEngine.EventSystems;

class TournamentCard : Card
{
    // member variables
    //mentioned in weaponCard.cs
    public int shields { get; private set; }

    // member functions
    public TournamentCard(string cardType, string cardName, int cardShields)
    {
        
        type        = cardType;
        name        = cardName; 
        shields     = cardShields;
    }
    
    /*
    public int getShields()
    {
        return this.shields;
    }
    */
}
=======
using UnityEngine.EventSystems;

class TournamentCard : Card
{
    // member variables
    //mentioned in weaponCard.cs
    public int shields { get; private set; }

    // member functions
    public TournamentCard(string cardType, string cardName, int cardShields)
    {
        
        type        = cardType;
        name        = cardName; 
        shields     = cardShields;
    }
    
    /*
    public int getShields()
    {
        return this.shields;
    }
    */
}
>>>>>>> master
