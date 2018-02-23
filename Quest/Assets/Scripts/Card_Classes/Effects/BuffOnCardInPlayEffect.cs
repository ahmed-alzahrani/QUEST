using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffOnCardInPlayEffect : AllyEffect
{
  public BuffOnCardInPlayEffect() {}

  public int BuffOnQuest(string quest, string currentQuest, int buff){return 0;}

  public int BuffOnCardInPlay(List<Player> players, string Ally, int buff) {
    for (int i = 0; i < players.Count; i++)
    {
      if (players[i].hasAlly(Ally)){
        return buff;
      }
    }
    return 0;
  }

}
