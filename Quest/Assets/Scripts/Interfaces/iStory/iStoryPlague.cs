using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryPlague : iStory{
  public iStoryPlague(){}
  public bool execute(List<Player> players, Card storyCard, Deck adventure){
    // implement Plague --> Drawer loses 2 shields if possible
    if (players != null) {
      Debug.Log("Drawer loses 2 shields if possible");
      players[0].removeShields(2);
      return true;
    }
        return false;
  }
}
