using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Maybe make the functions static
public class GameUtil
{
    //Utility class that stores some behavior functions

    //check if a player has over 12 cards
    public static bool PlayerOffending(List<Player> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].handCheck())
            {
                // we are over 12 cards on a player 
                return true;
            }
        }

        return false;
    }

    //checking for any tests in an array of cards
    public static bool AnyTests(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].type == "Test Card")
            {
                return true;
            }
        }

        return false;
    }

    //Any foes in array of cards
    public static bool AnyFoes(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].type == "Foe Card")
            {
                return true;
            }
        }

        return false;
    }

    //Any weapons in array of cards
    public static bool AnyWeapons(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].type == "Weapon Card")
            {
                return true;
            }
        }

        return false;
    }

    //Checks if an amour card has been played (is in playedCards)
    public static bool AnyAmours(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].type == "Amour Card")
            {
                return true;
            }
        }

        return false;
    }

    //MAYBE A UI UTIL THING
    //discard Cards  
    public static void DiscardCards(Deck myDeck, List<Card> discardCards, CardUIScript discardPile)
    {
        //logic discard as well as UI one
        myDeck.discardCards(discardCards);
        if (myDeck.discard.Count > 0)
        {
            discardPile.myCard = myDeck.discard[myDeck.discard.Count - 1];
        }
        else
        {
            discardPile.myCard = new TestCard("" , "" , "" , 0);
        }
        discardPile.ChangeTexture();
    }

    //Draw Cards from a certain deck
    public static List<Card> DrawFromDeck(Deck deckToDrawFrom, int numCards)
    {
        List<Card> drawnCards = new List<Card>();

        for (int i = 0; i < numCards; i++)
        {
            drawnCards.Add(deckToDrawFrom.drawCard());
        }

        return drawnCards;
    }

    //Setup Quest Info
    public static void AddQuestData(int stageNumber, int totalBP , Text questStageNumber , Text questStageBPTotal)
    {
        questStageNumber.text = "Stage: " + stageNumber.ToString();
        questStageBPTotal.text = "BP: " + totalBP.ToString();
    }

    //checking number of participants
    public static int CheckParticipation(List<Player> players)
    {
        int numberOfParticipants = 0;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].participating)
            {
                numberOfParticipants++;
            }
        }
        return numberOfParticipants;
    }

    //checking for if there is a sponsor
    public static int CheckSponsorship(List<Player> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].sponsoring)
            {
                return i;
            }
        }
        return -1;
    }

    //toggle the story deck ui animations
    public static void ToggleDeckAnimation(GameObject storyDeckUIButton , bool drawStoryCard)
    {
        storyDeckUIButton.GetComponent<Animator>().enabled = drawStoryCard;
    }

    //reset players participation and sponsorship
    public static void ResetPlayers(List<Player> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].participating = false;
            players[i].sponsoring = false;
        }
    }

    public static bool SponsorCapabilityCheck(Controller game)
    {
        if (!SponsorCapabilitySoftCheck(game))
            return false;
        else if (SponsorCapabilityHardCheck(game))
            return true;
        else
            return false;
    }

    public static bool SponsorCapabilitySoftCheck(Controller game)
    {
        int validStageCardsCount = 0;
        bool testInhand = false;

        for (int i = 0; i < game.players[game.currentPlayerIndex].hand.Count; i++)
        {
            if (game.players[game.currentPlayerIndex].hand[i] != null)
            {
                if (game.players[game.currentPlayerIndex].hand[i].type == "Foe Card")
                    validStageCardsCount++;
                else if (!testInhand && game.players[game.currentPlayerIndex].hand[i].type == "Test Card")
                {
                    testInhand = true;
                    validStageCardsCount++;
                }
            }
        }

        if (validStageCardsCount >= game.currentQuest.stages)
            return true;
        else
            return false;
    }

    public static bool SponsorCapabilityHardCheck(Controller game)
    {
        //Current number of possible valid stages that can be created.
        int validStageCardsCount = 0;
        //Number of weapons in the player's hand.
        int weaponCount = 0;
        bool testInhand = false;
        strategyUtil util = new strategyUtil();
        //Parallel array of booleans for the player's hand representing available weapon slots in the hand.
        bool[] weaponAvailable = new bool[game.players[game.currentPlayerIndex].hand.Count];

        //Integer array which stores the number of instances of a given BP found in the player's hand.
        //All battle points are a multiple of 5, which means they're translatable from a 5 - 70 scale to a 0 - 14 scale
        //This is then widened to account for the highest possible BP to prevent a index out of bounds error    
        //Maximum BP size of a stage 
        //(Max Dragon + Excalibur + Lance + Battle Axe + Sword + Horse + Dagger)
        //(70 + 30 + 20 + 15 + 10 + 10 + 5) / 5 = 32
        int[] foeValues = new int[(70 + 30 + 20 + 15 + 10 + 10 + 5) / 5];

        //Setting values to 0
        for (int i = 0; i < foeValues.Length; i++)
        {
            foeValues[i] = 0;
        }

        for (int i = 0; i < game.players[game.currentPlayerIndex].hand.Count; i++)
        {
            if (game.players[game.currentPlayerIndex].hand[i] != null)
            {
                if (game.players[game.currentPlayerIndex].hand[i].type == "Foe Card")
                {
                    foeValues[(util.getContextBP((FoeCard)game.players[game.currentPlayerIndex].hand[i], game.currentQuest.name) / 5) - 1]++;
                    //Non-weapon found, thus weapon unavailable at i
                    weaponAvailable[i] = false;
                }

                //If a test is found in the hand, validStageCardsCount is incremented and will never be less than 1, 
                //reducing the number of unique BPs required by 1.
                else if (!testInhand && game.players[game.currentPlayerIndex].hand[i].type == "Test Card")
                {
                    testInhand = true;
                    validStageCardsCount++;
                    //Non-weapon found, thus weapon unavailable at i
                    weaponAvailable[i] = false;
                }
                else if (game.players[game.currentPlayerIndex].hand[i].type == "Weapon Card")
                {
                    weaponCount++;
                    //Weapon found, thus weapon available at i
                    weaponAvailable[i] = true;
                }
            }
        }

        //Checks to see if there are 3 foes with unique BPs
        for (int i = 0; i < foeValues.Length; i++)
        {
            if (foeValues[i] != 0)
            {
                validStageCardsCount++;
            }
        }
        if (validStageCardsCount >= game.currentQuest.stages)
            return true;

        //If the number foes with unique BPs plus
        else if (validStageCardsCount + weaponCount < game.currentQuest.stages)
            return false;

        //Begins the check of whether or not adding weapon cards can create a valid hand
        else
        {
            //If there is a test in the player's hand, reduce the total
            //number of uniqueBP values required by one.
            if (testInhand)
                validStageCardsCount = 1;
            else
                validStageCardsCount = 0;

            //Goes through the array of foe values, if a given index has a value greater than 1
            //weapon cards are introduced to attempt finding a unique BP.
            for (int i = 0; i < foeValues.Length; i++)
            {
                if (foeValues[i] > 1)
                {
                    for (int j = 0; j < game.players[game.currentPlayerIndex].hand.Count; j++)
                    {
                        if (game.players[game.currentPlayerIndex].hand[j].type == "Weapon Card" && weaponAvailable[j])
                        {
                            //Stores the BP value of the weaponcard found.
                            int wbp = ((WeaponCard)game.players[game.currentPlayerIndex].hand[j]).battlePoints;

                            //Second check if foeValues[i] is a unique BP as this for loop will be 
                            //run for the length of the hand, this prevents reducing foeValues[i] below 1.
                            if (foeValues[i] > 1)
                            {
                                //A new valid unique BP has been found using one weapon card, the value at foeValues[i]
                                //is decremented and the value at a new index equal to the the current index plus the weapon bp
                                //is incremented.
                                if (foeValues[i + (wbp / 5)] < 1)
                                {
                                    foeValues[i]--;
                                    foeValues[i + (wbp / 5)]++;
                                    weaponAvailable[j] = false;
                                    //Checks to see if this addition of a new unique BP value creates a valid quest configuration
                                    if (CVQP(validStageCardsCount, foeValues , game))
                                        return true;
                                }
                                else
                                {
                                    //Itterates for each of the duplicate BPs at foeValues[i]
                                    for (int dupe_it = 0; dupe_it < foeValues[i]; dupe_it++)
                                    {
                                        //List of weapons already added to the foe.
                                        List<WeaponCard> addedWeapons = new List<WeaponCard>();
                                        addedWeapons.Add((WeaponCard)game.players[game.currentPlayerIndex].hand[j]);
                                        List<int> usedWeapons = new List<int>();

                                        //A "Hopping Index" which carries the current foeBP + sum of weapon BPs added
                                        int ii = i + (wbp / 5);
                                        //This is the section which tests if adding multiple weapons to a single foe allows the creation of a valid Quest
                                        for (int k = j + 1; k < game.players[game.currentPlayerIndex].hand.Count; k++)
                                        {
                                            if (game.players[game.currentPlayerIndex].hand[k].type == "Weapon Card" && weaponAvailable[k])
                                            {
                                                //holds the weapon card found in the player's hand, done in order to reduce line length
                                                WeaponCard foundWeaponCard = (WeaponCard)game.players[game.currentPlayerIndex].hand[k];

                                                //Checks to see if the weapon card found is a duplicate of 
                                                //a weapon card already applied to the foe.
                                                bool duplicateWeaponCheck = false;
                                                for (int aW_it = 0; aW_it < addedWeapons.Count; aW_it++)
                                                    if (addedWeapons[aW_it].name == foundWeaponCard.name)
                                                        duplicateWeaponCheck = true;
                                                if (duplicateWeaponCheck)
                                                    continue;

                                                //alternate variable name performing the same duties as wbp
                                                int wbp_VERSION_K = foundWeaponCard.battlePoints;
                                                usedWeapons.Add(k);

                                                //A new valid unique BP has been found using multiple weapon cards, the value at foeValues[i]
                                                //is decremented and the value at a new index equal to the the current index plus the sum bp
                                                //of all weapons used is incremented.
                                                if (foeValues[ii + (wbp_VERSION_K / 5)] < 1)
                                                {
                                                    foeValues[i]--;
                                                    foeValues[ii + (wbp_VERSION_K / 5)]++;

                                                    for (int clean_up_it = 0; clean_up_it < usedWeapons.Count; clean_up_it++)
                                                    {
                                                        weaponAvailable[clean_up_it] = false;
                                                    }

                                                    weaponAvailable[j] = false;
                                                    //Checks to see if this addition of a new unique BP value creates a valid quest configuration
                                                    if (CVQP(validStageCardsCount, foeValues , game))
                                                        return true;
                                                    //new unique BP found, break loop and attempt to find another valid BP for the next dupicate BP at i
                                                    break;
                                                }

                                                //A weapon is added to the current list of weapons being applied to the foe
                                                else
                                                {
                                                    addedWeapons.Add(foundWeaponCard);
                                                    ii += wbp_VERSION_K;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Final check if there are enough unique BP values to fill the quest before returning false
            if (CVQP(validStageCardsCount, foeValues , game))
                return true;
            else
                return false;
        }
    }

    //Checks to see if the number of unique BP's available to the
    //player is equal to or greater than the number of stages in the quest.
    public static bool CVQP(int vSCC, int[] foeValues , Controller game)
    {

        for (int i = 0; i < foeValues.Length; i++)
        {
            if (foeValues[i] != 0)
            {
                vSCC++;
            }
        }

        if (vSCC >= game.currentQuest.stages)
            return true;
        else
            return false;
    }
}
