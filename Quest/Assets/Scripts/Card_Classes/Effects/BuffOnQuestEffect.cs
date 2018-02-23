using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffOnQuestEffect : AllyEffect
{
  public BuffOnQuestEffect() {}

  public int BuffOnQuest(string quest, string currentQuest, int buff){
    if (quest == currentQuest){
      return buff;
    }
    return 0;
  }
  public int BuffOnCardInPlay(List<Player> players, string ally, int buff) {return 0;}
}
