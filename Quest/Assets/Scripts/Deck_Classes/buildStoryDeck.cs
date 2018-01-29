using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class buildStoryDeck{

  private Deck deck;
  private Card card;
  private Button button;
  private Deck storyDeck;

    public buildStoryDeck() { }

  public Deck build(){
    return new Deck("Story Deck", 28, buildDeck());
  }

  public List<Card> buildDeck(){
    List<Card> storyCards = new List<Card>();
    List<Card> questCards = generateQuestCards();
    foreach(Card c in questCards)
        {
            storyCards.Add(c);
        }

    List<Card> tournamentCards = generateTournamentCards();
    foreach(Card c in tournamentCards)
        {
            storyCards.Add(c);
        }
    List<Card> eventCards = generateEventCards();
    foreach(Card c in eventCards)
        {
            storyCards.Add(c);
        }


     return storyCards;
  }

  public List<Card> generateQuestCards()
    {
        List<Card> questCards = new List<Card>();

        questCards.Add(new QuestCard("Quest Card", "Journey through the Enchanted Forest", 3, "Evil Knight"));
        questCards.Add(new QuestCard("Quest Card", "Vanquish King Arthur's Enemies", 3, null));
        questCards.Add(new QuestCard("Quest Card", "Repel the Saxon Raiders", 2, "All Saxons"));
        questCards.Add(new QuestCard("Quest Card", "Boar Hunt", 2, "Boar"));
        questCards.Add(new QuestCard("Quest Card", "Search for the Questing Beast", 4, null));
        questCards.Add(new QuestCard("Quest Card", "Defend the Queen's Honor", 4, "All"));
        questCards.Add(new QuestCard("Quest Card", "Boar Hunt", 2, "Boar"));
        questCards.Add(new QuestCard("Quest Card", "Rescue the Fair Maiden", 3, "Black Knight"));
        questCards.Add(new QuestCard("Quest Card", "Slay the Dragon", 3, "Dragon"));
        questCards.Add(new QuestCard("Quest Card", "Search for the Holy Grail", 5, "All"));
        questCards.Add(new QuestCard("Quest Card", "Test of the Green Knight", 4, "Green Knight"));
        questCards.Add(new QuestCard("Quest Card", "Repel the Saxon Raiders", 2, "All Saxons"));
        questCards.Add(new QuestCard("Quest Card", "Vanquish King Arthur's Enemies", 3, null));

        return questCards;
    }

  public List<Card> generateTournamentCards()
    {
        List<Card> tournamentCards = new List<Card>();
        tournamentCards.Add(new TournamentCard("Tournament Card", "Tournament at Camelot", 3));
        tournamentCards.Add(new TournamentCard("Tournament Card", "Tournament at Orkney", 2));
        tournamentCards.Add(new TournamentCard("Tournament Card", "Tournament at Tintagel", 1));
        tournamentCards.Add(new TournamentCard("Tournament Card", "Tournament at York", 0));

        return tournamentCards;
    }

  public List<Card> generateEventCards()
    {
        List<Card> eventCards = new List<Card>();
        eventCards.Add(new EventCard("Event Card", "Chivalrous Deed", "Player(s) with both lowest rank and least amount of shields, receives 3 shields"));
        eventCards.Add(new EventCard("Event Card", "Pox", "All players except the player who drew this card lose 1 shield"));
        eventCards.Add(new EventCard("Event Card", "Plague", "Drawer loses 2 shields if possible"));
        eventCards.Add(new EventCard("Event Card", "King's Recognition", "The next player(s) to complete a Quest will receive 2 extra shields"));
        eventCards.Add(new EventCard("Event Card", "King's Recognition", "The next player(s) to complete a Quest will receive 2 extra shields"));
        eventCards.Add(new EventCard("Event Card", "Queen's Favor", "The lowest ranked player(s) immediately receives 2 Adventure Cards"));
        eventCards.Add(new EventCard("Event Card", "Queen's Favor", "The lowest ranked player(s) immediately receives 2 Adventure Cards"));
        eventCards.Add(new EventCard("Event Card", "Court Called to Camelot", "All Allies in play must be discarded"));
        eventCards.Add(new EventCard("Event Card", "Court Called to Camelot", "All Allies in play must be discarded"));
        eventCards.Add(new EventCard("Event Card", "King's Call to Arms", "The highest ranked player(s) must place 1 weapon in the discard pile. If unable to do so, 2 Foe Cards must be discarded"));
        eventCards.Add(new EventCard("Event Card", "Prosperity throughout the Realm", "All players may immediately draw 2 Adventure Cards"));

        return eventCards;
    }
}
