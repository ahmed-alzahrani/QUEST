using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class iEventKingsCallToArms : iEvent{
  public void PlayEvent(List<Player> players){
    // implement iEventKingsCallToArmss
    if (players != null) {
      Debug.Log("The highest ranked player(s) must place 1 weapon in the discard pile, or if they can not, 2 foe cards");
    }
  }
}
