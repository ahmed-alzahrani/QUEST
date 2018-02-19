using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryProsperity : iStory{
  public iStoryProsperity(){}
  public void execute(List<Player> players, int shields){
    // implement Propserity Throughout the Realm
    // All players immediately draw 2 adventure cards
    if (players != null) {
      Debug.Log("All Players may immedaitely draw 2 adventure cards");
    }
  }
}
