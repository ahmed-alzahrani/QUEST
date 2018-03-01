using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryPlague : iStory{
  public iStoryPlague(){}
  public void execute(List<Player> players, Card storyCard, GameController game){
    // implement Plague --> Drawer loses 2 shields if possible
    if (players != null) {
      Debug.Log("Drawer loses 2 shields if possible");
      players[game.currentPlayerIndex].removeShields(2);
    }
    game.isDoneStoryEvent = true;
  }
}
