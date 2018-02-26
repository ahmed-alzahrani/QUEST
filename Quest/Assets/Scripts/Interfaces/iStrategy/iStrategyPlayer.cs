using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStrategyPlayer : iStrategy
{
  public iStrategyPlayer() {}



    // Tournament Strategy
    public int participateInTourney(List<Player> players, int shields, GameController gameController)
    {
        //check the yes / No buttons
        if (gameController.userInput.buttonResult == "Yes")
        {
            Debug.Log("Yes");
            return 1;
        }
        else if (gameController.userInput.buttonResult == "No")
        {
            Debug.Log("No");
            return 0;
        }
        else
        {
            //still checking
            //Debug.Log("still checking");
            return 2;
        }
    }

    public List<Card> playTournament(List<Player> players, List<Card> hand, int baseBP, int shields)
    {
      return null;

    }



    // Quest Strategy
    public int sponsorQuest(List<Player> players, int stages, List<Card> hand, GameController game)
    {
        // its the same for players
         return participateInTourney(players , 0 , game);
    }

    public List<List<Card>> setupQuest(int stages, List<Card> hand, string questFoe)
    {
      //List<List<Card>> questLine = new List<List<Card>>();
      return null;
    }

    public List<Card> setupFoeStage(int currentStage, int stages, List<Card> hand, string questFoe)
    {
      return hand;
    }

    public List<Card> setupTestStage(List<Card> hand)
    {
      List<Card> test = new List<Card>();
      return test;
      // get the test card with the highest bid test card in the hand
    }

    public int participateInQuest(int stages, List<Card> hand, GameController game)
    {
      return 1;
    }

    public List<Card> playFoeEncounter(int stage, int stages, List<Card> hand, int previous, bool amour, string questName, List<Player> players)
    {
      return hand;
    }


    // Test Strategy

    public int willIBid(int currentBid, List<Card> hand, int round, GameController game)
    {
      return currentBid + 1;
    }

    public List<Card> playBid(List<Card> hand, int round)
    {
      return hand;
    }

    public List<Card> kingsCall(List<Card> hand)
    {
      return hand;
    }

    public List<Card> discardWeapon(List<Card> hand){
      return hand;
    }
    public List<Card> discardFoesForKing(List<Card> hand){
      return hand;
    }


    //Called when there is a binary resposne required from the player
    public int respondToPrompt(GameController game)
    {
        //check the yes / No buttons
        if (game.userInput.buttonResult == "Yes")
        {
            Debug.Log("Yes");
            return 1;
        }
        else if (game.userInput.buttonResult == "No")
        {
            Debug.Log("No");
            return 0;
        }
        else
        {
            //still checking
            //Debug.Log("still checking");
            return 2;
        }
    }

    public List<Card> fixHandDiscrepancy(List<Card> hand){
      return null;
    }

}
