using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class iEventProsperity : iEvent{
  public void PlayEvent(List<Player> players){
    // implement Propserity Throughout the Realm
    if (players != null) {
      Debug.Log("All Players may immedaitely draw 2 adventure cards");
    }
  }
}
