using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iStrategy {

  //tournament strategy
  bool participateInTourney(List<Player> players, int shields);
  List<Card> playTournament(List<Player> players, List<Card> hand, int baseBP, int shields);

  // Quest Strategy
  bool sponsorQuest(List<Player> players, int stages, List<Card> hand);
  List<List<Card>> setupQuest(int stages, List<Card> hand);
  List<Card> setupFoeStage(int currentStage, int stages, List<Card> hand);
  List<Card>   setupTestStage(List<Card> hand);
  bool participateInQuest(int stages, List<Card> hand);
  List<Card> playFoeEncounter(int stage, int stages, List<Card> hand, int previous, bool amour);


  // Test strategy
  int willIBid(int currentBid, List<Card> hand, int round);
  List<Card> playBid(List<Card> hand, int round);

  // additonal stratgy to be added includes:


  List<Card> kingsCall(List<Card> hand);

  // evaluating what cards to discard if the hand becomes too full
}
