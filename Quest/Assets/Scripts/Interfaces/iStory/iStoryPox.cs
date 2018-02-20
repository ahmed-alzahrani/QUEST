using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryPox : iStory{
  public iStoryPox(){}
  // Pox causes every player EXCEPT the player who drew the card to lose 1 shield
  public bool execute(List<Player> players, Card storyCard, Deck adventure){
    if (players != null) {
      for(int i = 1; i < players.Count; i++){
        players[i].removeShields(1);
      }
    }
     return true;
  }
}
