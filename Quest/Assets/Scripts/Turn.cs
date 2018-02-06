using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Turn{
    // member variables
    public List<Player> players {get; set;}
    public Card storyCard {get; set;}


    // member functions

    // 2 param constructor
    public Turn(List<Player> playerList, Card story){
        players = playerList;
        storyCard = story;
    }

    // execute function needs to go here that just calls the execute function of the story card

    public void display()
    {
        Debug.Log("Players: " + players);
        Debug.Log("Story Card: " + storyCard );
    }
}
