using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class iEventCourtCalled : iEvent{
  public void PlayEvent(List<Player> players){
    // implement Court Called to Camelot
    if (players != null) {
      Debug.Log("All Allies in play must be discarded");
    }
  }
}
