using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NoBuff : AllyEffect
{
  public NoBuff() {}
  public int BuffOnQuest(string quest, string currentQuest, int buff){ return 0; }
  public int BuffOnCardInPlay(List<Player> players, string Ally, int buff) {return 0;}
}
