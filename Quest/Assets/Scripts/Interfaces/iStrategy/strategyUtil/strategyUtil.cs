using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class strategyUtil
{
  public strategyUtil(){}

  // iStrategyCPU1 VERSION
  // Can Someone Rank Up? Looks at each player and each type of rank up individually from Knight to Knight of the Round Table
  public bool canSomeoneRankUp(List<Player> players, int shields)
  {
      for (var i = 0; i < players.Count; i++)
      {
          int score = players[i].score;
          // Can rank up from Squire to Knight
          if (score < players[i].knightScore && (score + shields) >= players[i].knightScore)
          {
              return true;
          }
          // Can rank up from Knight to Champion Knight
          if (score < players[i].champKnightScore && (score + shields) >= players[i].champKnightScore)
          {
              return true;
          }
          // Can become a Knight of the Round Table
          if (score < players[i].kotrkScore && (score + shields) >= players[i].kotrkScore)
          {
              return true;
          }
      }
      return false;
  }

  // checks whether or not a CPU has the requisite cards to sponsor the quest
  public bool canISponsor(List<Card> hand, int stages)
  {
    // see if we have enough cards to sponsor the quest
    // Count through our hand and subtract
    strategyUtil strat = new strategyUtil();
    int stageCount = stages;
    List<Card> foes = new List<Card>();
    for (var i = 0; i < hand.Count; i++){
      // count up our foes to compare to the # of stages
      if (hand[i].type == "Foe Card" && checkDuplicate(hand[i], foes, "Foe Card")){
        stageCount -= 1;
        foes.Add(hand[i]);
      }
    }
    // decreate stageCount by one more if we had a Test
    if (strat.haveTest(hand)){
      stageCount -= 1;
    }
    return (stageCount <= 0);
  }

  // checks for a duplicate in instances where a player is restricted by cardType
  public bool checkDuplicate(Card card, List<Card> cards, string cardType)
  {
      if (card.type == cardType)
      {
          for (var i = 0; i < cards.Count; i++)
          {
              if (cards[i].name == card.name)
              {
                  return false;
              }
          }
      }
      return true;
  }

  // tells us if there are multiple of a card within a hand
  public bool hasMultiple(List<Card> cards, string name)
  {
      int count = 0;
      for (int i = 0; i < cards.Count; i++)
      {
          if (cards[i].name == name)
          {
              count += 1;
          }
      }
      return (count >= 2);
  }

  // public bool haveTests tells a player if they have a test
  public bool haveTest(List<Card> hand)
  {
      for (int i = 0; i < hand.Count; i++)
      {
          if (hand[i].type == "Test Card")
          {
              return true;
          }
      }
      return false;
  }

  // Discards the lowest possible weapon in the player's hand for the king's call to arms event
  public List<Card> discardWeapon(List<Card> hand)
  {
    List<Card> weapons = new List<Card>();
    List<Card> discard = new List<Card>();

    for (int i = 0; i < hand.Count; i++)
    {
      if (hand[i].type == "Weapon Card")
      {
        weapons.Add(hand[i]);
      }
    }

    weapons = sortWeaponsByAscendingOrder(weapons);
    discard.Add(weapons[0]);
    return discard;
  }

  // Discard 2 lowest BP foes for the King's Call Event
  public List<Card> discardFoesForKing(List<Card> hand)
  {
      List<Card> foes = new List<Card>();
      List<Card> discard = new List<Card>();

      // Get Foes from hand
      for (int i = 0; i < hand.Count; i++)
      {
          if (hand[i].type == "Foe Card")
          {
              foes.Add(hand[i]);
          }
      }

      // if there is only one foe return that
      if (foes.Count == 1)
      {
          discard.Add(foes[0]);
          return discard;
      }
      // else return the 2 lowest by bubble sorting
      else {
          // bubble sort into ascending order by min bp
          for (int x = 0; x < foes.Count; x++)
          {
              for (int i = 0; i < (foes.Count - 1); i++)
              {
                  FoeCard foe1 = (FoeCard)foes[i];
                  FoeCard foe2 = (FoeCard)foes[i + 1];
                  if (foe1.minBP > foe2.minBP)
                  {
                      var temp = foes[i + 1];
                      foes[i + 1] = foes[i];
                      foes[i] = temp;
                  }
              }
          }
          discard.Add(foes[0]);
          discard.Add(foes[1]);
          return discard;
      }
  }

  // Bubble Sorting Functions by card type!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

  // sorts the weapons in ASCENDING order of BP
  public List<Card> sortWeaponsByAscendingOrder(List<Card> weapons)
  {
    for (int x = 0; x < weapons.Count; x++){
      for (int i = 0; i < (weapons.Count - 1); i++){
        WeaponCard weapon1 = (WeaponCard)weapons[i];
        WeaponCard weapon2 = (WeaponCard)weapons[i + 1];
        if (weapon1.battlePoints > weapon2.battlePoints){
          var temp = weapons[i + 1];
          weapons[i + 1] = weapons[i];
          weapons[i] = temp;
        }
      }
    }
    return weapons;
  }

  // sorts the weapons in DESCENDING order of BP
  public List<Card> sortWeaponsByDescendingOrder(List<Card> weapons)
  {
    for (int x = 0; x < weapons.Count; x++){
      for (int i = 0; i < (weapons.Count - 1); i++){
        WeaponCard weapon1 = (WeaponCard)weapons[i];
        WeaponCard weapon2 = (WeaponCard)weapons[i + 1];
        if (weapon1.battlePoints < weapon2.battlePoints){
          var temp = weapons[i + 1];
          weapons[i + 1] = weapons[i];
          weapons[i] = temp;
        }
      }
    }
    return weapons;
  }



  // Sorts the weapon in ASCENDING order
  public List<Card> sortFoesByAscendingOrder(List<Card> foes, string questFoe)
  {
    for (int x = 0; x < foes.Count; x++)
    {
        for (int i = 0; i < (foes.Count - 1); i++)
        {
            FoeCard foe1 = (FoeCard)foes[i];
            FoeCard foe2 = (FoeCard)foes[i + 1];
            int foe1Bp = getContextBP(foe1, questFoe);
            int foe2Bp = getContextBP(foe2, questFoe);
            if (foe1Bp > foe2Bp)
            {
                var temp = foes[i + 1];
                foes[i + 1] = foes[i];
                foes[i] = temp;
            }
        }
    }
    return foes;
  }

  // Sorts the weapons in DESCENDING order
  public List<Card> sortFoesByDescendingOrder(List<Card> foes, string questFoe)
  {
    for (int x = 0; x < foes.Count; x++)
    {
        for (int i = 0; i < (foes.Count - 1); i++)
        {
            FoeCard foe1 = (FoeCard)foes[i];
            FoeCard foe2 = (FoeCard)foes[i + 1];
            int foe1Bp = getContextBP(foe1, questFoe);
            int foe2Bp = getContextBP(foe2, questFoe);

            if (foe1Bp < foe2Bp)
            {
                var temp = foes[i + 1];
                foes[i + 1] = foes[i];
                foes[i] = temp;
            }
        }
    }
    return foes;
  }

  // if the foe passed in is the quest foe, or the quest foe is all, or it is a saxon quest and the foe is a saxon return max
  public int getContextBP(FoeCard foe, string questFoe)
  {
    if (questFoe == foe.name || questFoe == "All" || saxonBonus(foe, questFoe))
    {
      return foe.maxBP;
    }
    return foe.minBP;
  }

  // returns True if the quest is a Saxon quest and the foe is a saxon
  public bool saxonBonus(FoeCard foe, string questFoe)
  {
    if (questFoe == "All Saxons"){
      if (foe.name == "Saxons" || foe.name == "Saxon Knight"){
        return true;
      }
    }
    return false;
  }

  // Sorts allies by ASCENDING order of BP
  public List<Card> sortAlliesByAscendingOrder(List<Card> allies, string questName, List<Player> players)
  {
    for (int x = 0; x < allies.Count; x++)
    {
        for (int i = 0; i < (allies.Count - 1); i++)
        {
            AllyCard ally1 = (AllyCard)allies[i];
            AllyCard ally2 = (AllyCard)allies[i + 1];
            if (ally1.getBattlePoints(questName, players) > ally2.getBattlePoints(questName, players))
            {
                var temp = allies[i + 1];
                allies[i + 1] = allies[i];
                allies[i] = temp;
            }
        }
    }
    return allies;
  }

  // Sorts teh list of tests by ASCENDING order of minimum
  public List<Card> sortTetstsbyAscendingOrder(List<Card> tests)
  {
    for (int x = 0; x < tests.Count; x++)
    {
        for (int i = 0; i < (tests.Count - 1); i++)
        {
            TestCard test1 = (TestCard)tests[i];
            TestCard test2 = (TestCard)tests[i + 1];
            if (test1.minimum < test2.minimum)
            {
                var temp = tests[i + 1];
                tests[i + 1] = tests[i];
                tests[i] = temp;
            }
        }
    }
    return tests;
  }

  // Sorts a list of all VALID cards with BP by ASCENDING order of Bp
  public List<Card> sortAllValidCardsByAscendingBP(List<Card> validCards, List<Player> players, string questName)
  {
    // Sort the list in order of BP using bubble sort
    for (int x = 0; x < validCards.Count; x++){
      for (int i = 0; i < (validCards.Count - 1); i++){
        int bp1 = getValidCardBP(validCards[i], players, questName);
        int bp2 = getValidCardBP(validCards[i + 1], players, questName);
        if (bp1 > bp2){
          var temp = validCards[i + 1];
          validCards[i + 1] = validCards[i];
          validCards[i] = temp;
        }
      }
    }

    return validCards;
  }

  // Sorts a list of all VALID cards with BP by ASCENDING order of Bp
  public List<Card> sortAllValidCardsByDescendingBP(List<Card> validCards, List<Player> players, string questName)
  {
    // Sort the list in order of BP using bubble sort
    for (int x = 0; x < validCards.Count; x++){
      for (int i = 0; i < (validCards.Count - 1); i++){
        int bp1 = getValidCardBP(validCards[i], players, questName);
        int bp2 = getValidCardBP(validCards[i + 1], players, questName);
        if (bp1 < bp2){
          var temp = validCards[i + 1];
          validCards[i + 1] = validCards[i];
          validCards[i] = temp;
        }
      }
    }

    return validCards;
  }


  // gets the BP of a single 'valid' card
  public int getValidCardBP(Card card, List<Player> players, string questName){
    if (card.type == "Ally Card"){
      AllyCard ally = (AllyCard)card;
      return ally.getBattlePoints(questName, players);
    }
    if (card.type == "Weapon Card"){
      WeaponCard weapon = (WeaponCard)card;
      return weapon.battlePoints;
    } else {
      // amour
      return 10;
    }
  }

  public int sumFoeEncounterCards(List<Card> cards, string questFoe)
  {
    int sum = 0;
    for (int i = 0; i < cards.Count; i++)
    {
      sum += getValidCardBP(cards[i], new List<Player>(), questFoe);
    }
    return sum;
  }

  public List<Card> fixHandCPU(List<Card> hand)
  {
    List<Card> discard = new List<Card>();

    while (hand.Count > 12){
      discard.Add(hand[hand.Count - 1]);
      hand.RemoveAt(hand.Count - 1);
    }

    return discard;
  }


}
