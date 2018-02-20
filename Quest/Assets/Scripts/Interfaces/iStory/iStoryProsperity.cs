using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryProsperity : iStory{
  public iStoryProsperity(){}
  public bool execute(List<Player> players, Card storyCard, Deck adventure){
    // implement Propserity Throughout the Realm
    // All players immediately draw 2 adventure cards
    if (players != null) {
      Debug.Log("All Players may immedaitely draw 2 adventure cards");
      for (int i = 0; i < players.Count; i++)
      {
                players[i].drawCards(adventure.drawCards(2)); 
      }
            return true;
    }
        return false;
  }
}
