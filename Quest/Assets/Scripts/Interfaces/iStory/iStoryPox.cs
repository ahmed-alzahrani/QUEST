using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryPox : iStory{
  public iStoryPox(){}
  // Pox causes every player EXCEPT the player who drew the card to lose 1 shield
  public void execute(List<Player> players, int shields){
    if (players != null) {
      for(int i = 1; i < players.Count; i++){
        players[i].removeShields(1);
      }
    }
  }
}
