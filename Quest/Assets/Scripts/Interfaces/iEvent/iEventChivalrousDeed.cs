using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class iEventChivalrousDeed : iEvent{
  public void PlayEvent(List<Player> players){
    // implement Chivalrous Deed
    if (players != null) {
      Debug.Log("Player(s) with both lowest rank and least amount of shields, receives 3 shields");
    }
  }
}
