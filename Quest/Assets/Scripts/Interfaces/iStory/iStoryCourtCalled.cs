using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryCourtCalled : iStory{
  public void execute(List<Player> players, int shields){
    // implement Court Called to Camelot
    if (players != null) {
      Debug.Log("All Allies in play must be discarded");
    }
  }
}
