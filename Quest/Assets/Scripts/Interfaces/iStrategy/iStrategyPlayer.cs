using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStrategyPlayer : iStrategy{

  public List<Card> executeTournament(List<Card> hand){
    return hand;
  }

  public List<Card> executeFoe(List<Card> hand){
    return hand;
  }

  public List<Card> executeTest(List<Card> hand){
    return hand;
  }

}