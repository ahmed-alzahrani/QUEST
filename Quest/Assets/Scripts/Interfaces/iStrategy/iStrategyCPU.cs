using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStrategyCPU : iStrategy
{
  
  public List<Card> executeTournament(List<Card> hand){
    return hand;
  }

  public List<Card> executeFoe(int bp , List<Card> hand){
    return hand;
  }

  public List<Card> executeTest(int bid ,List<Card> hand){
    return hand;
  }
}
