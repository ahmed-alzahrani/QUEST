using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class iEventPlague : iEvent{
  public void PlayEvent(List<Player> players){
    // implement Plague
    if (players != null) {
      Debug.Log("Drawer loses 2 shields if possible");
    }
  }
}
