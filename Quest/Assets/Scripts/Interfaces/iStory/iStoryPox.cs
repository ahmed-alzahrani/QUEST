using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryPox : iStory{
  // Pox causes every player EXCEPT the player who drew the card to lose 1 shield
  public void execute(List<Player> players, int shields){
    for(int i = 1; i <= players.Count;){
      players[i].rankDecrease();
    }
    // implement Pox
  }
}
