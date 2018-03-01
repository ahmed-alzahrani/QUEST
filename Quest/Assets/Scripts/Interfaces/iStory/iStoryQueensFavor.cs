using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryQueensFavor : iStory{
  public iStoryQueensFavor() {}
  public void execute(List<Player> players, Card storyCard, GameController game){
    // implement Queen's Favor
    if (players != null) {
      Debug.Log("The lowest ranked player(s) immediately receive 2 Adventure Cards");
      List<Player> lowestPlayers = new List<Player>();
      int lowest = players[0].score;
      // loop once to set lowest score
      for(int i = 0; i < players.Count; i++)
      {
        if (players[i].score < lowest)
        {
          lowest = players[i].score;
        }
      }
      // loop a second time to collect lowest player(s)
      for (int i = 0; i < players.Count; i++)
      {
        if (players[i].score == lowest)
        {
          lowestPlayers.Add(players[i]);
        }
      }
      // loop through the result with the call to draw 2 cards
      for (int i = 0; i < lowestPlayers.Count; i++)
      {
        lowestPlayers[i].drawCards(game.adventureDeck.drawCards(2));
      }
    }
    //check for discarding 
    game.playerStillOffending = game.PlayerOffending();
    game.isDoneStoryEvent = true;
  }
}
