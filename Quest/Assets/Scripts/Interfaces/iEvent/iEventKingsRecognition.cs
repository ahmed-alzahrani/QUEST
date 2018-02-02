using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class iEventKingsRecognition : iEvent{
  public void PlayEvent(List<Player> players){
    // implement King's Recongition
    if (players != null) {
      Debug.Log("The Next player(s) to complete a Quest will receive 2 extra shields");
    }
  }
}
