using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public abstract class Controller : MonoBehaviour
{
    public deckBuilder decks;
    public Deck storyDeck;
    public Deck adventureDeck;
    public bool kingsRecognition;

    //turn setup
    public int turnCount;
    public Turn nextTurn;

    //card ui prefab
    public GameObject cardPrefab;

    //checking user input
    public UIInput userInput;

    public List<Player> players;

    //UI game board information
    public CardUIScript shieldsCard;
    public CardUIScript rankCard;
    public Text UIShieldNum;
    public GameObject handPanel;
    public GameObject questPanel;
    public GameObject questStagePanel;
    public GameObject weaponPanel;
    public GameObject amourPanel;
    public GameObject activatedPanel;
    public GameObject cheatPanel;

    // player panels UI names bp number of cards ranks etc...
    public List<GameObject> playerPanels;
    public List<GameObject> playerAllyPanels;
    public List<Text> UINames;
    public List<Text> UIBPS;
    public List<Text> UINumCards;
    public List<Text> UIRanks;
    public List<Text> UIShields;

    //Quest data
    public Text questStageNumber;
    public Text questStageBPTotal;

    // deck UI information
    public GameObject adventureDeckUIButton;
    public CardUIScript adventureDeckDiscardPileUIButton;  //to change image here for discard pile
    public GameObject storyDeckUIButton;
    public CardUIScript storyDeckDiscardPileUIButton;      //to change image here for discard pile
    public Button rankDeckUIButton;

    public Card selectedCard;
    public List<Card> drawnAdventureCards;
    public Card drawnStoryCard;
    public int numCardsToBeDrawn;

    public int numPlayers;
    public int numHumanPlayers;
    public int numCpus;
    public int currentPlayerIndex;

    //for setting up gameboard and player information
    public bool isSettingUpGame;
    public int setupState;

    // Some names for players cpus etc....
    public List<string> playerNames;
    public List<string> cpuNames;
    public List<string> shieldPaths;

    //states for drawing an adventure or story card
    public bool drawAdventureCard;
    public bool drawStoryCard;

    //current story cards running
    public EventCard currentEvent;
    public QuestCard currentQuest;
    public TournamentCard currentTournament;
    public bool isDoneStoryEvent;

    public int numIterations;

    public List<Card>[] queriedCards;
    public List<Card>[] sponsorQueriedCards;

    public List<int> cpuStrategies;
    public List<int> winners;       //store the indeces of the winners

    public bool foundWinner;
    public bool playerStillOffending;      // players with over 12 cards exist

    public string[] cards;
    public string[] cardsStory;
    public int[] mordredControllerBeta;
    public int mordCurr;


    public void DrawFromAdventureDeck()
    {
        //have a draw from adventure deck that takes a certain number of cards to be drawn instead doing it one by one that way click only happens once
        if (drawAdventureCard)
        {
            drawnAdventureCards = GameUtil.DrawFromDeck(adventureDeck, numCardsToBeDrawn);
            drawAdventureCard = false;
        }

        GameUtil.ToggleDeckAnimation(storyDeckUIButton, storyDeckDiscardPileUIButton);
    }

    public void DrawFromStoryDeck()
    {
        //only one story card is ever drawn
        if (drawStoryCard)
        {
            drawnStoryCard = GameUtil.DrawFromDeck(storyDeck, 1)[0];
            drawStoryCard = false;
        }

        GameUtil.ToggleDeckAnimation(storyDeckUIButton, storyDeckDiscardPileUIButton);
    }

    public void MordredCheck()
    {
        //reset input if u change ur mind
        if (Input.GetKeyDown(KeyCode.Q))
        {
            mordCurr = 0;
            Debug.Log("Reset Mordred Input");
            return;
        }

        //shift and then numbers
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (mordCurr < 3)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                {
                    mordredControllerBeta[mordCurr] = 0;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                {
                    mordredControllerBeta[mordCurr] = 1;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                {
                    mordredControllerBeta[mordCurr] = 2;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                {
                    mordredControllerBeta[mordCurr] = 3;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
                {
                    mordredControllerBeta[mordCurr] = 4;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
                {
                    mordredControllerBeta[mordCurr] = 5;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
                {
                    mordredControllerBeta[mordCurr] = 6;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
                {
                    mordredControllerBeta[mordCurr] = 7;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
                {
                    mordredControllerBeta[mordCurr] = 8;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
                {
                    mordredControllerBeta[mordCurr] = 9;
                    mordCurr++;
                }

            }


            if (mordCurr >= mordredControllerBeta.Length)
            {
                Debug.Log("Player " + (mordredControllerBeta[0] + 1).ToString() + " Casting Mordred on Player " +
                    (mordredControllerBeta[1] + 1).ToString() + "'s Ally number " + (mordredControllerBeta[2] + 1).ToString());
                if (mordredControllerBeta[0] < numPlayers && mordredControllerBeta[1] < numPlayers)
                {
                    if (players[mordredControllerBeta[1]].activeAllies.Count > mordredControllerBeta[2])
                    {
                        for (int i = 0; i < players[mordredControllerBeta[0]].hand.Count; i++)
                        {
                            if (players[mordredControllerBeta[0]].hand[i].name == "Mordred")
                            {
                                List<Card> tempCards = new List<Card>();

                                tempCards.Add(players[mordredControllerBeta[0]].hand[i]);
                                tempCards.Add(players[mordredControllerBeta[1]].activeAllies[mordredControllerBeta[2]]);

                                players[mordredControllerBeta[0]].hand.RemoveAt(i);
                                players[mordredControllerBeta[1]].activeAllies.RemoveAt(mordredControllerBeta[2]);

                                GameUtil.DiscardCards(adventureDeck, tempCards, adventureDeckDiscardPileUIButton);
                            }
                        }
                        UIUtil.PopulatePlayerBoard(this);
                    }
                }
                mordCurr = 0;
            }
        }
    }

    public void SetupStoryCardCheck()
    {
        if (drawnStoryCard != null)
        {
            Debug.Log("DRAWN STORY CARD: " + drawnStoryCard.name);

            UIUtil.EmptyPanel(questStagePanel);
            questStageBPTotal.text = "";
            questStageNumber.text = "";

            //setup quest , tournament or event
            if (drawnStoryCard.type == "Event Card")
            {
                currentEvent = (EventCard)drawnStoryCard;
            }
            else if (drawnStoryCard.type == "Tournament Card")
            {
                userInput.ActivateBooleanCheck("Do you want to participate?");
                currentTournament = (TournamentCard)drawnStoryCard;
            }
            else if (drawnStoryCard.type == "Quest Card")
            {
                //store player that first drew the quest card
                QuestState.questDrawer = currentPlayerIndex;
                userInput.ActivateBooleanCheck("Do you want to sponsor this quest?");
                currentQuest = (QuestCard)drawnStoryCard;
            }

            numIterations = 0;
            UIUtil.AddCardToPanel(UIUtil.CreateUIElement(drawnStoryCard, cardPrefab), questPanel);
            drawnStoryCard = null;
        }
    }

    public bool StoryCardDone()
    {
        if (isDoneStoryEvent)
        {
            //discard story card into story discard pile
            if (currentEvent != null)
            {
                List<Card> cards = new List<Card>();
                cards.Add(currentEvent);
                GameUtil.DiscardCards(storyDeck, cards, storyDeckDiscardPileUIButton);
                // storyDeckDiscardPileUIButton.myCard = currentEvent;
                //storyDeckDiscardPileUIButton.ChangeTexture();
                //storyDeck.discard.Add(currentEvent);
            }
            else if (currentQuest != null)
            {
                List<Card> cards = new List<Card>();
                cards.Add(currentQuest);
                GameUtil.DiscardCards(storyDeck, cards, storyDeckDiscardPileUIButton);
                //storyDeckDiscardPileUIButton.myCard = currentQuest;
                //storyDeckDiscardPileUIButton.ChangeTexture();
                //storyDeck.discard.Add(currentQuest);
            }
            else if (currentTournament != null)
            {
                List<Card> cards = new List<Card>();
                cards.Add(currentTournament);
                GameUtil.DiscardCards(storyDeck, cards, storyDeckDiscardPileUIButton);
                //storyDeckDiscardPileUIButton.myCard = currentTournament;
                //storyDeckDiscardPileUIButton.ChangeTexture();
                //storyDeck.discard.Add(currentTournament);
            }

            isDoneStoryEvent = false;
            currentEvent = null;
            currentQuest = null;
            currentTournament = null;

            //reset players participation
            GameUtil.ResetPlayers(players);
            UIUtil.EmptyPanel(questPanel);
            System.Array.Clear(queriedCards, 0, queriedCards.Length);

            //reset turn to draw new story card
            drawStoryCard = true;
            GameUtil.ToggleDeckAnimation(storyDeckUIButton, drawStoryCard);
            userInput.DeactivateUI(); // just in case
            UIUtil.UpdatePlayerTurn(this);       // go to next turn

            return true;
        }

        return false;
    }

    public void RunStoryCard()
    {
        if (currentEvent != null)
        {
            currentEvent.effect.execute(players, currentEvent, this);
        }
        else if (currentTournament != null)
        {
            currentTournament.tournament.execute(null, currentTournament, this);
        }
        else if (currentQuest != null)
        {
            currentQuest.quest.execute(players, currentQuest, this);

            //CHECK HERE IF CARDS TO BE ADDED BY SPONSOR BREAK THE RULES RETURN ALL THESE CARDS TO HIS HAND AND FORCE HIM TO REDECIDE
            if (QuestState.state == "Sponsoring")
            {
                if (!GameUtil.AnyFoes(userInput.cardPrompt.selectedCards) && GameUtil.AnyWeapons(userInput.cardPrompt.selectedCards))
                {
                    //there are foes but no weapons return all cards to hand and start again
                    UIUtil.ReturnToPlayerHand(players[GameUtil.CheckSponsorship(players)], userInput.cardPrompt.selectedCards, this);
                }
            }
        }
    }

    public void CardAdditionCheck()
    {
        if (selectedCard != null)
        {
            //all ally cards activated are added to the board and active allies
            if (selectedCard.type == "Ally Card")
            {
                //ally cards always get added to active allies
                AllyCard ally = (AllyCard)selectedCard;
                players[currentPlayerIndex].activeAllies.Add(ally);
                players[currentPlayerIndex].hand.Remove(selectedCard);
                UIUtil.AddCardToPanel(UIUtil.CreateUIElement(selectedCard, cardPrefab), playerAllyPanels[currentPlayerIndex]);
            }
            else
            {
                //check cards for removal from card panel
                bool removedCard = userInput.CheckCard(selectedCard);

                if (removedCard)
                {
                    //removed card from panel So add it back to hand
                    players[currentPlayerIndex].hand.Add(selectedCard);
                    UIUtil.AddCardToPanel(UIUtil.CreateUIElement(selectedCard, cardPrefab), handPanel);
                }
                else
                {
                    bool addToPanel = true;

                    //CLUSTERFUCK
                    addToPanel = EventCheck(QuestCheck(TournamentCheck()));

                    //result
                    if (addToPanel)
                    {
                        //add it into the panel
                        players[currentPlayerIndex].hand.Remove(selectedCard);
                        userInput.AddToUICardPanel(UIUtil.CreateUIElement(selectedCard, cardPrefab));
                    }
                    else
                    {
                        //return it to hand since we didn't delete it from player we are fine
                        UIUtil.AddCardToPanel(UIUtil.CreateUIElement(selectedCard, cardPrefab), handPanel);
                    }
                }
            }

            //maybe we can check for certain cards like mordred here with some state
            selectedCard = null;
        }
    }

    public void DiscardCardAddition()
    {
        if (selectedCard != null)
        {
            //all ally cards activated are added to the board and active allies
            if (selectedCard.type == "Ally Card")
            {
                //ally cards always get added to active allies
                AllyCard ally = (AllyCard)selectedCard;
                players[currentPlayerIndex].activeAllies.Add(ally);
                players[currentPlayerIndex].hand.Remove(selectedCard);
                UIUtil.AddCardToPanel(UIUtil.CreateUIElement(selectedCard, cardPrefab), playerAllyPanels[currentPlayerIndex]);
            }
            else
            {
                bool removed = userInput.CheckDiscardCard(selectedCard);
                if (removed)
                {
                    //add card back to player 
                    players[currentPlayerIndex].hand.Add(selectedCard);
                    UIUtil.PopulatePlayerBoard(this);
                }
                else
                {
                    //add it into the panel
                    players[currentPlayerIndex].hand.Remove(selectedCard);

                    //Debug.Log(selectedCard.name);
                    userInput.AddToUIDiscardPanel(UIUtil.CreateUIElement(selectedCard, cardPrefab));
                }
            }

            selectedCard = null;
        }
    }

    public void CheckDiscardPhase()
    {
        if (numIterations >= numPlayers)
        {
            //done discarding
            numIterations = 0;
            userInput.DeactivateDiscardPanel();
            playerStillOffending = false;
        }
    }

    public bool TournamentCheck()
    {
        bool addToPanel = true;

        if (currentTournament != null)
        {
            // we are in a tournament check for weapon, amour cards allies are always added
            if (selectedCard.type == "Weapon Card")
            {
                for (int i = 0; i < userInput.cardPrompt.selectedCards.Count; i++)
                {
                    if (userInput.cardPrompt.selectedCards[i].name == selectedCard.name)
                    {
                        //duplicate exists cannot add it in
                        addToPanel = false;
                    }
                }
            }
            else if (selectedCard.type == "Amour Card")
            {
                //check for any amours already played 
                if (!GameUtil.AnyAmours(TournamentState.undiscardedCards[currentPlayerIndex]))
                {
                    addToPanel = true;
                }
                else
                {
                    addToPanel = false;
                }

                //check amours at the moment
                for (int i = 0; i < userInput.cardPrompt.selectedCards.Count; i++)
                {
                    if (userInput.cardPrompt.selectedCards[i].type == "Amour Card")
                    {
                        //more than one amour cannot add it
                        addToPanel = false;
                    }
                }
            }
            else
            {
                // not weapon amour or ally
                return false;
            }
        }

        return addToPanel;
    }

    public bool QuestCheck(bool result)
    {
        bool addToPanel = result;

        if (currentQuest != null)
        {
            //if it is the sponsor check that he added cards properly
            if (players[currentPlayerIndex].sponsoring)
            {
                if (GameUtil.AnyTests(userInput.cardPrompt.selectedCards)) { addToPanel = false; }
                else if (selectedCard.type == "Test Card")
                {
                    //check that it is the only card in the panel and do not allow him to any more cards
                    if (userInput.cardPrompt.selectedCards.Count > 0) { addToPanel = false; }
                    else { addToPanel = true; }
                }
                else if (selectedCard.type == "Foe Card")
                {
                    if (!GameUtil.AnyFoes(userInput.cardPrompt.selectedCards))
                    {
                        addToPanel = true;
                    }
                    else
                    {
                        addToPanel = false;
                    }
                }
                else if (selectedCard.type == "Weapon Card")
                {
                    // we have a weapon card which needs a special check
                    for (int i = 0; i < userInput.cardPrompt.selectedCards.Count; i++)
                    {
                        if (userInput.cardPrompt.selectedCards[i].name == selectedCard.name)
                        {
                            //duplicate exists cannot add it in
                            addToPanel = false;
                            break;
                        }
                    }

                    //if no duplication check if there is a foe in there
                    if (addToPanel)
                    {
                        addToPanel = GameUtil.AnyFoes(userInput.cardPrompt.selectedCards);
                    }
                }
                else
                {
                    addToPanel = false;
                }
            }
            else if (players[currentPlayerIndex].participating)
            {
                //only allowed weapon and amour cards
                if (selectedCard.type == "Weapon Card")
                {
                    for (int i = 0; i < userInput.cardPrompt.selectedCards.Count; i++)
                    {
                        if (userInput.cardPrompt.selectedCards[i].name == selectedCard.name)
                        {
                            //duplicate exists cannot add it in
                            addToPanel = false;
                            break;
                        }
                    }
                }
                else if (selectedCard.type == "Amour Card")
                {
                    //MAYBEEEEEE NEEEEEEDDSSSSSSS A CHANGEEEEE
                    //check for any amours already played 
                    if (!GameUtil.AnyAmours(QuestState.amours[currentPlayerIndex]))
                    {
                        addToPanel = true;
                    }
                    else
                    {
                        addToPanel = false;
                    }

                    //check for amours played at this instance 
                    for (int i = 0; i < userInput.cardPrompt.selectedCards.Count; i++)
                    {
                        if (userInput.cardPrompt.selectedCards[i].type == "Amour Card")
                        {
                            //more than one amour cannot add it
                            addToPanel = false;
                            break;
                        }
                    }
                }
                else if (selectedCard.type == "Foe Card")
                {
                    if (QuestState.stages != null)
                    {
                        if (QuestState.stages[QuestState.currentStage] != null)
                        {
                            if (QuestState.stages[QuestState.currentStage][0] != null)
                            {
                                if (QuestState.stages[QuestState.currentStage][0].type == "Test Card")
                                    addToPanel = true;
                                else
                                    addToPanel = false;
                            }
                            else
                                addToPanel = false;
                        }
                        else
                            addToPanel = false;
                    }
                    else
                        addToPanel = false;
                }
                else
                {
                    // not weapon amour or ally
                    addToPanel = false;
                }
            }
        }

        return addToPanel;
    }

    public bool EventCheck(bool result)
    {
        // if we have an event we are fine else return previous result
        if (currentEvent != null)
        {
            return true;
        }
        else
        {
            return result;
        }
    }

    public void CheatPanelToggle()
    {
        if (GameUtil.CheckSponsorship(players) >= 0 && GameUtil.CheckParticipation(players) > 0)
        {
            if (!cheatPanel.activeSelf)
            {

                cheatPanel.SetActive(true);
                int cheatPanelStageNumber = System.Int32.Parse(cheatPanel.transform.Find("StageNumber").GetComponent<Text>().text.Substring(6)) + 1;

                cheatPanel.transform.Find("StageNumber").GetComponent<Text>().text = "STAGE " + (cheatPanelStageNumber);
                cheatPanel.transform.Find("CardsInStage").GetComponent<Text>().text = "CARDS IN STAGE " + QuestState.stages[cheatPanelStageNumber].Count;

            }
            else
            {
                int cheatPanelStageNumber = System.Int32.Parse(cheatPanel.transform.Find("StageNumber").GetComponent<Text>().text.Substring(6));

                if (cheatPanelStageNumber >= QuestState.currentQuest.stages)
                {
                    cheatPanel.SetActive(false);
                    cheatPanel.transform.Find("StageNumber").GetComponent<Text>().text = "STAGE " + 0;
                    cheatPanel.transform.Find("CardsInStage").GetComponent<Text>().text = "CARDS IN STAGE " + 0;

                }
                else
                {
                    cheatPanel.transform.Find("StageNumber").GetComponent<Text>().text = "STAGE " + (cheatPanelStageNumber + 1);
                    cheatPanel.transform.Find("CardsInStage").GetComponent<Text>().text = "CARDS IN STAGE " + QuestState.stages[cheatPanelStageNumber].Count;


                }


            }
        }
    }


}
