using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStrategyCPU2 : iStrategy
{
  // Tournament Strategy

  // Strategy #2, the CPU always joins tournaments
  public bool participateInTourney(List<Player> players, int shields)
  {
    return true;
  }

  public int getValidCardBP(Card card){
    if (card.type == "Ally"){
      AllyCard ally = (AllyCard)card;
      return ally.battlePoints;
    }
    if (card.type == "Weapon"){
      WeaponCard weapon = (WeaponCard)card;
      return weapon.battlePoints;
    } else {
      // amour
      return 10;
    }
  }


  // We play the fewest cards possible to get to 50 or our best possible total
  public List<Card> playTournament(List<Player> players, List<Card> hand, int baseBP, int shields)
  {
    // Generate a list of valid cards --> weapon, ally, and amour
    List<Card> validCards = new List<Card>();
    for (var i = 0; i < hand.Count; i++) {
      var type = hand[i].type;
      if (type == "Ally" || type == "Weapon" || type == "Amour"){
        validCards.Add(hand[i]);
      }
    }

    // Sort the list in order of BP using bubble sort
    for (int x = 0; x <= validCards.Count; x++){
      for (int i = 0; i <= validCards.Count; i++){
        int bp1 = getValidCardBP(validCards[i]);
        int bp2 = getValidCardBP(validCards[i + 1]);
        if (bp1 > bp2){
          var temp = validCards[i + 1];
          validCards[i + 1] = validCards[i];
          validCards[i] = temp;
        }
      }
    }

    // get how much BP we need left by subtracting our base
    int bpNeeded = 50 - baseBP;

    // instantiate the list of cards were returning to play
    List<Card> cardsToPlay = new List<Card>();

    // index = 0, because were trying to move through the validCards one at a time
    int index = 0;

    // until either we run out of validCards or we have gone above the BP threshold we wish to hit (50)
    while (bpNeeded >= 0){
      // first make sure that we still have cards in validCards to evaluate
      if (index < validCards.Count){
        // check that the card were trying to play isn't a duplicate weapon
        if (checkDuplicate(validCards[index], cardsToPlay, "Weapon")){
          // add the card to our cards to be played
          bpNeeded -= validCards[index].battlePoints;
          cardsToPlay.Add(validCards[index]);
        }
        index ++;
      } else {
        break;
      }
    }

    // return those cards to play
    return cardsToPlay;
  }

  // checks for a duplicate in instances where a player is restricted by cardType
  public bool checkDuplicate(Card card, List<Card> cards, string cardType)
  {
    if (card.type == cardType){
      for (var i = 0; i < cards.Count; i++){
        if (cards[i].name == card.name){
          return false;
        }
      }
    }
    return true;
  }





  // Quest Strategy
  public bool sponsorQuest(List<Player> players, int stages, List<Card> hand)
  {
    // if somebody can rank up, we return false to decline sponsoring the quest
    if (canSomeoneRankUp(players, stages)) {
      return false;
    }

    return canISponsor(hand, stages);
  }

  public bool canISponsor(List<Card> hand, int stages)
  {
    // see if we have enough cards to sponsor the quest
    // Count through our hand and subtract
    int stageCount = stages;
    bool haveTest = false;
    for (var i = 0; i < hand.Count; i++){
      // count up our foes to compare to the # of stages
      if (hand[i].type == "Foe"){
        stageCount -= 1;
      }
      // if we have a Test card in our hand we set the bool to true
      if (hand[i].type == "Test"){
        haveTest = true;
      }
    }
    // decreate stageCount by one more if we had a Test
    if (haveTest){
      stageCount -= 1;
    }
    return (stageCount <= 0);
  }


  // Can Someone Rank Up? Looks at each player and each type of rank up individually from Knight to Knight of the Round Table
  public bool canSomeoneRankUp(List<Player> players, int shields)
  {
    for(var i = 0; i < players.Count; i++){
      int score = players[i].score;
      // Can rank up from Squire to Knight
      if (score < 5 && (score + shields) > 5){
        return true;
      }
      // Can rank up from Knight to Champion Knight
      if (score < 7 && (score + shields) > 7){
        return true;
      }
      // Can become a Knight of the Round Table
      if (score < 10 && (score + shields) > 10){
        return true;
      }
    }
    return false;
  }

  // Return a list of list the count of stages, where at each index is the card(s) that sets up each stage
  public List<List<Card>> setupQuest(int stages, List<Card> hand)
  {
    // instantiate the quest line
    List<List<Card>> questLine = new List<List<Card>>();

    // create the final stage first
    List<Card> finalStage = setupFoeStage(stages, stages, hand);

    // if we have a test, create a test stage and then work from the first stage to fill in the foe stages
    if (haveTest(hand)){
      List<Card> testStage = setupTestStage(hand);
      for (int i = 0; i < (stages - 2); i++){
        List<Card> foeStage = setupFoeStage(i, stages, hand);
        questLine.Add(foeStage);
      }
      questLine.Add(testStage);
      // else we don't have a test, we fill in foe stages the same way but 1 more for the missing test
    } else {
      for (int i = 0; i < (stages - 1); i++){
        List<Card> foeStage = setupFoeStage(i, stages, hand);
        questLine.Add(foeStage);
      }
    }
    // add our final Stage that we created first so its on the end and return
    questLine.Add(finalStage);
    return questLine;
  }

  // looks through our hand and tells us if there is a single test card
  public bool haveTest(List<Card> hand){
    for(int i = 0; i < hand.Count; i++){
      if (hand[i].type == "Test"){
        return true;
      }
    }
    return false;
  }

  // evaluates whether its the last stage or not and sets up either a Final Foe stage or an early Foe Stage
  public List<Card> setupFoeStage(int currentStage, int stages, List<Card> hand)
  {
    if (currentStage == stages){
      return setUpFinalFoe(hand);
    }
    return setUpEarlyFoeEncounter(hand);
  }


  // sets up the final stage of a quest for this CPU player, in this strategy:
  // we need to get 40BP in as few cards as possible
  public List<Card> setUpFinalFoe(List<Card> hand)
  {

    // instantiate a List of foes and weapons from the user's hand
    List<Card> foes = new List<Card>();
    List<Card> weapons = new List<Card>();

    // seperate the foes and weapons into their own lists from the hand
    for (var i = 0; i < hand.Count; i++){
      if (hand[i].type == "Foe"){
        foes.Add(hand[i]);
      }
      // make sure that we sort out weapons that are already in the weapons
      if (hand[i].type == "Weapon" && checkDuplicate(hand[i], weapons, "Weapon")){
        weapons.Add(hand[i]);
      }
    }


    // bubble sort the foes from the player's hand in ascending order
    for (int x = 0; x <= foes.Count; x++){
      for (int i = 0; i <= foes.Count; i++){
        if (foes[i].minBP < foes[i + 1].minBP){
          var temp = foes[i + 1];
          foes[i + 1] = foes[i];
          foes[i] = temp;
        }
      }
    }

    // bubble sort the weapons from the player's hand in ascending order
    for (int x = 0; x <= weapons.Count; x++){
      for (int i = 0; i <= weapons.Count; i++){
        if (weapons[i].battlePoints < weapons[i + 1].battlePoints){
          var temp = weapons[i + 1];
          weapons[i + 1] = weapons[i];
          weapons[i] = temp;
        }
      }
    }

    // instantiate the foeEncounter list
    List<Card> foeEncounter = new List<Card>();

    // subtract the foe with the MOST BP in the user's hand from 40, the AI threshold
    int bpNeeded = (40 - foes[0].minBP);
    // Add this foe to the foeEncounter as the foe to be played
    foeEncounter.Add(foes[0]);

    // initialize index as 0 to loop through the weapons
    int index = 0;

    // while we still need BP toreach our 40 threshold
    while(bpNeeded > 0 && index < weapons.Count){
      // if we still have weapons to loop through
        // subtract the BP of the next most powerful weapon from the threshold
      bpNeeded -= weapons[index].battlePoints;
        // add this weapon to the encounter
      foeEncounter.Add(weapons[index]);
        // increment index
      index++;
    }

    // return the most powerful foe we have with the set of weapons that most quickly gets us to 40 BP.
    return foeEncounter;
  }


  // This sets up an early Foe Encounter, a Foe encounter before the last round of a Quest.
  // This strategy is to simply play the lowest BP foe from the hand with NO weapons attached
  public List<Card> setUpEarlyFoeEncounter(List<Card> hand)
  {
    // get the list of foe cards from the user's hand
    List<Card> foes = new List<Card>();
    for (var i = 0; i < hand.Count; i++){
      if (hand[i].type == "Foe"){
        foes.Add(hand[i]);
      }
    }

    // bubble sort the user's foes so that the first item in the list is the lowest BP foe
    for (int x = 0; x <= foes.Count; x++){
      for (int i = 0; i <= foes.Count; i++){
        if (foes[i].minBP < foes[i + 1].minBP){
          var temp = foes[i + 1];
          foes[i + 1] = foes[i];
          foes[i] = temp;
        }
      }
    }

    // make a list, add the lowest BP foe and return it

    List<Card> foeEncounter = new List<Card>();
    foeEncounter.Add(foes[0]);
    return foeEncounter;
  }

// returns the test card from the user's hand with the highest minimum
  public List<Card> setupTestStage(List<Card> hand)
  {
    List<Card> tests = new List<Card>();
    for (int i = 0; i < hand.Count; i++){
      if (hand[i].type == "Test"){
        tests.Add(hand[i]);
      }
    }
    // sort tests in ascending order of minimum bid
    for (int x = 0; x <= tests.Count; x++){
      for (int i = 0; i <= tests.Count; i++){
        if (tests[i].minimum < tests[i + 1].minimum){
          var temp = tests[i + 1];
            tests[i + 1] = tests[i];
          tests[i] = temp;
        }
      }
    }

    List<Card> test = new List<Card>();
    // return a list with only the highest minimum test
    test.Add(tests[0]);
    return test;
    // get the test card with the highest bid test card in the hand
  }


  public bool participateInQuest(int stages, List<Card> hand)
  {
    // Simply calls both the tests to see if the CPU can play sufficient BP to progress and discard Foes for a Test
    return (canIIncrement(stages, hand) && canIDiscard(hand));
  }

  /*
  Knowing that the player wants to increment by 10BP each round, that means that for x rounds,the minimum number of BP needed to play would be

  10 * 1 + 10 * 2 + 10 * 3 ---> 10 * x for x rounds.

  The player simply tallies their BP, only counting 1 Amour card to see if they can play in the Quest
  */
  public bool canIIncrement(int stages, List<Card> hand){
    int bpNeeded = 0;
    for (int x = 1; x <= stages; x++) {
      bpNeeded += (10 * x);
    }

    List<Card> validCards = new List<Card>();
    for (int i = 0; i < hand.Count; i++){
      if (hand[i].type == "Weapon"){
        validCards.Add(hand[i]);
      }
      if (hand[i].type == "Amour" && checkDuplicate(hand[i], validCards, "Amour")){
        validCards.Add(hand[i]);
      }
      if (hand[i].type == "Ally" && hand[i].battlePoints > 0){
        validCards.Add(hand[i]);
      }
    }

    int bpInPosession = 0;
    for (int i = 0; i < validCards.Count; i++){
      bpInPosession += validCards[i].battlePoints;
    }
    return (bpInPosession >= bpNeeded);
  }

  // checks that the player has at LEAST 2 foes with less than 25 BP that they can discard if need be
  public bool canIDiscard(List<Card> hand){
    int count = 0;
    for (int i = 0; i < hand.Count; i++){
      if (hand[i].type == "Foe" && hand[i].battlePoints < 25){
        count += 1;
      }
    }
    return (count >= 2);
  }

  public List<Card> playFoeEncounter(int stage, int stages, List<Card> hand, int previous, bool amour)
  {
    // if it is the final stage we play final foe (most powerful combination available)
    if (stage == stages){
      List<Card> foeEncounter = playFinalFoe(hand, amour);
      return foeEncounter;

    } else {
      // else we play the earlier foe strategy, where we try to increment by 10 based on the previous stage
      List<Card> foeEncounter = playEarlierFoe(hand, previous, amour);
      return foeEncounter;
    }
  }

  // earlier foe behavior, we try to play using the smallest cards possible until we play 10 more than the previous foe encounter
  public List<Card> playEarlierFoe(List<Card> hand, int previous, bool amour){
    // set our threshold
    int bpNeeded = previous + 10;
    // instantiate the list of cards were going to play and return
    List<Card> foeEncounter = new List<Card>();

    // if we haven't yet played the amour
    // check if we have an amour card and play it, deduct its BP from the bpNeeded
    if (amour == false){
      for(int i = 0; i < hand.Count; i++){
        if (hand[i].type == "Amour"){
          foeEncounter.Add(hand[i]);
          bpNeeded -= 10;
        }
      }
    }

    // make a list of the valid cards to play, non-duplicate weapons and Allies with more than 0 BP.
    List<Card> validCards = new List<Card>();
    for (int i = 0; i < hand.Count; i++){
      if (hand[i].type == "Weapon" && checkDuplicate(hand[i], validCards, "Weapon")){
        validCards.Add(hand[i]);
      }
      if (hand[i].type == "Ally" && hand[i].battlePoints > 0){
        validCards.Add(hand[i]);
      }
    }

    // sort in descending order of BP
    for (int x = 0; x <= validCards.Count; x++){
      for (int i = 0; i <= validCards.Count; i++){
        if (validCards[i].battlePoints > validCards[i + 1].battlePoints){
          var temp = validCards[i + 1];
          validCards[i + 1] = validCards[i];
          validCards[i] = temp;
        }
      }
    }

    // while we still need BP, loop through add the cards with the lowest BP possible
    int index = 0;
    while(bpNeeded > 0 && index < hand.Count) {
      bpNeeded -= validCards[index].battlePoints;
      foeEncounter.Add(validCards[index]);
      index ++;
    }

    // return the resulting list
    return foeEncounter;
  }

  public List<Card> playFinalFoe(List<Card> hand, bool amour){
    List<Card> foeEncounter = new List<Card>();

    for (int i = 0; i < hand.Count; i++){
      if (hand[i].type == "Weapon" && checkDuplicate(hand[i], foeEncounter, "Weapon")){
        foeEncounter.Add(hand[i]);
      }
      if (hand[i].type == "Ally" && hand[i].battlePoints > 0){
        foeEncounter.Add(hand[i]);
      }
      if (hand[i].type == "Amour" && (amour == false)){
        foeEncounter.Add(hand[i]);
      }
    }
    return foeEncounter;

  }


  // Test Strategy

  // wrapper function for playBid that simply gets the integer referring to the bid amount
  // Since this AI doesn't use AMOUR cards or Allies for extra bids, we can simply count the foe cards they have bid
  public int willIBid(int currentBid, List<Card> hand, int round)
  {
    // return the count of playBid
    List<Card> bid = playBid(hand, round);
    if (bid.Count <= currentBid){
      return 0;
    }
    return bid.Count;
  }

  // generates the list that is the actual bid to be played by the user, given the round they are bidding in
  public List<Card> playBid(List<Card> hand, int round)
  {
    // instantiate a list that represents the bid we're willing to play
    List<Card> bid = new List<Card>();
    // In Round 1 this AI will bid foes with less than 25 BP, no duplicates
    if (round == 1){
      for (int i = 0; i < hand.Count; i++){
        if ((hand[i].type == "Foe" && hand[i].minBP < 25) && checkDuplicate(hand[i], bid, "Foe")){
          bid.Add(hand[i]);
        }
      }
    }
    // in Round 2, this AI will bid the same way as round 1, except it will allow duplicates
    if (round == 2){
      for (int i = 0; i < hand.Count; i++){
        if (hand[i].type == "Foe" && hand[i].minBP < 25){
          bid.Add(hand[i]);
        }
      }
    }
    // return our bid as a list of cards
    return bid;
  }

  public List<Card> kingsCall(List<Card> hand)
  {
    return hand;
  }
}
