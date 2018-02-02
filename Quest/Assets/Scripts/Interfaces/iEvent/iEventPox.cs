using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class iEventPox : iEvent{
  // Pox causes every player EXCEPT the player who drew the card to lose 1 shield
  public void PlayEvent(List<Player> players){
    for(int i = 1; i <= players.Count;){
      players[i].rankDecrease();
    }
    // implement Pox
  }
}
