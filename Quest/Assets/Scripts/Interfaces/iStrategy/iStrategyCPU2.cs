using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStrategyCPU2 : iStrategy
{
  public iStrategyCPU2() {}
  // Tournament Strategy

  // Strategy #2, the CPU always joins tournaments
  public int participateInTourney(List<Player> players, int shields , GameController gameController)
  {
    return 1;
  }

  // We play the fewest cards possible to get to 50 or our best possible total
  public List<Card> playTournament(List<Player> players, List<Card> hand, int baseBP, int shields)
  {
    strategyUtil strat = new strategyUtil();
    // Generate a list of valid cards --> weapon, ally, and amour
    List<Card> validCards = new List<Card>();
    for (var i = 0; i < hand.Count; i++) {
      var type = hand[i].type;
      if (type == "Ally Card" || type == "Weapon Card" || type == "Amour Card"){
        validCards.Add(hand[i]);
      }
    }
    validCards = strat.sortAllValidCardsByDescendingBP(validCards, players, "");
    // get how much BP we need left by subtracting our base
    int bpNeeded = 50 - baseBP;

    // instantiate the list of cards were returning to play
    List<Card> cardsToPlay = new List<Card>();

    // index = 0, because were trying to move through the validCards one at a time
    int index = 0;

    // until either we run out of validCards or we have gone above the BP threshold we wish to hit (50)
    while (bpNeeded > 0 && index < validCards.Count){
      // first make sure that we still have cards in validCards to evaluate
        // check that the card were trying to play isn't a duplicate weapon
      if (strat.checkDuplicate(validCards[index], cardsToPlay, "Weapon Card")){
        // add the card to our cards to be played
        bpNeeded -= strat.getValidCardBP(validCards[index], players, "");
        cardsToPlay.Add(validCards[index]);
        hand.Remove(validCards[index]);
      }
      index ++;
      }
      return cardsToPlay;
    }

  // Quest Strategy
  public int sponsorQuest(List<Player> players, int stages, List<Card> hand, GameController game)
  {
    strategyUtil strat = new strategyUtil();
    // if somebody can rank up, we return false to decline sponsoring the quest
    if (strat.canSomeoneRankUp(players, stages)) {
      return 0;
    }

    if (strat.canISponsor(hand, stages))
    {
        return 1;
    }
        return 0;
  }

  // Return a list of list the count of stages, where at each index is the card(s) that sets up each stage
  public List<List<Card>> setupQuest(int stages, List<Card> hand, string questFoe)
  {
    strategyUtil strat = new strategyUtil();
    // instantiate the quest line
    List<List<Card>> questLine = new List<List<Card>>();

    // create the final stage first
    List<Card> finalStage = setupFoeStage(stages, stages, hand, questFoe);

    // if we have a test, create a test stage and then work from the first stage to fill in the foe stages
    if (strat.haveTest(hand)){
      List<Card> testStage = setupTestStage(hand);
      for (int i = 0; i < (stages - 2); i++){
        List<Card> foeStage = setupFoeStage(i, stages, hand, questFoe);
        questLine.Add(foeStage);
      }
      questLine.Add(testStage);
      // else we don't have a test, we fill in foe stages the same way but 1 more for the missing test
    } else {
      for (int i = 0; i < (stages - 1); i++){
        List<Card> foeStage = setupFoeStage(i, stages, hand, questFoe);
        questLine.Add(foeStage);
      }
    }
    // add our final Stage that we created first so its on the end and return
    questLine.Add(finalStage);
    return questLine;
  }

  // evaluates whether its the last stage or not and sets up either a Final Foe stage or an early Foe Stage
  public List<Card> setupFoeStage(int currentStage, int stages, List<Card> hand, string questFoe)
  {
    if (currentStage == stages){
      return setUpFinalFoe(hand, questFoe);
    }
    return setUpEarlyFoeEncounter(hand, questFoe);
  }


  // sets up the final stage of a quest for this CPU player, in this strategy:
  // we need to get 40BP in as few cards as possible
  public List<Card> setUpFinalFoe(List<Card> hand, string questFoe)
  {
    strategyUtil strat = new strategyUtil();

    // instantiate a List of foes and weapons from the user's hand
    List<Card> foes = new List<Card>();
    List<Card> weapons = new List<Card>();

    // seperate the foes and weapons into their own lists from the hand
    for (var i = 0; i < hand.Count; i++){
      if (hand[i].type == "Foe Card"){
        foes.Add(hand[i]);
      }
      // make sure that we sort out weapons that are already in the weapons
      if (hand[i].type == "Weapon Card" && strat.checkDuplicate(hand[i], weapons, "Weapon Card")){
        weapons.Add(hand[i]);
      }
    }
    foes = strat.sortFoesByDescendingOrder(foes, questFoe);
    weapons = strat.sortWeaponsByDescendingOrder(weapons);

    // instantiate the foeEncounter list
    List<Card> foeEncounter = new List<Card>();

    // subtract the foe with the MOST BP in the user's hand from 40, the AI threshold
    FoeCard strongFoe = (FoeCard)foes[0];
    int bpNeeded = (40 - strongFoe.minBP);
    // Add this foe to the foeEncounter as the foe to be played
    foeEncounter.Add(foes[0]);
    hand.Remove(foes[0]);

    // initialize index as 0 to loop through the weapons
    int index = 0;

    // while we still need BP toreach our 40 threshold
    while(bpNeeded > 0 && index < weapons.Count){
      // if we still have weapons to loop through
        // subtract the BP of the next most powerful weapon from the threshold
      WeaponCard nextWeapon = (WeaponCard)weapons[index];
      bpNeeded -= nextWeapon.battlePoints;
        // add this weapon to the encounter
      foeEncounter.Add(weapons[index]);
      hand.Remove(weapons[index]);
        // increment index
      index++;
    }

    // return the most powerful foe we have with the set of weapons that most quickly gets us to 40 BP.
    return foeEncounter;
  }


  // This sets up an early Foe Encounter, a Foe encounter before the last round of a Quest.
  // This strategy is to simply play the lowest BP foe from the hand with NO weapons attached
  public List<Card> setUpEarlyFoeEncounter(List<Card> hand, string questFoe)
  {
    strategyUtil strat = new strategyUtil();
    // get the list of foe cards from the user's hand
    List<Card> foes = new List<Card>();
    for (var i = 0; i < hand.Count; i++){
      if (hand[i].type == "Foe Card"){
        foes.Add(hand[i]);
      }
    }

    foes = strat.sortFoesByAscendingOrder(foes, questFoe);

    // make a list, add the lowest BP foe and return it
    List<Card> foeEncounter = new List<Card>();
    foeEncounter.Add(foes[0]);
    hand.Remove(foes[0]);
    return foeEncounter;
  }

// returns the test card from the user's hand with the highest minimum
  public List<Card> setupTestStage(List<Card> hand)
  {
    strategyUtil strat = new strategyUtil();
    List<Card> tests = new List<Card>();
    for (int i = 0; i < hand.Count; i++){
      if (hand[i].type == "Test Card"){
        tests.Add(hand[i]);
      }
    }
    tests = strat.sortTetstsbyAscendingOrder(tests);
    List<Card> test = new List<Card>();
    // return a list with only the highest minimum test
    test.Add(tests[0]);
    hand.Remove(test[0]);
    return test;
    // get the test card with the highest bid test card in the hand
  }


    public int participateInQuest(int stages, List<Card> hand, GameController game)
    {
        // Simply calls both the tests to see if the CPU can play sufficient BP to progress and discard Foes for a Test
        if ((canIIncrement(stages, hand, game.players, game.currentQuest) && canIDiscard(hand)))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

  public bool canIIncrement(int stages, List<Card> hand, List<Player> players, QuestCard card)
  {

    strategyUtil strat = new strategyUtil();
    int firstStageToFill = 1;
    bool hasAmour = false;
    int prev = 0;

    List<Card> allies = new List<Card>();
    List<Card> weapons = new List<Card>();
    // Seperately fill lists with our weapons and allies
    for (int i = 0; i < hand.Count; i++){
      if (hand[i].type == "Weapon Card"){
        weapons.Add(hand[i]);
      }
      // If we have an Amour Card, it will immediately be played meaning we start filling stages from round 2 (bc round 1 will be 10BP)
      if (hand[i].type == "Amour Card" && (hasAmour == false)){
        hasAmour = true;
        firstStageToFill = 2;
        prev = 10;
      }
      if (hand[i].type == "Ally Card"){
        AllyCard ally = (AllyCard)hand[i];
        if (ally.getBattlePoints(card.name, players) > 0){
          allies.Add(hand[i]);
        }
      }
    }
    // sort them by ascending order
    allies = strat.sortAlliesByAscendingOrder(allies, card.name, players);
    weapons = strat.sortWeaponsByAscendingOrder(weapons);

    // Now we walk through each stage of the quest
    for (int i = firstStageToFill; i <= stages; i++){
      // instantiate a list of unique weapon to ensure each stage doesn't have duplicate weapons
      List<Card> uniqueWeapons = new List<Card>();
      // set our threshold for how many BP this stage needs (incremented by 10)
      int pointThreshold = (10 + prev);
      // our current points for this stage
      int points = 0;
      // while we still have allies to play and we have yet to hit our point threshold
      while (allies.Count > 0 && (points < pointThreshold)){
        // play our allies and increment points to include their battlePoints
        AllyCard ally = (AllyCard)allies[0];
        points += ally.getBattlePoints(card.name, players);
        allies.Remove(allies[0]);
      }
      // if we run out of allies, we can loop through weapons and make sure that were not gonna play a duplicate weapon
      while (weapons.Count > 0 && (points < pointThreshold) && strat.checkDuplicate(weapons[0], uniqueWeapons, "Weapon Card")){
        WeaponCard weapon = (WeaponCard)weapons[0];
        points += weapon.battlePoints;
        weapons.Remove(weapons[0]);
      }
      // if we've run out of allies AND weapons but haven't filled in this current stage yet then return false
      if (points < pointThreshold){
        return false;
      }
      // set prev to whatever our points ended up as
      prev = points;
    }
    // return true if weve made it through all stages
    return true;
  }


  // checks that the player has at LEAST 2 foes with less than 25 BP that they can discard if need be
  public bool canIDiscard(List<Card> hand){
    int count = 0;
    for (int i = 0; i < hand.Count; i++){
      if (hand[i].type == "Foe Card"){ // && hand[i].battlePoints < 25){
        FoeCard foe = (FoeCard)hand[i];
        if (foe.minBP < 25){
          count += 1;
        }
      }
    }
    return (count >= 2);
  }

  public List<Card> playFoeEncounter(int stage, int stages, List<Card> hand, int previous, bool amour, string questName, List<Player> players)
  {
    // if it is the final stage we play final foe (most powerful combination available)
    if (stage == stages){
      return playFinalFoe(hand, amour);

    } else {
      // else we play the earlier foe strategy, where we try to increment by 10 based on the previous stage
      return playEarlierFoe(hand, previous, amour, questName, players);
    }
  }

  // earlier foe behavior, we try to play using the smallest cards possible until we play 10 more than the previous foe encounter
  public List<Card> playEarlierFoe(List<Card> hand, int previous, bool amour, string questName, List<Player> players){
    strategyUtil strat = new strategyUtil();
    // set our threshold
    int bpNeeded = previous + 10;
    // instantiate the list of cards were going to play and return
    List<Card> foeEncounter = new List<Card>();

    // if we haven't yet played the amour
    // check if we have an amour card and play it, deduct its BP from the bpNeeded
    if (amour == false){
      for(int i = 0; i < hand.Count; i++){
        if (hand[i].type == "Amour Card"){
          foeEncounter.Add(hand[i]);
          bpNeeded -= 10;
        }
      }
    }

    List<Card> weapons = new List<Card>();
    List<Card> allies = new List<Card>();

    // make a list of the valid cards to play, non-duplicate weapons and Allies with more than 0 BP.
    List<Card> validCards = new List<Card>();
    for (int i = 0; i < hand.Count; i++){
      if (hand[i].type == "Weapon Card" && strat.checkDuplicate(hand[i], weapons, "Weapon Card")){
        weapons.Add(hand[i]);
      }
      if (hand[i].type == "Ally Card"){
        AllyCard ally = (AllyCard)hand[i];
        if (ally.getBattlePoints(questName, players) > 0){
          allies.Add(hand[i]);
        }
      }
    }

    // sort them by ascending order
    allies = strat.sortAlliesByAscendingOrder(allies, questName, players);
    weapons = strat.sortWeaponsByAscendingOrder(weapons);

    while (allies.Count > 0 && bpNeeded > 0){
      foeEncounter.Add(allies[0]);
      AllyCard ally = (AllyCard)allies[0];
      bpNeeded -= ally.getBattlePoints(questName, players);
      allies.Remove(allies[0]);
    }

    while (weapons.Count > 0 && bpNeeded > 0){
      foeEncounter.Add(weapons[0]);
      WeaponCard weapon = (WeaponCard)weapons[0];
      bpNeeded -= weapon.battlePoints;
      weapons.Remove(weapons[0]);
    }
    // return the resulting list
    return foeEncounter;
  }

  public List<Card> playFinalFoe(List<Card> hand, bool amour){
    strategyUtil strat = new strategyUtil();
    List<Card> foeEncounter = new List<Card>();

    for (int i = 0; i < hand.Count; i++){
      if (hand[i].type == "Weapon Card" && strat.checkDuplicate(hand[i], foeEncounter, "Weapon Card")){
        foeEncounter.Add(hand[i]);
      }
      if (hand[i].type == "Ally Card"){// && hand[i].battlePoints > 0){
        AllyCard ally = (AllyCard)hand[i];
        if (ally.battlePoints > 0){
          foeEncounter.Add(hand[i]);
        }
      }
      if (hand[i].type == "Amour Card" && (amour == false)){
        foeEncounter.Add(hand[i]);
      }
    }
    for (int i = 0; i < foeEncounter.Count; i++)
    {
      hand.Remove(foeEncounter[i]);
    }
    return foeEncounter;

  }


  // Test Strategy

  // wrapper function for playBid that simply gets the integer referring to the bid amount
  // Since this AI doesn't use AMOUR cards or Allies for extra bids, we can simply count the foe cards they have bid
  public int willIBid(int currentBid, List<Card> hand, int round, GameController game)
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
    strategyUtil strat = new strategyUtil();
    // instantiate a list that represents the bid we're willing to play
    List<Card> bid = new List<Card>();
    // In Round 1 this AI will bid foes with less than 25 BP, no duplicates
    if (round == 1){
      for (int i = 0; i < hand.Count; i++){
        if ((hand[i].type == "Foe Card" && strat.checkDuplicate(hand[i], bid, "Foe Card"))){ //        hand[i].minBP < 25)
          FoeCard foe = (FoeCard)hand[i];
          if (foe.minBP < 25){
            bid.Add(hand[i]);
          }
        }
      }
    }
    // in Round 2, this AI will bid the same way as round 1, except it will allow duplicates
    if (round == 2){
      for (int i = 0; i < hand.Count; i++){
        if (hand[i].type == "Foe Card"){ // && hand[i].minBP < 25){
          FoeCard foe = (FoeCard)hand[i];
          if (foe.minBP < 25){
            bid.Add(hand[i]);
          }
        }
      }
    }
    // return our bid as a list of cards
    return bid;
  }

  public List<Card> discardWeapon(List<Card> hand)
  {
    strategyUtil strat = new strategyUtil();
    return strat.discardWeapon(hand);
  }

  // Discard 2 lowest BP foes for the King's Call Event
  public List<Card> discardFoesForKing(List<Card> hand)
  {
    strategyUtil strat = new strategyUtil();
    return strat.discardFoesForKing(hand);
  }




    //Called when there is a binary resposne required from the player
    public int respondToPrompt(GameController game)
    {

        //still checking
        //Debug.Log("still checking");
        return 2;
    }

    public List<Card> fixHandDiscrepancy(List<Card> hand){
      strategyUtil strat = new strategyUtil();
      List<Card> toDiscard = strat.fixHandCPU(hand);
      return toDiscard;
    }
}
