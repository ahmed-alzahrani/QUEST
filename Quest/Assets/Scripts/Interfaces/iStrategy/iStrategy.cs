using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iStrategy
{
    //tournament strategy
    int participateInTourney(List<Player> players, int shields, GameController gameController);
    int participateInQuest(int stages, List<Card> hand, GameController game);
    List<Card> playTournament(List<Player> players, List<Card> hand, int baseBP, int shields);

    // Quest Strategy
    int sponsorQuest(List<Player> players, int stages, List<Card> hand, GameController game);
    List<List<Card>> setupQuest(int stages, List<Card> hand, string questFoe);
    List<Card> setupFoeStage(int currentStage, int stages, List<Card> hand, string questFoe, int prev);
    List<Card> setupTestStage(List<Card> hand);
    List<Card> playFoeEncounter(int stage, int stages, List<Card> hand, int previous, bool amour, string questName, List<Player> players);


    // Test strategy
    int willIBid(int currentBid, List<Card> hand, int round, GameController game);
    List<Card> playBid(List<Card> hand, int round);

    // additonal stratgy to be added includes:

    // Strategy functions for the king's call event
    List<Card> discardWeapon(List<Card> hand);
    List<Card> discardFoesForKing(List<Card> hand);

    // evaluating what cards to discard if the hand becomes too full


    //Called when there is a binary resposne required from the player
    int respondToPrompt(GameController game);

    List<Card> fixHandDiscrepancy(List<Card> hand);
}
