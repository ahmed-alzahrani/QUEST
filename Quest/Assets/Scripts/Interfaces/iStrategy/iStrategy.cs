using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iStrategy {
  List<Card> executeTournament(List<Card> hand);
  List<Card> executeFoe(int bp, List<Card> hand);
  List<Card> executeTest(int bid, List<Card> hand);
  List<Card> kingsCall(List<Card> hand);
}
