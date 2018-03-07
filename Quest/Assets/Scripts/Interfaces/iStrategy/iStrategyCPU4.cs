using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStrategyCPU4 : iStrategyCPU3
{
  public iStrategyCPU4(){
    strategyUtil strat = new strategyUtil();
    iStrategyCPU1 strat1 = new iStrategyCPU1();
    iStrategyCPU2 strat2 = new iStrategyCPU2();
  }

  public bool strat1Approach()
  {
    Random rnd = new Random();
    result = Rand.Next(0, 2);

    if (result == 1)
    {
      return true;
    }
    else
    {
      return false;
    }
  }

  public int participateInTourney(List<Player> players, int shields, GameController game)
  {
    if (strat1Approach())
    {
      return 1;
    }
    return 2;
  }

  public List<Card> playTournament(List<Player> players, List<Card> hand, int baseBP, int shields)
  {
    if (strat1Approach())
    {
      return strat1.playTournament(players, hand, baseBP, shields);
    }
    else
    {
      return strat2.playTournament(players, hand, baseBP, shields);
    }
  }

  public int sponsorQuest(List<Player> players, int stages, List<Card> hand, GameController game)
  {
    if (strat.canSomeoneRankUp(players, stages))
    {
      return 0;
    }

    if (strat.canISponsor(hand, stages))
    {
      return 1;
    }
    return 0;
  }

  public List<List<Card>> setupQuest(int stages, List<Card> hand, string questFoe)
  {
    if (strat1Approach())
    {
      return strat1.setupQuest(stages, hand, questFoe);
    }
    else
    {
      return strat2.setupQuest(stages, hand, questFoe);
    }
  }

  public List<Card> setupFoeStage(int currentStage, int stages, List<Card> hand, string questFoe, int prev)
  {
    return null;
  }

  public List<Card> setupTestStage(List<Card> hand)
  {
    return null;
  }

  public int participateInQuest(int stages, List<Card> hand, GameController game)
  {
    if (strat1Approach())
    {
      return strat1.participateInQuest(stages, hand, game);
    }
    else
    {
      return strat2.participateInQuest(stages, hand, game);
    }
  }

  public List<Card> playFoeEncounter(int stage, int stages, List<Card> hand, int previous, bool amour, string questName, List<Player> players)
  {
    return strat1.playFoeEncounter(stage, stages, hand, previous, amour, questName, players);
  }

  public int willIBid(int currentBid, List<Card> hand, int round, GameController game)
  {
    return strat1.willIBid(currentBid, hand, round, game);
  }

  public List<Card> playBid(List<Card> hand, int round)
  {
    return strat1.playBid(hand, round);
  }

  public List<Card> discardWeapon(List<Card> hand)
  {
    return strat.discardWeapon(hand);
  }

  public List<Card> discardFoesForKing(List<Card> hand)
  {
    return strat.discardFoesForKing(hand);
  }

  public int respondToPrompt(GameController game)
  {
    return 2;
  }

  public List<Card> fixHandDiscrepancy(List<Card> hand)
  {
    List<Card> toDiscard = strat.fixHandCPU(hand);
  }

}
