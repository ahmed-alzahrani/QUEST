using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class deckBuilder{

  public deckBuilder(){}

  // building the RANK deck
  public Deck buildRankDeck(){
    return new Deck("Rank Deck", 12, buildRankCards());
  }

  public List<Card> buildRankCards(){
    List<Card> rankCards = new List<Card>();
    for (int i = 0; i < 4; i++){
      RankCard squireCard = new RankCard("Rank", "Squire", 5);
      rankCards.Add(squireCard);
    }
    for (int i = 0; i < 4; i++){
      RankCard KnightCard = new RankCard("Rank", "Knight", 10);
      rankCards.Add(KnightCard);
    }
    for (int i = 0; i < 4; i++){
      RankCard champKnightCard = new RankCard("Rank", "Champion Knight", 20);
      rankCards.Add(champKnightCard);
    }
    return rankCards;
  }

  // building the STORY deck
  public Deck buildStoryDeck(){
    return new Deck("Story Deck", 28, buildStoryCards());
  }

  public List<Card> buildStoryCards(){
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

        // building the ADVENTURE deck

        public Deck buildAdventureDeck(){
          return new Deck("Adventure Deck", 125, buildAdventureCards());
        }

        public List<Card> buildAdventureCards(){
          List<Card> adventureCards = new List<Card>();

          List<Card> foeCards = generateFoeCards();
          foreach(Card c in foeCards)
              {
                  adventureCards.Add(c);
              }

          List<Card> weaponCards = generateWeaponCards();
          foreach(Card c in weaponCards)
              {
                  adventureCards.Add(c);
              }

          List<Card> allyCards = generateAllyCards();
          foreach(Card c in allyCards)
              {
                  adventureCards.Add(c);
              }

          List<Card> amourCards = generateAmourCards();
          foreach(Card c in amourCards)
              {
                  adventureCards.Add(c);
              }

          List<Card> testCards = generateTestCards();
          foreach(Card c in testCards)
              {
                  adventureCards.Add(c);
              }
          return adventureCards;
        }

        public List<Card> generateFoeCards(){
          List<Card> foeCards = new List<Card>();
          foeCards.Add(new FoeCard("Foe Card", "Robber Knight", 15, 15));
          foeCards.Add(new FoeCard("Foe Card", "Robber Knight", 15, 15));
          foeCards.Add(new FoeCard("Foe Card", "Robber Knight", 15, 15));
          foeCards.Add(new FoeCard("Foe Card", "Robber Knight", 15, 15));
          foeCards.Add(new FoeCard("Foe Card", "Robber Knight", 15, 15));
          foeCards.Add(new FoeCard("Foe Card", "Robber Knight", 15, 15));
          foeCards.Add(new FoeCard("Foe Card", "Robber Knight", 15, 15));
          foeCards.Add(new FoeCard("Foe Card", "Saxons", 10, 20));
          foeCards.Add(new FoeCard("Foe Card", "Saxons", 10, 20));
          foeCards.Add(new FoeCard("Foe Card", "Saxons", 10, 20));
          foeCards.Add(new FoeCard("Foe Card", "Saxons", 10, 20));
          foeCards.Add(new FoeCard("Foe Card", "Saxons", 10, 20));
          foeCards.Add(new FoeCard("Foe Card", "Boar", 5, 15));
          foeCards.Add(new FoeCard("Foe Card", "Boar", 5, 15));
          foeCards.Add(new FoeCard("Foe Card", "Boar", 5, 15));
          foeCards.Add(new FoeCard("Foe Card", "Boar", 5, 15));
          foeCards.Add(new FoeCard("Foe Card", "Thieves", 5, 5));
          foeCards.Add(new FoeCard("Foe Card", "Thieves", 5, 5));
          foeCards.Add(new FoeCard("Foe Card", "Thieves", 5, 5));
          foeCards.Add(new FoeCard("Foe Card", "Thieves", 5, 5));
          foeCards.Add(new FoeCard("Foe Card", "Thieves", 5, 5));
          foeCards.Add(new FoeCard("Foe Card", "Thieves", 5, 5));
          foeCards.Add(new FoeCard("Foe Card", "Thieves", 5, 5));
          foeCards.Add(new FoeCard("Foe Card", "Thieves", 5, 5));
          foeCards.Add(new FoeCard("Foe Card", "Green Knight", 25, 40));
          foeCards.Add(new FoeCard("Foe Card", "Green Knight", 25, 40));
          foeCards.Add(new FoeCard("Foe Card", "Black Knight", 25, 35));
          foeCards.Add(new FoeCard("Foe Card", "Black Knight", 25, 35));
          foeCards.Add(new FoeCard("Foe Card", "Black Knight", 25, 35));
          foeCards.Add(new FoeCard("Foe Card", "Evil Knight", 20, 30));
          foeCards.Add(new FoeCard("Foe Card", "Evil Knight", 20, 30));
          foeCards.Add(new FoeCard("Foe Card", "Evil Knight", 20, 30));
          foeCards.Add(new FoeCard("Foe Card", "Evil Knight", 20, 30));
          foeCards.Add(new FoeCard("Foe Card", "Evil Knight", 20, 30));
          foeCards.Add(new FoeCard("Foe Card", "Evil Knight", 20, 30));
          foeCards.Add(new FoeCard("Foe Card", "Saxon Knight", 15, 25));
          foeCards.Add(new FoeCard("Foe Card", "Saxon Knight", 15, 25));
          foeCards.Add(new FoeCard("Foe Card", "Saxon Knight", 15, 25));
          foeCards.Add(new FoeCard("Foe Card", "Saxon Knight", 15, 25));
          foeCards.Add(new FoeCard("Foe Card", "Saxon Knight", 15, 25));
          foeCards.Add(new FoeCard("Foe Card", "Saxon Knight", 15, 25));
          foeCards.Add(new FoeCard("Foe Card", "Saxon Knight", 15, 25));
          foeCards.Add(new FoeCard("Foe Card", "Saxon Knight", 15, 25));
          foeCards.Add(new FoeCard("Foe Card", "Dragon", 50, 70));
          foeCards.Add(new FoeCard("Foe Card", "Giant", 40, 40));
          foeCards.Add(new FoeCard("Foe Card", "Giant", 40, 40));
          foeCards.Add(new FoeCard("Foe Card", "Mordred", 30, 30));
          foeCards.Add(new FoeCard("Foe Card", "Mordred", 30, 30));
          foeCards.Add(new FoeCard("Foe Card", "Mordred", 30, 30));
          foeCards.Add(new FoeCard("Foe Card", "Mordred", 30, 30));

              return foeCards;
        }
        public List<Card> generateWeaponCards(){
          List<Card> weaponCards = new List<Card>();
          weaponCards.Add(new WeaponCard("Weapon Card", "Horse", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Horse", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Horse", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Horse", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Horse", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Horse", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Horse", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Horse", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Horse", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Horse", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Horse", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Sword", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Sword", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Sword", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Sword", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Sword", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Sword", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Sword", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Sword", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Sword", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Sword", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Sword", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Sword", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Sword", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Sword", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Sword", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Sword", 10));
          weaponCards.Add(new WeaponCard("Weapon Card", "Dagger", 5));
          weaponCards.Add(new WeaponCard("Weapon Card", "Dagger", 5));
          weaponCards.Add(new WeaponCard("Weapon Card", "Dagger", 5));
          weaponCards.Add(new WeaponCard("Weapon Card", "Dagger", 5));
          weaponCards.Add(new WeaponCard("Weapon Card", "Dagger", 5));
          weaponCards.Add(new WeaponCard("Weapon Card", "Dagger", 5));
          weaponCards.Add(new WeaponCard("Weapon Card", "Excalibur", 30));
          weaponCards.Add(new WeaponCard("Weapon Card", "Excalibur", 30));
          weaponCards.Add(new WeaponCard("Weapon Card", "Lance", 20));
          weaponCards.Add(new WeaponCard("Weapon Card", "Lance", 20));
          weaponCards.Add(new WeaponCard("Weapon Card", "Lance", 20));
          weaponCards.Add(new WeaponCard("Weapon Card", "Lance", 20));
          weaponCards.Add(new WeaponCard("Weapon Card", "Lance", 20));
          weaponCards.Add(new WeaponCard("Weapon Card", "Lance", 20));
          weaponCards.Add(new WeaponCard("Weapon Card", "Battle-Axe", 15));
          weaponCards.Add(new WeaponCard("Weapon Card", "Battle-Axe", 15));
          weaponCards.Add(new WeaponCard("Weapon Card", "Battle-Axe", 15));
          weaponCards.Add(new WeaponCard("Weapon Card", "Battle-Axe", 15));
          weaponCards.Add(new WeaponCard("Weapon Card", "Battle-Axe", 15));
          weaponCards.Add(new WeaponCard("Weapon Card", "Battle-Axe", 15));
          weaponCards.Add(new WeaponCard("Weapon Card", "Battle-Axe", 15));
          weaponCards.Add(new WeaponCard("Weapon Card", "Battle-Axe", 15));
          return weaponCards;
        }
        public List<Card> generateAllyCards(){
          List<Card> allyCards = new List<Card>();
          allyCards.Add(new AllyCard("Ally Card", "Sir Gaiwan", 10, "+20 on the Test of the Green Knight Quest"));
          allyCards.Add(new AllyCard("Ally Card", "King Pellinore", 10, "4 Bids on the Search for the Questing Beast Quest"));
          allyCards.Add(new AllyCard("Ally Card", "Sir Percival", 5, "+ 20 on the Search for the Holy Grail Quest"));
          allyCards.Add(new AllyCard("Ally Card", "Sir Tristan", 10, "+ 20 when Queen Iseult is in play"));
          allyCards.Add(new AllyCard("Ally Card", "King Arthur", 10, "+ 2 Bids"));
          allyCards.Add(new AllyCard("Ally Card", "Queen Guinevere", 0, "+ 2 Bids"));
          allyCards.Add(new AllyCard("Ally Card", "Merlin", 0, "Player may preview any 1 stage per Quest"));
          allyCards.Add(new AllyCard("Ally Card", "Queen Iseult", 0, "2 Bids. 4 Bids when Tristan is in play"));
          allyCards.Add(new AllyCard("Ally Card", "Sir Lancelot", 15, "+ 25 When on the Quest to Defend the Queen's Honor"));
          allyCards.Add(new AllyCard("Ally Card", "Sir Galahad", 15, ""));

          return allyCards;
        }
        public List<Card> generateAmourCards(){
          List<Card> amourCards = new List<Card>();
          amourCards.Add(new AmourCard("Amour Card", "Amour", 10));
          amourCards.Add(new AmourCard("Amour Card", "Amour", 10));
          amourCards.Add(new AmourCard("Amour Card", "Amour", 10));
          amourCards.Add(new AmourCard("Amour Card", "Amour", 10));
          amourCards.Add(new AmourCard("Amour Card", "Amour", 10));
          amourCards.Add(new AmourCard("Amour Card", "Amour", 10));
          amourCards.Add(new AmourCard("Amour Card", "Amour", 10));
          amourCards.Add(new AmourCard("Amour Card", "Amour", 10));

          return amourCards;
        }
        public List<Card> generateTestCards(){
          List<Card> testCards = new List<Card>();
          testCards.Add(new TestCard("Test Card", "Test of the Questing Beast", 4));
          testCards.Add(new TestCard("Test Card", "Test of the Questing Beast", 4));
          testCards.Add(new TestCard("Test Card", "Test of Temptation", 3));
          testCards.Add(new TestCard("Test Card", "Test of Temptation", 3));
          testCards.Add(new TestCard("Test Card", "Test of Valor", 3));
          testCards.Add(new TestCard("Test Card", "Test of Valor", 3));
          testCards.Add(new TestCard("Test Card", "Test of Morgan Le Fey", 3));
          testCards.Add(new TestCard("Test Card", "Test of Morgan Le Fey", 3));
          return testCards;
        }







}
