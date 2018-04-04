using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUtil
{
    //Check players for 1 or more winners and return them in the winners array
    public static void FindWinners(List<Player> players, List<int> winners, UIInput userInput)
    {
        //check for winners
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].rankUpCheck(players[i].score, 0))
            {
                //store the index of the winners of the game
                winners.Add(i);
            }
        }

        //create a userPrompt for the winners
        if (winners.Count > 0)
        {
            string winner = "";

            //code for showing winners here
            for (int i = 0; i < winners.Count; i++)
            {
                winner += players[winners[i]].name;
            }

            winner += " Won the Game!!!";

            userInput.ActivateBooleanCheck(winner);
        }
    }

    //return certain cards to a player's hand
    public static void ReturnToPlayerHand(Player player, List<Card> cards , Controller game)
    {
        //add userInput selected hand back to player
        for (int i = 0; i < cards.Count; i++)
        {
            player.hand.Add(cards[i]);
            cards.RemoveAt(i);
            i--;
        }

        PopulatePlayerBoard(game);
    }

    //Add a card to a UI panel
    public static void AddCardToPanel(GameObject UICard, GameObject panel)
    {
        CardUIScript script = UICard.GetComponent<CardUIScript>();
        if (panel.name == "CurrentHand")
        {
            script.isHandCard = true;
        }
        else
        {
            script.isHandCard = false;
        }

        //set as a child of the ui card
        UICard.transform.SetParent(panel.transform);
    }

    //if you want to empty any panel
    public static void EmptyPanel(GameObject myPanel)
    {
        for (int i = 0; i < myPanel.transform.childCount; i++)
        {
            Object.Destroy(myPanel.transform.GetChild(i).gameObject);
        }
    }

    //Calculate player's bp points for a quest or tournament
    public static void PopulatePlayerBPS(Controller game)
    {
        //sets up player BPS for quests tourneys etc .... and returns them back when done check current event and quest and tourney
        for (int i = 0; i < game.players.Count; i++)
        {
            //check for special ally abilities
            if (game.currentQuest != null)
            {
                game.UIBPS[i].text = "BP: " + game.players[i].CalculateBP(game.currentQuest.name, game.players);
            }
            else
            {
                game.UIBPS[i].text = "BP: " + game.players[i].CalculateBP("", game.players);

            }

            //participating in a quest tourneys don't matter really
            if (game.players[i].participating)
            {
                int totalPlayedBP = 0;

                //go through amours and foe / weapon cards

                //go through amours of quest will be empty if in tournament or anything else
                if (QuestState.amours != null)
                {
                    if (QuestState.amours[i] != null)
                    {
                        //add amour values
                        for (int j = 0; j < QuestState.amours[i].Count; j++)
                        {
                            AmourCard amour = (AmourCard)QuestState.amours[i][j];
                            totalPlayedBP += amour.battlePoints;
                        }
                    }
                }

                //go through weapons of quest
                if (game.queriedCards[i] != null && game.currentQuest != null)
                {
                    for (int k = 0; k < game.queriedCards[i].Count; k++)
                    {
                        if (game.queriedCards[i][k].type == "Weapon Card")
                        {
                            //these should be weapons
                            totalPlayedBP += ((WeaponCard)game.queriedCards[i][k]).battlePoints;
                        }
                    }
                }

                //go through weapons and amours of tournament add them up
                if (game.currentTournament != null)
                {
                    if (TournamentState.undiscardedCards != null && TournamentState.undiscardedCards[i] != null)
                    {
                        for (int k = 0; k < TournamentState.undiscardedCards[i].Count; k++)
                        {
                            //there probably is a function that does this ????
                            if (TournamentState.undiscardedCards[i][k].type == "Weapon Card")
                            {
                                totalPlayedBP += ((WeaponCard)TournamentState.undiscardedCards[i][k]).battlePoints;
                            }
                            else if (TournamentState.undiscardedCards[i][k].type == "Amour Card")
                            {
                                totalPlayedBP += ((AmourCard)TournamentState.undiscardedCards[i][k]).battlePoints;
                            }
                        }
                    }
                }

                //add up result
               game.UIBPS[i].text += " (" + totalPlayedBP.ToString() + ")";
            }
        }
    }

    //add cards to quest
    public static void PopulateQuestBoard(Controller game , bool faceUp)
    {
        EmptyPanel(game.questStagePanel);
        game.questStageBPTotal.text = "";
        game.questStageNumber.text = "Stage: " + (QuestState.currentStage + 1).ToString();

        //check if it is flipped b4 adding the card and set its current backing
        //maybe show bp total maybe not
        if (QuestState.stages[QuestState.currentStage] != null)
        {
            for (int i = 0; i < QuestState.stages[QuestState.currentStage].Count; i++)
            {
                //flip the cards
                GameObject card = CreateUIElement(QuestState.stages[QuestState.currentStage][i] , game.cardPrefab);
                if (!faceUp)
                {
                    card.GetComponent<CardUIScript>().flipCard();
                }
                else
                {
                    card.GetComponent<CardUIScript>().ChangeTexture();
                }
                AddCardToPanel(card, game.questStagePanel);
            }
        }
    }

    public static void CalculateUIPlayerInfo(Controller game)
    {
        //might need a function in player that calculates the BP at any time
        for (int i = 0; i < game.players.Count; i++)
        {
            game.UINames[i].text = "Name: " + game.players[i].name;
            game.UINumCards[i].text = "Cards: " + game.players[i].hand.Count.ToString();
            game.UIRanks[i].text = "Rank: " + game.players[i].rankCard.name;
            game.UIShields[i].text = "Shields: " + game.players[i].score.ToString();
        }

        //adds player BPS for a quest or generally
        PopulatePlayerBPS(game);

        game.playerPanels[game.currentPlayerIndex].GetComponent<Outline>().enabled = true;
        game.playerAllyPanels[game.currentPlayerIndex].GetComponent<Outline>().enabled = true;
    }

    //adds ui elements to game board of current player
    public static void PopulatePlayerBoard(Controller game)
    {
        //using current player once we setup player turn change will update this
        EmptyPanel(game.handPanel);

        //empty all ally panels
        for (int i = 0; i < game.playerAllyPanels.Count; i++)
        {
            EmptyPanel(game.playerAllyPanels[i]);
        }

        Player myPlayer = game.players[game.currentPlayerIndex];

        //rank card and shield numbers
        SetRankCardUI(game);

        //shield cards
        game.shieldsCard.myCard = new RankCard(null, null, myPlayer.shieldPath, 0);
        game.shieldsCard.ChangeTexture();

        //Setup Hand
        for (int i = 0; i < myPlayer.hand.Count; i++)
        {
            AddCardToPanel(CreateUIElement(myPlayer.hand[i] , game.cardPrefab), game.handPanel);
        }

        //Setup all Ally cards
        for (int j = 0; j < game.players.Count; j++)
        {
            for (int i = 0; i < game.players[j].activeAllies.Count; i++)
            {
                Debug.Log("adding active allies");
                AddCardToPanel(CreateUIElement(game.players[j].activeAllies[i], game.cardPrefab), game.playerAllyPanels[j]);
            }
        }
    }

    //initializes player information and hands
    public static List<Player> CreatePlayers(Controller game, List<string> humanNames, List<string> cpuNames)
    {
        List<Player> myPlayers;
        myPlayers = new List<Player>();

        //create human players (set up their references and give them a hand)
        for (int i = 0; i < game.numHumanPlayers; i++)
        {
            int shield = Random.Range(0, game.shieldPaths.Count - 1);
            Player myPlayer = new Player("Player" + (i + 1).ToString(), GameUtil.DrawFromDeck(game.adventureDeck, 12), new iStrategyPlayer(), game.shieldPaths[shield]);
            game.shieldPaths.RemoveAt(shield);       //Each player has unique shields
            myPlayer.gameController = game;
            game.playerPanels[i].SetActive(true);    //add a ui panel for each player
            game.playerAllyPanels[i].SetActive(true);
            myPlayers.Add(myPlayer);
        }

        // create AI players

        for (int i = 0; i < game.numCpus; i++)
        {
            //int rnd = Random.Range(0 , 2);
            int shield = Random.Range(0, game.shieldPaths.Count - 1);
            Player newPlayer;

            Debug.Log(game.cpuStrategies[i]);

            //log resulting strategy
            if (game.cpuStrategies[i] == 1) { newPlayer = new Player("CPU" + (i + 1).ToString(), GameUtil.DrawFromDeck(game.adventureDeck, 12), new iStrategyCPU1(), game.shieldPaths[shield]); }
            else { newPlayer = new Player("CPU" + (i + 1).ToString(), GameUtil.DrawFromDeck(game.adventureDeck, 12), new iStrategyCPU2(), game.shieldPaths[shield]); }

            game.playerPanels[i + game.numHumanPlayers].SetActive(true);    //add a ui panel for each player
            game.playerAllyPanels[i].SetActive(true);
            game.shieldPaths.RemoveAt(shield); //Each player has unique shields
            myPlayers.Add(newPlayer);
        }

        //create a queried cards array
        game.queriedCards = new List<Card>[game.numPlayers];
        game.sponsorQueriedCards = new List<Card>[5];
        return myPlayers;
    }

    public static List<Player> CreateNetworkPlayers(Controller game, GameObject[] humanPlayers)
    {
        int humans = humanPlayers.Length;
        List<Player> myPlayers;
        myPlayers = new List<Player>();

        // create human players (set up their references and give them a hand)
        for(int i = 0; i < humans; i++)
        {
            int shield = Random.Range(0, game.shieldPaths.Count - 1);
            //string name = humanPlayers[i].GetComponent<Prototype.NetworkLobby.LobbyPlayer>().playerName;
            UnityEngine.Networking.NetworkIdentity connect = humanPlayers[i].GetComponent<UnityEngine.Networking.NetworkIdentity>();
            
            //change this!!!!!
            Player myPlayer = new Player("", GameUtil.DrawFromDeck(game.adventureDeck , 12), new iStrategyPlayer(), game.shieldPaths[shield], 5, 12, 22, connect);
            game.shieldPaths.RemoveAt(shield);       //Each player has unique shields
            myPlayer.gameController = game;
            game.playerPanels[i].SetActive(true);    //add a ui panel for each player
            game.playerAllyPanels[i].SetActive(true);
            myPlayers.Add(myPlayer);
        }

        // create the CPU players depending on the number of human players
        int cpus = 4 - humans;
        for (int i = 0; i < cpus; i++)
        {
            int shield = Random.Range(0, game.shieldPaths.Count - 1);
            Player newPlayer;
            newPlayer = new Player("CPU" + (i + 1).ToString(), GameUtil.DrawFromDeck(game.adventureDeck, 12), new iStrategyCPU2(), game.shieldPaths[shield]);
            game.playerPanels[i + game.numHumanPlayers].SetActive(true);    //add a ui panel for each player
            game.playerAllyPanels[i].SetActive(true);
            game.shieldPaths.RemoveAt(shield); //Each player has unique shields
            myPlayers.Add(newPlayer);

        }

        //create a queried cards array
        game.queriedCards = new List<Card>[game.numPlayers];
        game.sponsorQueriedCards = new List<Card>[5];
        return myPlayers;
    }

    //Create a UI element for cards
    public static GameObject CreateUIElement(Card cardLogic , GameObject cardPrefab)
    {
        //create game object of card prefab
        GameObject UICard = Object.Instantiate(cardPrefab, new Vector3(0, 0, 0), new Quaternion());

        //add card logic to the ui card
        CardUIScript script = UICard.GetComponent<CardUIScript>();
        script.myCard = cardLogic;
        script.ChangeTexture();

        return UICard;
    }

    //to be called by players to setup rank and shields
    public static void SetRankCardUI(Controller game)
    {
        game.rankCard.myCard = game.players[game.currentPlayerIndex].rankCard;
        game.rankCard.ChangeTexture();
        game.UIShieldNum.text = ": " + game.players[game.currentPlayerIndex].score;
    }

    //checks for cycles will occur elsewhere or maybe later
    //updates turn to go to next turn
    public static void UpdatePlayerTurn(Controller game)
    {
        game.playerPanels[game.currentPlayerIndex].GetComponent<Outline>().enabled = false;
        game.playerAllyPanels[game.currentPlayerIndex].GetComponent<Outline>().enabled = false;


        //modify the value of the current player index to use the player array
        if (game.currentPlayerIndex >= game.numPlayers - 1) { game.currentPlayerIndex = 0; }
        else { game.currentPlayerIndex++; }

        //resetup ui messages
        CalculateUIPlayerInfo(game);
        PopulatePlayerBoard(game);
    }

}
