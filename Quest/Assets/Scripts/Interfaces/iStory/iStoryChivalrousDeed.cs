using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player(s) with the lowest rank and least amount of shields recieves 3 shields
public class iStoryChivalrousDeed : iStory{
  public iStoryChivalrousDeed(){ }

  public bool execute(List<Player> players, Card card, Deck adventure){
    // implement Chivalrous Deed

    // gets a list of the players who have the lowest score in the game and grant them 3 shields each
    if (players != null) {
      List<Player> lowest = getLowestPlayers(players);
      for (var i = 0; i < lowest.Count; i++) {
        lowest[i].addShields(3);
      }
       Debug.Log("Player(s) with both lowest rank and least amount of shields, receives 3 shields");
       return true;
    }
        return false;
  }

  // getLowest loops through the list of players and gets the current lowest score in the game
  public int getLowest(List<Player> players){
    int lowest = players[0].score;
    for (var i = 0; i < players.Count; i++) {
      if (players[i].score < lowest) {
        lowest = players[i].score;
      }
    }
    return lowest;
  }

  // getLowestPlayers returns a list of all players who currently have the lowest score in the game
  public List<Player> getLowestPlayers(List<Player> players) {
    int lowest = getLowest(players);
    List<Player> lowestPlayers = new List<Player>();
    for (var i = 0; i < players.Count; i++) {
      if (players[i].score == lowest) {
        lowestPlayers.Add(players[i]);
      }
    }
    return lowestPlayers;
  }
}
