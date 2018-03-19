using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStrategyCPU3 : iStrategy
{
  public iStrategyCPU3(){}

  // This CPU will participate in a tournament if there are more than 2 bonus shields on offer.
  public int participateInTourney(List<Player> players, int shields, Controller game)
  {
    if (shields > 2)
    {
      return 1;
    }
    return 0;
  }

  // This CPU will play the stongest hand possible while NOT duplicating weapons
  public List<Card> playTournament(List<Player> players, List<Card> hand, int baseBP, int shields)
  {
    strategyUtil strat = new strategyUtil();
    List<Card> validCards = new List<Card>();
    for (int i = 0; i < hand.Count; i++)
    {
      var type = hand[i].type;
      if (type == "Ally Card" || type == "Weapon Card" || type == "Amour Card")
      {
        validCards.Add(hand[i]);
      }
    }

    validCards = strat.sortAllValidCardsByDescendingBP(validCards, players, "");
    List<Card> cardsToPlay = new List<Card>();

    for (int i = 0; i < validCards.Count; i++)
    {
      if (strat.checkDuplicate(validCards[i], cardsToPlay, "Weapon Card"))
      {
        cardsToPlay.Add(validCards[i]);
        hand.Remove(validCards[i]);
      }
    }
    return cardsToPlay;
  }

  // This CPU sponsors a quest if EITHER of its two conditions are met.
  public int sponsorQuest(List<Player> players, int stages, List<Card> hand, Controller game)
  {
    if (firstCondition(players, stages, hand) || secondCondition(hand, stages))
    {
      return 1;
    } else{
      return 0;
    }
  }

  // the first condition checks that more than one player can rank up in this quest and the CPU has valid sponsorship.
  public bool firstCondition(List<Player> players, int stages, List<Card> hand)
  {
    strategyUtil strat = new strategyUtil();
    int count = strat.rankUpCount(players, stages);
    return (count > 1 && strat.canISponsor(hand, stages));
  }

  // The second condition checks that the CPU has a valid sponsorship and 6 or more foes.
  public bool secondCondition(List<Card> hand, int stages)
  {
    strategyUtil strat = new strategyUtil();
    int foeCount = 0;
    for (int i = 0; i < hand.Count; i++)
    {
      if (hand[i].type == "Foe Card")
      {
        foeCount += 1;
      }
    }
    return (foeCount > 5 && strat.canISponsor(hand, stages));
  }


  public List<List<Card>> setupQuest(int stages, List<Card> hand, string questFoe)
  {
    strategyUtil strat = new strategyUtil();
    // initalize the quest line
    List<List<Card>> questLine = new List<List<Card>>();

    // call setupFoeStage with stage == stages so it'll know to set up the FINAL encounter.
    List<Card> finalStage = setupFoeStage(stages, stages, hand, questFoe, 0);

    // get the BP of the foe encounter and add it to the questLine
    int prevBP = strat.sumFoeEncounterCards(finalStage, questFoe);
    questLine.Add(finalStage);

    // we either build the rest of the Quest taking into account a test encounter.
    if (strat.haveTest(hand))
    {
      List<Card> testStage = setupTestStage(hand);
      questLine.Add(testStage);
      for (int i = 0; i < (stages - 2); i++)
      {
        List<Card> foeStage = setupFoeStage(i, stages, hand, questFoe, prevBP);
        prevBP = strat.sumFoeEncounterCards(foeStage, questFoe);
        questLine.Add(foeStage);
      }
    }
    // or a quest being filled by purely foe encounters
    else {
      for (int i = 0; i < (stages - 1); i++)
      {
        List<Card> foeStage = setupFoeStage(i, stages, hand, questFoe, prevBP);
        prevBP = strat.sumFoeEncounterCards(foeStage, questFoe);
        questLine.Add(foeStage);
      }
    }

    // reverse the quest because weve set up the stages backward then return
    questLine.Reverse();
    return questLine;
  }

  public List<Card> setupFoeStage(int currentStage, int stages, List<Card> hand, string questFoe, int prev)
  {
    // set up a final foe if its the last stage
    if (currentStage == stages)
    {
      return setUpFinalFoe(hand, questFoe);
    }
    // otherwise set up an earlier stage encounter
    return setUpEarlyFoeEncounter(hand, questFoe, prev);
  }

  public List<Card> setUpFinalFoe(List<Card> hand, string questFoe)
  {
    strategyUtil strat = new strategyUtil();
    // get a list of our foes and weapons
    List<Card> foes = new List<Card>();
    List<Card> weapons = new List<Card>();

    for (var i = 0; i < hand.Count; i++)
    {
      if (hand[i].type == "Foe Card")
      {
        foes.Add(hand[i]);
      }
      // for our weapons, we also filter out duplicates
      if (hand[i].type == "Weapon Card" && strat.checkDuplicate(hand[i], weapons, "Weapon Card"))
      {
        weapons.Add(hand[i]);
      }
    }

    // sort the foes in descending order so our strongest foe is first
    foes = strat.sortFoesByDescendingOrder(foes, questFoe);

    // instantiate the foe encounter
    List<Card> foeEncounter = new List<Card>();

    // add the strongest foe to the encounter
    foeEncounter.Add(foes[0]);
    hand.Remove(foes[0]);

    // loop twice, once to add all of our non duplicate weapons to the encounter
    for(int i = 0; i < weapons.Count; i++)
    {
      foeEncounter.Add(weapons[i]);
    }

    // and once to remove from our hands
    for(int i = 1; i < foeEncounter.Count; i++)
    {
      hand.Remove(foeEncounter[i]);
    }

    return foeEncounter;
  }


  public List<Card> setUpEarlyFoeEncounter(List<Card> hand, string questFoe, int prev)
  {
    strategyUtil strat = new strategyUtil();
    // instantiate a list of foes we have and the foe encounter
    List<Card> foeEncounter = new List<Card>();
    List<Card> foes = new List<Card>();

    // put all valid foe cards in the foes list
    for (int i = 0; i < hand.Count; i++)
    {
      if (hand[i].type == "Foe Card" && strat.getValidCardBP(hand[i], new List<Player>(), questFoe) < prev)
      {
        foes.Add(hand[i]);
      }
    }
    // sort by descending order, take teh strongest and return
    foes = strat.sortFoesByDescendingOrder(foes, questFoe);
    foeEncounter.Add(foes[0]);
    hand.Remove(foes[0]);

    return foeEncounter;
  }

  public List<Card> setupTestStage(List<Card> hand)
  {
    strategyUtil strat = new strategyUtil();
    List<Card> tests = new List<Card>();
    // gather all of the tests in the CPU hand
    for (int i = 0; i < hand.Count; i++)
    {
      if (hand[i].type == "Test Card")
      {
        tests.Add(hand[i]);
      }
    }
    // sort them in ascending order and return the first
    tests = strat.sortTetstsbyAscendingOrder(tests);
    List<Card> test = new List<Card>();
    test.Add(tests[0]);
    return test;
  }

  public int participateInQuest(int stages, List<Card> hand, Controller game)
  {
    // this CPU will always elect to play in any Quest
    return 1;
  }


  public List<Card> playFoeEncounter(int stage, int stages, List<Card> hand, int previous, bool amour, string questName, List<Player> players)
  {
    // we check if if its the final stage or not and prepare differently
    if (stage == stages)
    {
      List<Card> foeEncounter = playFinalFoe(hand, amour);
      return foeEncounter;
    }
    else {
      List<Card> foeEncounter = playEarlierFoe(hand, previous, amour, questName, players);
      return foeEncounter;
    }
  }

  public List<Card> playEarlierFoe(List<Card> hand, int previous, bool amour, string questName, List<Player> players)
  {
    strategyUtil strat = new strategyUtil();
    List<Card> foeEncounter = new List<Card>();
    List<Card> weapons = new List<Card>();

    /*
    Loop through the hand, if it as an Amour and Amour == false we add it, if it is an ally we add it
    This is because the effects of these will be felt throughout the quest, play them now.

    We also add only our weakest weapon each round.
    */
    for (int i = 0; i < hand.Count; i++)
    {
      if (amour == false)
      {
        if (hand[i].type == "Amour Card")
        {
          foeEncounter.Add(hand[i]);
          amour = true;
        }
      }
      if (hand[i].type == "Ally Card")
      {
        foeEncounter.Add(hand[i]);
      }
      if (hand[i].type == "Weapon Card")
      {
        weapons.Add(hand[i]);
      }
    }

    weapons = strat.sortWeaponsByAscendingOrder(weapons);
    foeEncounter.Add(weapons[0]);
    return foeEncounter;
  }

  public List<Card> playFinalFoe(List<Card> hand, bool amour)
  {
    strategyUtil strat = new strategyUtil();
    List<Card> foeEncounter = new List<Card>();

    // loop through our hand and play ALL non duplicate weapons allies and possible amours

    for (int i = 0; i < hand.Count; i++)
    {
      if (hand[i].type == "Weapon Card" && strat.checkDuplicate(hand[i], foeEncounter, "Weapon Card"))
      {
        foeEncounter.Add(hand[i]);
      }
      if (hand[i].type == "Ally Card")
      {
        foeEncounter.Add(hand[i]);
      }
      if (hand[i].type == "Amour Card" && (amour == false))
      {
        foeEncounter.Add(hand[i]);
        amour = true;
      }
    }
    return foeEncounter;
  }

  public int willIBid(int currentBid, List<Card> hand, int round, Controller game)
  {
    // we play the bid and make the bid of that value if its greater than the current bid
    List<Card> bid = playBid(hand, round);
    if (bid.Count > currentBid)
    {
      return bid.Count;
    }
    return 0;
  }

  public List<Card> playBid(List<Card> hand, int round)
  {
    strategyUtil strat = new strategyUtil();
    // this CPU bids with any test cards, foe cards of less than 30bp and duplicate weapons (1 of each)
    List<Card> bid = playBid(hand, round);
    if (round > 1)
    {
      return bid;
    }
    for (int i = 0; i < hand.Count; i++)
    {
      if (hand[i].type == "Test Card")
      {
        bid.Add(hand[i]);
      }
      if (hand[i].type == "Foe Card")
      {
        FoeCard foe = (FoeCard)hand[i];
        if (foe.minBP < 30)
        {
          bid.Add(hand[i]);
        }
      }
      if (hand[i].type == "Weapon Card" && strat.hasMultiple(hand, hand[i].name) && strat.checkDuplicate(hand[i], bid, hand[i].type))
      {
        bid.Add(hand[i]);
      }
    }
    return bid;
  }

  // discardWeapon and discardFoesForKing are both taken from stratUtil
  public List<Card> discardWeapon(List<Card> hand)
  {
    strategyUtil strat = new strategyUtil();
    return strat.discardWeapon(hand);
  }

  public List<Card> discardFoesForKing(List<Card> hand)
  {
    strategyUtil strat = new strategyUtil();
    return strat.discardFoesForKing(hand);
  }

  public int respondToPrompt(Controller game)
  {
    return 2;
  }

  // taken from strategyUtil
  public List<Card> fixHandDiscrepancy(List<Card> hand)
  {
    strategyUtil strat = new strategyUtil();
    List<Card> toDiscard = strat.fixHandCPU(hand);
    return toDiscard;
  }

}
