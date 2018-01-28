using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class buildAdventureDeck : MonoBehaviour{

  private Deck deck;
  private Card card;
  private RankCard rankCard;
  private Button button;
  private Deck adventureDeck;

  void Awake(){
    // Debug.Log("I am awake!");
    adventureDeck = new Deck("AdventureDeck", 125, buildDeck());
    button = gameObject.GetComponent<Button>();
    button.onClick.AddListener(adventureDeck.display);
  }

  void Update(){
      //  Debug.Log("I exist!!!!!!!!!!!");

  }

  public List<Card> buildDeck(){
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
