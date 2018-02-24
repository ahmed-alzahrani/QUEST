using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStrategyCPU1 : iStrategy
{
    public iStrategyCPU1() { }

    // Tournament Strategy

    // Strategy #1, the player participates if anyone, including themselves, can stand to rank up
    public int participateInTourney(List<Player> players, int shields, GameController game)
    {
        strategyUtil strat = new strategyUtil();
        if (strat.canSomeoneRankUp(players, shields))
        {
            return 1;
        }
        return 0;
    }

    public List<Card> playTournament(List<Player> players, List<Card> hand, int baseBP, int shields)
    {
        strategyUtil strat = new strategyUtil();
        // This AI evaluates wheteher there is a potential rank up on this tournament
        if (strat.canSomeoneRankUp(players, shields))
        {
            // If there is, he considers it high stakes and plays his strongest hand
            return (highStakesTournament(hand, players));
        }
        // else he considers it low stakes and plays only duplicate weapons
        return lowStakesTournament(hand);
    }

    // high stakes Tournament returns the strongest possible play the CPU can make
    public List<Card> highStakesTournament(List<Card> hand, List<Player> players)
    {
        strategyUtil strat = new strategyUtil();
        List<Card> cardsToPlay = new List<Card>();
        // loop through the hand
        for (int i = 0; i < hand.Count; i++)
        {
            // play the amour card if we haven't already
            if (hand[i].type == "Amour Card" && strat.checkDuplicate(hand[i], cardsToPlay, "Amour Card"))
            {
                cardsToPlay.Add(hand[i]);
                hand.Remove(hand[i]);
            }
            // play the weapon card if we haven't already played a weapon of that type
            if (hand[i].type == "Weapon Card" && strat.checkDuplicate(hand[i], cardsToPlay, "Weapon Card"))
            {
                cardsToPlay.Add(hand[i]);
                hand.Remove(hand[i]);
            }
            // play any ally card with BP
            if (hand[i].type == "Ally Card")
            {
                AllyCard ally = (AllyCard)hand[i];
                if (ally.getBattlePoints("", players) > 0)
                {
                    cardsToPlay.Add(hand[i]);
                    hand.Remove(hand[i]);

                }
            }
        }
        // return all the eligible cards to play
        return cardsToPlay;
    }

    // low stakes tournament, will only play weapons of which there are 2 or more
    public List<Card> lowStakesTournament(List<Card> hand)
    {
        strategyUtil strat = new strategyUtil();
        List<Card> cardsToPlay = new List<Card>();

        // loop through the hand
        for (int i = 0; i < hand.Count; i++)
        {
            // if it is a weapon and our hand has multiple we play it
            if (hand[i].type == "Weapon Card" && strat.hasMultiple(hand, hand[i].name) && strat.checkDuplicate(hand[i], cardsToPlay, "Weapon Card"))
            {
                cardsToPlay.Add(hand[i]);
                hand.Remove(hand[i]);
            }
        }
        return cardsToPlay;
    }

    // Quest Strategy
    public int sponsorQuest(List<Player> players, int stages, List<Card> hand, GameController game)
    {
        strategyUtil strat = new strategyUtil();
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
        // instantiate the quest line
        strategyUtil strat = new strategyUtil();
        List<List<Card>> questLine = new List<List<Card>>();

        // create the final stage first
        List<Card> finalStage = setupFoeStage(stages, stages, hand, questFoe);
        questLine.Add(finalStage);

        // if we have a test, create a test stage and then work from the first stage to fill in the foe stages
        if (strat.haveTest(hand))
        {
            List<Card> testStage = setupTestStage(hand);
            questLine.Add(testStage);
            for (int i = 0; i < (stages - 2); i++)
            {
                List<Card> foeStage = setupFoeStage(i, stages, hand, questFoe);
                questLine.Add(foeStage);
            }
            // else we don't have a test, we fill in foe stages the same way but 1 more for the missing test
        }
        else {
            for (int i = 0; i < (stages - 1); i++)
            {
                List<Card> foeStage = setupFoeStage(i, stages, hand, questFoe);
                questLine.Add(foeStage);
            }
        }
        questLine.Reverse();
        return questLine;
    }

    public List<Card> setupFoeStage(int currentStage, int stages, List<Card> hand, string questFoe)
    {
        if (currentStage == stages)
        {
            return setUpFinalFoe(hand, questFoe);
        }
        return setUpEarlyFoeEncounter(hand, questFoe);
    }

    public List<Card> setUpEarlyFoeEncounter(List<Card> hand, string questFoe)
    {
        strategyUtil strat = new strategyUtil();
        List<Card> foeEncounter = new List<Card>();
        List<Card> foes = new List<Card>();

        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].type == "Foe Card")
            {
                foes.Add(hand[i]);
            }
        }

        foes = strat.sortFoesByDescendingOrder(foes, questFoe);
        foeEncounter.Add(foes[0]);
        hand.Remove(foes[0]);

        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].type == "Weapon Card" && strat.hasMultiple(hand, hand[i].name) && strat.checkDuplicate(hand[i], foeEncounter, "Weapon Card"))
            {
                foeEncounter.Add(hand[i]);
                hand.Remove(hand[i]);
            }
        }
        return foeEncounter;
    }

    // sets up the final stage of a quest for this CPU player, in this strategy:
    // we need to get 50BP in as few cards as possible
    public List<Card> setUpFinalFoe(List<Card> hand, string questFoe)
    {
        strategyUtil strat = new strategyUtil();
        // instantiate a List of foes and weapons from the user's hand
        List<Card> foes = new List<Card>();
        List<Card> weapons = new List<Card>();

        // seperate the foes and weapons into their own lists from the hand
        for (var i = 0; i < hand.Count; i++)
        {
            if (hand[i].type == "Foe Card")
            {
                foes.Add(hand[i]);
            }
            // make sure that we sort out weapons that are already in the weapons
            if (hand[i].type == "Weapon Card" && strat.checkDuplicate(hand[i], weapons, "Weapon Card"))
            {
                weapons.Add(hand[i]);
            }
        }

        foes = strat.sortFoesByDescendingOrder(foes, questFoe);
        weapons = strat.sortWeaponsByDescendingOrder(weapons);

        // instantiate the foeEncounter list
        List<Card> foeEncounter = new List<Card>();

        // subtract the foe with the MOST BP in the user's hand from 40, the AI threshold
        FoeCard firstFoe = (FoeCard)foes[0];
        int bpNeeded = (50 - firstFoe.minBP);
        // Add this foe to the foeEncounter as the foe to be played
        foeEncounter.Add(foes[0]);
        hand.Remove(foes[0]);

        // initialize index as 0 to loop through the weapons
        int index = 0;

        // while we still need BP toreach our 50 threshold
        while (bpNeeded > 0 && index < weapons.Count)
        {
            // if we still have weapons to loop through
            // subtract the BP of the next most powerful weapon from the threshold
            WeaponCard weapon = (WeaponCard)weapons[index];
            bpNeeded -= weapon.battlePoints;
            // add this weapon to the encounter
            foeEncounter.Add(weapons[index]);
            hand.Remove(weapons[index]);
            // increment index
            index++;
        }

        // return the most powerful foe we have with the set of weapons that most quickly gets us to 50 BP.
        return foeEncounter;
    }

    public List<Card> setupTestStage(List<Card> hand)
    {
        strategyUtil strat = new strategyUtil();
        List<Card> tests = new List<Card>();
        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].type == "Test Card")
            {
                tests.Add(hand[i]);
            }
        }
        tests = strat.sortTetstsbyAscendingOrder(tests);
        List<Card> test = new List<Card>();
        test.Add(tests[0]);
        return test;
        // get the test card with the highest bid test card in the hand
    }

    public int participateInQuest(int stages, List<Card> hand, GameController game)
    {
        // Do I have 2 weapons/allies per stage, And 2 foes of 20 or less BP for a test?
        //  return canIPlay(stages, hand) && canIDiscard(hand);
        if (canIPlay(stages, hand) && canIDiscard(hand))
        {
            return 1;
        }
        return 0;
    }

    public bool canIPlay(int stages, List<Card> hand)
    {
        int count = (2 * stages);

        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].type == "Weapon Card")
            {
                count -= 1;
            }
            if (hand[i].type == "Ally Card")
            {
                AllyCard ally = (AllyCard)hand[i];
                if (ally.battlePoints > 0)
                {
                    count -= 1;
                }
            }
        }
        return count <= 0;
    }

    public bool canIDiscard(List<Card> hand)
    {
        int count = 0;
        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].type == "Foe Card")
            {
                FoeCard foe = (FoeCard)hand[i];
                if (foe.minBP < 20)
                {
                    count += 1;
                }
            }
        }

        return (count >= 2);
    }

    public List<Card> playFoeEncounter(int stage, int stages, List<Card> hand, int previous, bool amour, string questName, List<Player> players)
    {
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

    // Strategy 1 for an earlier foe encounter is as follows:

    /*
    1. Sort Amour/Allies/Weapons in decreasing order of BP

    2. Play 2 if (if possible) allies /amours,

    */
    public List<Card> playEarlierFoe(List<Card> hand, int previous, bool amour, string questName, List<Player> players)
    {
        strategyUtil strat = new strategyUtil();
        List<Card> allies = new List<Card>();
        List<Card> weapons = new List<Card>();
        List<Card> foeEncounter = new List<Card>();
        int count = 0;
        int amourCount = 0;

        for (int i = 0; i < hand.Count; i++)
        {
            if (amour == false)
            {
                if (hand[i].type == "Amour Card")
                {
                    foeEncounter.Add(hand[i]);
                    amourCount += 1;
                    amour = true;
                }
            }
            if (hand[i].type == "Ally Card")
            { // && hand[i].battlePoints > 0){
                AllyCard ally = (AllyCard)hand[i];
                if (ally.battlePoints > 0)
                {
                    count++;
                    allies.Add(hand[i]);
                }
            }
            if (hand[i].type == "Weapon Card")
            {
              weapons.Add(hand[i]);
            }
        }

        if (count >= 2)
        {
            allies = strat.sortAlliesByAscendingOrder(allies, questName, players);
            foeEncounter.Add(allies[0]);
            hand.Remove(allies[0]);
            if (foeEncounter.Count == 2)
            {
                return foeEncounter;
            }
            foeEncounter.Add(allies[1]);
            hand.Remove(allies[1]);
        }
        else {
            for (int i = 0; i < allies.Count; i++)
            {
                foeEncounter.Add(allies[i]);
            }
            weapons = strat.sortWeaponsByAscendingOrder(weapons);
            int index = 0;
            while (foeEncounter.Count < 2 && index < weapons.Count)
            {
                foeEncounter.Add(weapons[index]);
                index++;
            }
        }
        return foeEncounter;
    }

    public List<Card> playFinalFoe(List<Card> hand, bool amour)
    {
        strategyUtil strat = new strategyUtil();
        List<Card> foeEncounter = new List<Card>();

        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].type == "Weapon Card" && strat.checkDuplicate(hand[i], foeEncounter, "Weapon Card"))
            {
                foeEncounter.Add(hand[i]);
            }
            if (hand[i].type == "Ally Card")
            { // && hand[i].battlePoints > 0){
                AllyCard ally = (AllyCard)hand[i];
                if (ally.battlePoints > 0)
                {
                    foeEncounter.Add(hand[i]);
                }
            }
            if (hand[i].type == "Amour Card" && (amour == false))
            {
                foeEncounter.Add(hand[i]);
            }
        }
        return foeEncounter;

    }


    // Test Strategy

    public int willIBid(int currentBid, List<Card> hand, int round, GameController game)
    {
        // return the count of playBid
        List<Card> bid = playBid(hand, round);
        for (int i = 0; i < bid.Count; i++)
        {
          bid[i].display();
        }
        if (bid.Count > currentBid)
        {
            return bid.Count;
        }
        return 0;
    }

    public List<Card> playBid(List<Card> hand, int round)
    {
        List<Card> bid = new List<Card>();
        if (round == 2)
        {
            return bid;
        }
        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].type == "Foe Card")
            { // && hand[i].battlePoints < 20){
                FoeCard foe = (FoeCard)hand[i];
                if (foe.minBP < 20)
                {
                    bid.Add(hand[i]);
                }
            }
        }
        return bid;
    }

    // Discards the weapon with the lowest BP
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

}
