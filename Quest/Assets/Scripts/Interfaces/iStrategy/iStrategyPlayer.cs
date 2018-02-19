using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStrategyPlayer : iStrategy
{
  public iStrategyPlayer() {}
    // Tournament Strategy
    public bool participateInTourney(List<Player> players, int shields)
    {
      return true;
    }

    public List<Card> playTournament(List<Player> players, List<Card> hand, int baseBP, int shields)
    {
      return hand;

    }



    // Quest Strategy
    public bool sponsorQuest(List<Player> players, int stages, List<Card> hand)
    {
      return true;
    }

    public List<List<Card>> setupQuest(int stages, List<Card> hand)
    {
      List<List<Card>> questLine = new List<List<Card>>();
      return questLine;
    }

    public List<Card> setupFoeStage(int currentStage, int stages, List<Card> hand)
    {
      return hand;
    }

    public List<Card> setupTestStage(List<Card> hand)
    {
      List<Card> test = new List<Card>();
      return test;
      // get the test card with the highest bid test card in the hand
    }

    public bool participateInQuest(int stages, List<Card> hand)
    {
      return true;
    }

    public List<Card> playFoeEncounter(int stage, int stages, List<Card> hand, int previous, bool amour)
    {
      return hand;
    }


    // Test Strategy

    public int willIBid(int currentBid, List<Card> hand, int round)
    {
      return currentBid + 1;
    }

    public List<Card> playBid(List<Card> hand, int round)
    {
      return hand;
    }

    public List<Card> kingsCall(List<Card> hand)
    {
      return hand;
    }

    public List<Card> discardWeapon(List<Card> hand){
      return hand;
    }
    public List<Card> discardFoesForKing(List<Card> hand){
      return hand;
    }
}
