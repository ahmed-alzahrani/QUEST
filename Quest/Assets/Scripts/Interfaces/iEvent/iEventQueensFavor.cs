using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class iEventQueensFavor : iEvent{
  public void PlayEvent(List<Player> players){
    // implement Queen's Favor
    if (players != null) {
      Debug.Log("The lowest ranked player(s) immediately receive 2 Adventure Cards");
    }
  }
}
