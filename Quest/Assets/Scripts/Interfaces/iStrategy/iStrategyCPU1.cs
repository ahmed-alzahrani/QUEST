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
        if (canSomeoneRankUp(players, shields))
        {
            return 1;
        }
        return 0;
    }

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


    public List<Card> playTournament(List<Player> players, List<Card> hand, int baseBP, int shields)
    {
        // This AI evaluates wheteher there is a potential rank up on this tournament
        if (canSomeoneRankUp(players, shields))
        {
            // If there is, he considers it high stakes and plays his strongest hand
            return (highStakesTournament(hand));
        }
        // else he considers it low stakes and plays only duplicate weapons
        return lowStakesTournament(hand);
    }

    // high stakes Tournament returns the strongest possible play the CPU can make
    public List<Card> highStakesTournament(List<Card> hand)
    {
        List<Card> cardsToPlay = new List<Card>();
        // loop through the hand
        for (int i = 0; i < hand.Count; i++)
        {
            // play the amour card if we haven't already
            if (hand[i].type == "Amour Card" && checkDuplicate(hand[i], cardsToPlay, "Amour Card"))
            {
                cardsToPlay.Add(hand[i]);
                hand.Remove(hand[i]);
            }
            // play the weapon card if we haven't already played a weapon of that type
            if (hand[i].type == "Weapon Card" && checkDuplicate(hand[i], cardsToPlay, "Weapon Card"))
            {
                cardsToPlay.Add(hand[i]);
                hand.Remove(hand[i]);
            }
            // play any ally card with BP
            if (hand[i].type == "Ally Card")
            {
                AllyCard ally = (AllyCard)hand[i];
                if (ally.battlePoints > 0)
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
        List<Card> cardsToPlay = new List<Card>();

        // loop through the hand
        for (int i = 0; i < hand.Count; i++)
        {
            // if it is a weapon and our hand has multiple we play it
            if (hand[i].type == "Weapon Card" && hasMultiple(hand, hand[i].name) && checkDuplicate(hand[i], cardsToPlay, "Weapon Card"))
            {
                cardsToPlay.Add(hand[i]);
                hand.Remove(hand[i]);
            }
        }
        return cardsToPlay;
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


    // Quest Strategy
    public int sponsorQuest(List<Player> players, int stages, List<Card> hand, GameController game)
    {
        if (canSomeoneRankUp(players, stages))
        {
            return 0;
        }

        if (canISponsor(hand, stages))
        {
            return 1;
        }
        return 0;
    }

    public bool canISponsor(List<Card> hand, int stages)
    {
        // see if we have enough cards to sponsor the quest
        // Count through our hand and subtract
        int stageCount = stages;
        List<Card> validCards = new List<Card>();
        bool haveTest = false;
        for (var i = 0; i < hand.Count; i++)
        {
            // count up our foes to compare to the # of stages
            if (hand[i].type == "Foe Card" && checkDuplicate(hand[i], validCards, "Foe Card"))
            {
                validCards.Add(hand[i]);
            }
            // if we have a Test card in our hand we set the bool to true
            if (hand[i].type == "Test Card")
            {
                haveTest = true;
            }
        }
        // decreate stageCount by one more if we had a Test
        if (haveTest)
        {
            stageCount -= 1;
        }
        return (stageCount <= validCards.Count);
    }

    public List<List<Card>> setupQuest(int stages, List<Card> hand)
    {
        // instantiate the quest line
        List<List<Card>> questLine = new List<List<Card>>();

        // create the final stage first
        List<Card> finalStage = setupFoeStage(stages, stages, hand);
        questLine.Add(finalStage);

        // if we have a test, create a test stage and then work from the first stage to fill in the foe stages
        if (haveTest(hand))
        {
            List<Card> testStage = setupTestStage(hand);
            questLine.Add(testStage);
            for (int i = 0; i < (stages - 2); i++)
            {
                List<Card> foeStage = setupFoeStage(i, stages, hand);
                questLine.Add(foeStage);
            }
            // else we don't have a test, we fill in foe stages the same way but 1 more for the missing test
        }
        else {
            for (int i = 0; i < (stages - 1); i++)
            {
                List<Card> foeStage = setupFoeStage(i, stages, hand);
                questLine.Add(foeStage);
            }
        }
        questLine.Reverse();
        return questLine;
    }

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

    public List<Card> setupFoeStage(int currentStage, int stages, List<Card> hand)
    {
        if (currentStage == stages)
        {
            return setUpFinalFoe(hand);
        }
        return setUpEarlyFoeEncounter(hand);
    }

    public List<Card> setUpEarlyFoeEncounter(List<Card> hand)
    {
        List<Card> foeEncounter = new List<Card>();
        List<Card> foes = new List<Card>();

        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].type == "Foe Card")
            {
                foes.Add(hand[i]);
            }
        }

        // bubble sort the foes from the player's hand in ascending order
        for (int x = 0; x < foes.Count; x++)
        {
            for (int i = 0; i < (foes.Count - 1); i++)
            {
                FoeCard foe1 = (FoeCard)foes[i];
                FoeCard foe2 = (FoeCard)foes[i + 1];
                if (foe1.minBP < foe2.minBP)
                {
                    var temp = foes[i + 1];
                    foes[i + 1] = foes[i];
                    foes[i] = temp;
                }
            }
        }

        foeEncounter.Add(foes[0]);
        hand.Remove(foes[0]);

        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].type == "Weapon Card" && hasMultiple(hand, hand[i].name) && checkDuplicate(hand[i], foeEncounter, "Weapon Card"))
            {
                foeEncounter.Add(hand[i]);
                hand.Remove(hand[i]);
            }
        }
        return foeEncounter;
    }

    // sets up the final stage of a quest for this CPU player, in this strategy:
    // we need to get 50BP in as few cards as possible
    public List<Card> setUpFinalFoe(List<Card> hand)
    {
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
            if (hand[i].type == "Weapon Card" && checkDuplicate(hand[i], weapons, "Weapon Card"))
            {
                weapons.Add(hand[i]);
            }
        }


        // bubble sort the foes from the player's hand in ascending order
        for (int x = 0; x < foes.Count; x++)
        {
            for (int i = 0; i < (foes.Count - 1); i++)
            {
                FoeCard foe1 = (FoeCard)foes[i];
                FoeCard foe2 = (FoeCard)foes[i + 1];
                if (foe1.minBP < foe2.minBP)
                {
                    var temp = foes[i + 1];
                    foes[i + 1] = foes[i];
                    foes[i] = temp;
                }
            }
        }

        // bubble sort the weapons from the player's hand in ascending order
        for (int x = 0; x < weapons.Count; x++)
        {
            for (int i = 0; i < (weapons.Count - 1); i++)
            {
                WeaponCard weapon1 = (WeaponCard)weapons[i];
                WeaponCard weapon2 = (WeaponCard)weapons[i + 1];
                if (weapon1.battlePoints < weapon2.battlePoints)
                {
                    var temp = weapons[i + 1];
                    weapons[i + 1] = weapons[i];
                    weapons[i] = temp;
                }
            }
        }

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
        List<Card> tests = new List<Card>();
        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].type == "Test Card")
            {
                tests.Add(hand[i]);
            }
        }

        // bubble sort the foes from the player's hand in ascending order
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

    public List<Card> playFoeEncounter(int stage, int stages, List<Card> hand, int previous, bool amour)
    {
        if (stage == stages)
        {
            List<Card> foeEncounter = playFinalFoe(hand, amour);
            return foeEncounter;
        }
        else {
            List<Card> foeEncounter = playEarlierFoe(hand, previous, amour);
            return foeEncounter;
        }
    }

    // Strategy 1 for an earlier foe encounter is as follows:

    /*
    1. Sort Amour/Allies/Weapons in decreasing order of BP

    2. Play 2 if (if possible) allies /amours,

    */
    public List<Card> playEarlierFoe(List<Card> hand, int previous, bool amour)
    {
        List<Card> validCards = new List<Card>();
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
                    validCards.Add(hand[i]);
                }
            }
        }

        if (count >= 2)
        {
            // bubble sort the valid cards in ascending order
            for (int x = 0; x < validCards.Count; x++)
            {
                for (int i = 0; i < (validCards.Count - 1); i++)
                {
                    AllyCard ally1;
                    AllyCard ally2;
                    ally1 = (AllyCard)validCards[i];
                    ally2 = (AllyCard)validCards[i + 1];
                    if (ally1.battlePoints > ally2.battlePoints)
                    {
                        var temp = validCards[i + 1];
                        validCards[i + 1] = validCards[i];
                        validCards[i] = temp;
                    }
                }
            }
            foeEncounter.Add(validCards[0]);
            hand.Remove(validCards[0]);
            if (foeEncounter.Count == 2)
            {
                return foeEncounter;
            }
            foeEncounter.Add(validCards[1]);
            hand.Remove(validCards[1]);
        }
        else {
            for (int i = 0; i < validCards.Count; i++)
            {
                foeEncounter.Add(validCards[i]);
            }

            List<Card> weapons = getWeapons(hand);
            int index = 0;
            while (foeEncounter.Count < 2 && index < weapons.Count)
            {
                foeEncounter.Add(weapons[index]);
                index++;
            }
        }
        return foeEncounter;
    }

    public List<Card> getWeapons(List<Card> hand)
    {
        List<Card> weapons = new List<Card>();
        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].type == "Weapon Card")
            {
                weapons.Add(hand[i]);
            }
        }
        for (int x = 0; x < weapons.Count; x++)
        {
            for (int i = 0; i < (weapons.Count - 1); i++)
            {
                WeaponCard weapon1 = (WeaponCard)weapons[i];
                WeaponCard weapon2 = (WeaponCard)weapons[i + 1];
                if (weapon1.battlePoints > weapon2.battlePoints)
                {
                    var temp = weapons[i + 1];
                    weapons[i + 1] = weapons[i];
                    weapons[i] = temp;
                }
            }
        }
        return weapons;
    }

    public List<Card> playFinalFoe(List<Card> hand, bool amour)
    {
        List<Card> foeEncounter = new List<Card>();

        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].type == "Weapon Card" && checkDuplicate(hand[i], foeEncounter, "Weapon Card"))
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
        List<Card> weapons = new List<Card>();
        List<Card> discard = new List<Card>();

        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].type == "Weapon Card")
            {
                weapons.Add(hand[i]);
            }
        }

        // bubble sort weapons into ascending order
        for (int x = 0; x < weapons.Count; x++)
        {
            for (int i = 0; i < (weapons.Count - 1); i++)
            {
                WeaponCard weapon1 = (WeaponCard)weapons[i];
                WeaponCard weapon2 = (WeaponCard)weapons[i + 1];
                if (weapon1.battlePoints > weapon2.battlePoints)
                {
                    var temp = weapons[i + 1];
                    weapons[i + 1] = weapons[i];
                    weapons[i] = temp;
                }
            }
        }

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
}