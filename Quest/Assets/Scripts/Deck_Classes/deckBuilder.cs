using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class deckBuilder
{

    public deckBuilder() { }

    // building the STORY deck
    public Deck buildStoryDeck()
    {
        return new Deck("Story Deck", buildStoryCards());
    }

    public List<Card> buildStoryCards()
    {
        List<Card> storyCards = new List<Card>();
        List<Card> questCards = generateQuestCards();
        foreach (Card c in questCards)
        {
            storyCards.Add(c);
        }

        List<Card> tournamentCards = generateTournamentCards();
        foreach (Card c in tournamentCards)
        {
            storyCards.Add(c);
        }
        List<Card> eventCards = generateEventCards();
        foreach (Card c in eventCards)
        {
            storyCards.Add(c);
        }


        return storyCards;
    }

    public List<Card> generateQuestCards()
    {
        List<Card> questCards = new List<Card>();

        questCards.Add(new QuestCard("Quest Card", "Journey through the Enchanted Forest", "Textures/Quests/journeyThroughTheEnchantedForest", 3, "Evil Knight"));
        questCards.Add(new QuestCard("Quest Card", "Vanquish King Arthur's Enemies", "Textures/Quests/vanquishKingArthursEnemies", 3, null));
        questCards.Add(new QuestCard("Quest Card", "Repel the Saxon Raiders", "Textures/Quests/repelTheSaxonRaiders", 2, "All Saxons"));
        questCards.Add(new QuestCard("Quest Card", "Boar Hunt", "Textures/Quests/boarHunt", 2, "Boar"));
        questCards.Add(new QuestCard("Quest Card", "Search for the Questing Beast", "Textures/Quests/searchForTheQuestingBeast", 4, null));
        questCards.Add(new QuestCard("Quest Card", "Defend the Queen's Honor", "Textures/Quests/defendTheQueensHonor", 4, "All"));
        questCards.Add(new QuestCard("Quest Card", "Boar Hunt", "Textures/Quests/boarHunt", 2, "Boar"));
        questCards.Add(new QuestCard("Quest Card", "Rescue the Fair Maiden", "Textures/Quests/rescueTheFairMaiden", 3, "Black Knight"));
        questCards.Add(new QuestCard("Quest Card", "Slay the Dragon", "Textures/Quests/slayTheDragon", 3, "Dragon"));
        questCards.Add(new QuestCard("Quest Card", "Search for the Holy Grail", "Textures/Quests/searchForTheHolyGrail", 5, "All"));
        questCards.Add(new QuestCard("Quest Card", "Test of the Green Knight", "Textures/Quests/testOfTheGreenKnight", 4, "Green Knight"));
        questCards.Add(new QuestCard("Quest Card", "Repel the Saxon Raiders", "Textures/Quests/repelTheSaxonRaiders", 2, "All Saxons"));
        questCards.Add(new QuestCard("Quest Card", "Vanquish King Arthur's Enemies", "Textures/Quests/vanquishKingArthursEnemies", 3, null));

        return questCards;
    }

    public List<Card> generateTournamentCards()
    {
        List<Card> tournamentCards = new List<Card>();
        tournamentCards.Add(new TournamentCard("Tournament Card", "Tournament at Camelot", "Textures/tournament/camelot", 3));
        tournamentCards.Add(new TournamentCard("Tournament Card", "Tournament at Orkney", "Textures/tournament/orkney", 2));
        tournamentCards.Add(new TournamentCard("Tournament Card", "Tournament at Tintagel", "Textures/tournament/tintagel", 1));
        tournamentCards.Add(new TournamentCard("Tournament Card", "Tournament at York", "Textures/tournament/york", 0));

        return tournamentCards;
    }

    public List<Card> generateEventCards()
    {
        List<Card> eventCards = new List<Card>();
        eventCards.Add(new EventCard("Event Card", "Chivalrous Deed", "Player(s) with both lowest rank and least amount of shields, receives 3 shields", "Textures/events/chivalrousDeed", new iStoryChivalrousDeed()));
        eventCards.Add(new EventCard("Event Card", "Pox", "All players except the player who drew this card lose 1 shield", "Textures/events/pox", new iStoryPox()));
        eventCards.Add(new EventCard("Event Card", "Plague", "Drawer loses 2 shields if possible", "Textures/events/plague", new iStoryPlague()));
        eventCards.Add(new EventCard("Event Card", "King's Recognition", "The next player(s) to complete a Quest will receive 2 extra shields", "Textures/events/kingsRecognition", new iStoryKingsRecognition()));
        eventCards.Add(new EventCard("Event Card", "King's Recognition", "The next player(s) to complete a Quest will receive 2 extra shields", "Textures/events/kingsRecognition", new iStoryKingsRecognition()));
        eventCards.Add(new EventCard("Event Card", "Queen's Favor", "The lowest ranked player(s) immediately receives 2 Adventure Cards", "Textures/events/queensFavor", new iStoryQueensFavor()));
        eventCards.Add(new EventCard("Event Card", "Queen's Favor", "The lowest ranked player(s) immediately receives 2 Adventure Cards", "Textures/events/queensFavor", new iStoryQueensFavor()));
        eventCards.Add(new EventCard("Event Card", "Court Called to Camelot", "All Allies in play must be discarded", "Textures/events/courtCalledToCamelot", new iStoryCourtCalled()));
        eventCards.Add(new EventCard("Event Card", "Court Called to Camelot", "All Allies in play must be discarded", "Textures/events/courtCalledToCamelot", new iStoryCourtCalled()));
        eventCards.Add(new EventCard("Event Card", "King's Call to Arms", "The highest ranked player(s) must place 1 weapon in the discard pile. If unable to do so, 2 Foe Cards must be discarded", "Textures/events/kingsCallToArms", new iStoryKingsCallToArms()));
        eventCards.Add(new EventCard("Event Card", "Prosperity throughout the Realm", "All players may immediately draw 2 Adventure Cards", "Textures/events/prosperityThroughoutTheRealm", new iStoryProsperity()));
        return eventCards;
    }

    // building the ADVENTURE deck

    public Deck buildAdventureDeck()
    {
        return new Deck("Adventure Deck", buildAdventureCards());
    }

    public List<Card> buildAdventureCards()
    {
        List<Card> adventureCards = new List<Card>();

        List<Card> foeCards = generateFoeCards();
        foreach (Card c in foeCards)
        {
           adventureCards.Add(c);
        }

        List<Card> weaponCards = generateWeaponCards();
        foreach (Card c in weaponCards)
        {
           adventureCards.Add(c);
        }

        List<Card> allyCards = generateAllyCards();
        foreach (Card c in allyCards)
        {
            adventureCards.Add(c);
        }

        List<Card> amourCards = generateAmourCards();
        foreach (Card c in amourCards)
        {
           adventureCards.Add(c);
        }

        List<Card> testCards = generateTestCards();
        foreach (Card c in testCards)
        {
            adventureCards.Add(c);
        }
        return adventureCards;
    }

    public List<Card> generateFoeCards()
    {
        List<Card> foeCards = new List<Card>();
        foeCards.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
        foeCards.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
        foeCards.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
        foeCards.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
        foeCards.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
        foeCards.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
        foeCards.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
        foeCards.Add(new FoeCard("Foe Card", "Saxons", "Textures/foe/saxons", 10, 20));
        foeCards.Add(new FoeCard("Foe Card", "Saxons", "Textures/foe/saxons", 10, 20));
        foeCards.Add(new FoeCard("Foe Card", "Saxons", "Textures/foe/saxons", 10, 20));
        foeCards.Add(new FoeCard("Foe Card", "Saxons", "Textures/foe/saxons", 10, 20));
        foeCards.Add(new FoeCard("Foe Card", "Saxons", "Textures/foe/saxons", 10, 20));
        foeCards.Add(new FoeCard("Foe Card", "Boar", "Textures/foe/boar", 5, 15));
        foeCards.Add(new FoeCard("Foe Card", "Boar", "Textures/foe/boar", 5, 15));
        foeCards.Add(new FoeCard("Foe Card", "Boar", "Textures/foe/boar", 5, 15));
        foeCards.Add(new FoeCard("Foe Card", "Boar", "Textures/foe/boar", 5, 15));
        foeCards.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
        foeCards.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
        foeCards.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
        foeCards.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
        foeCards.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
        foeCards.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
        foeCards.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
        foeCards.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
        foeCards.Add(new FoeCard("Foe Card", "Green Knight", "Textures/foe/greenKnight", 25, 40));
        foeCards.Add(new FoeCard("Foe Card", "Green Knight", "Textures/foe/greenKnight", 25, 40));
        foeCards.Add(new FoeCard("Foe Card", "Black Knight", "Textures/foe/blackKnight", 25, 35));
        foeCards.Add(new FoeCard("Foe Card", "Black Knight", "Textures/foe/blackKnight", 25, 35));
        foeCards.Add(new FoeCard("Foe Card", "Black Knight", "Textures/foe/blackKnight", 25, 35));
        foeCards.Add(new FoeCard("Foe Card", "Evil Knight", "Textures/foe/evilKnight", 20, 30));
        foeCards.Add(new FoeCard("Foe Card", "Evil Knight", "Textures/foe/evilKnight", 20, 30));
        foeCards.Add(new FoeCard("Foe Card", "Evil Knight", "Textures/foe/evilKnight", 20, 30));
        foeCards.Add(new FoeCard("Foe Card", "Evil Knight", "Textures/foe/evilKnight", 20, 30));
        foeCards.Add(new FoeCard("Foe Card", "Evil Knight", "Textures/foe/evilKnight", 20, 30));
        foeCards.Add(new FoeCard("Foe Card", "Evil Knight", "Textures/foe/evilKnight", 20, 30));
        foeCards.Add(new FoeCard("Foe Card", "Saxon Knight", "Textures/foe/saxonKnight", 15, 25));
        foeCards.Add(new FoeCard("Foe Card", "Saxon Knight", "Textures/foe/saxonKnight", 15, 25));
        foeCards.Add(new FoeCard("Foe Card", "Saxon Knight", "Textures/foe/saxonKnight", 15, 25));
        foeCards.Add(new FoeCard("Foe Card", "Saxon Knight", "Textures/foe/saxonKnight", 15, 25));
        foeCards.Add(new FoeCard("Foe Card", "Saxon Knight", "Textures/foe/saxonKnight", 15, 25));
        foeCards.Add(new FoeCard("Foe Card", "Saxon Knight", "Textures/foe/saxonKnight", 15, 25));
        foeCards.Add(new FoeCard("Foe Card", "Saxon Knight", "Textures/foe/saxonKnight", 15, 25));
        foeCards.Add(new FoeCard("Foe Card", "Saxon Knight", "Textures/foe/saxonKnight", 15, 25));
        foeCards.Add(new FoeCard("Foe Card", "Dragon", "Textures/foe/dragon", 50, 70));
        foeCards.Add(new FoeCard("Foe Card", "Giant", "Textures/foe/giant", 40, 40));
        foeCards.Add(new FoeCard("Foe Card", "Giant", "Textures/foe/giant", 40, 40));
        foeCards.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 30, 30));
        foeCards.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 30, 30));
        foeCards.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 30, 30));
        foeCards.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 30, 30));

        return foeCards;
    }

    public List<Card> generateWeaponCards()
    {
        List<Card> weaponCards = new List<Card>();
        weaponCards.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
        weaponCards.Add(new WeaponCard("Weapon Card", "Dagger", "Textures/weapons/dagger", 5));
        weaponCards.Add(new WeaponCard("Weapon Card", "Dagger", "Textures/weapons/dagger", 5));
        weaponCards.Add(new WeaponCard("Weapon Card", "Dagger", "Textures/weapons/dagger", 5));
        weaponCards.Add(new WeaponCard("Weapon Card", "Dagger", "Textures/weapons/dagger", 5));
        weaponCards.Add(new WeaponCard("Weapon Card", "Dagger", "Textures/weapons/dagger", 5));
        weaponCards.Add(new WeaponCard("Weapon Card", "Dagger", "Textures/weapons/dagger", 5));
        weaponCards.Add(new WeaponCard("Weapon Card", "Excalibur", "Textures/weapons/excalibur", 30));
        weaponCards.Add(new WeaponCard("Weapon Card", "Excalibur", "Textures/weapons/excalibur", 30));
        weaponCards.Add(new WeaponCard("Weapon Card", "Lance", "Textures/weapons/lance", 20));
        weaponCards.Add(new WeaponCard("Weapon Card", "Lance", "Textures/weapons/lance", 20));
        weaponCards.Add(new WeaponCard("Weapon Card", "Lance", "Textures/weapons/lance", 20));
        weaponCards.Add(new WeaponCard("Weapon Card", "Lance", "Textures/weapons/lance", 20));
        weaponCards.Add(new WeaponCard("Weapon Card", "Lance", "Textures/weapons/lance", 20));
        weaponCards.Add(new WeaponCard("Weapon Card", "Lance", "Textures/weapons/lance", 20));
        weaponCards.Add(new WeaponCard("Weapon Card", "Battle-Axe", "Textures/weapons/battle-ax", 15));
        weaponCards.Add(new WeaponCard("Weapon Card", "Battle-Axe", "Textures/weapons/battle-ax", 15));
        weaponCards.Add(new WeaponCard("Weapon Card", "Battle-Axe", "Textures/weapons/battle-ax", 15));
        weaponCards.Add(new WeaponCard("Weapon Card", "Battle-Axe", "Textures/weapons/battle-ax", 15));
        weaponCards.Add(new WeaponCard("Weapon Card", "Battle-Axe", "Textures/weapons/battle-ax", 15));
        weaponCards.Add(new WeaponCard("Weapon Card", "Battle-Axe", "Textures/weapons/battle-ax", 15));
        weaponCards.Add(new WeaponCard("Weapon Card", "Battle-Axe", "Textures/weapons/battle-ax", 15));
        weaponCards.Add(new WeaponCard("Weapon Card", "Battle-Axe", "Textures/weapons/battle-ax", 15));
        return weaponCards;
    }

    public List<Card> generateAllyCards()
    {
        string gawain = "+20 BP ON the Test of the Green Knight Quest";
        string pellinore = "+4 Bids on the Search for the Questing Beast Quest";
        string percival = "+20 BP on the Search for the Holy Grail";
        string tristan = "+20 BP When Queen Iseult is in play";
        string merlin = "Player may preview any 1 stage per Quest";
        string iseult = "+ 4 Bids when Sir Tristan is in play";
        string lance = "+25 BP When on the Quest to Defend the Queen's Honor";

        List<Card> allyCards = new List<Card>();
        allyCards.Add(new AllyCard("Ally Card", "Sir Gawain", "Textures/Ally/sirGawain", 10, 0, gawain, "Test of the Green Knight", "None", 0, 20, new BuffOnQuestEffect()));
        allyCards.Add(new AllyCard("Ally Card", "King Pellinore", "Textures/Ally/kingPellinore", 10, 0, pellinore, "Search for the Questing Beast", "None", 4, 0, new BuffOnQuestEffect()));
        allyCards.Add(new AllyCard("Ally Card", "Sir Percival", "Textures/Ally/sirPercival", 5, 0, percival, "Search for the Holy Grail", "None", 0, 20, new BuffOnQuestEffect()));
        allyCards.Add(new AllyCard("Ally Card", "Sir Tristan", "Textures/Ally/sirTristan", 10, 0, tristan, "None", "Queen Iseult", 0, 20, new BuffOnCardInPlayEffect()));
        allyCards.Add(new AllyCard("Ally Card", "King Arthur", "Textures/Ally/kingArthur", 10, 2, "", "None", "None", 0, 0, new NoBuff()));
        allyCards.Add(new AllyCard("Ally Card", "Queen Guinevere", "Textures/Ally/queenGuinevere", 0, 3, "", "None", "None", 0, 0, new NoBuff()));
        allyCards.Add(new AllyCard("Ally Card", "Merlin", "Textures/Ally/merlin", 0, 0, merlin, "None", "None", 0, 0, new NoBuff()));
        allyCards.Add(new AllyCard("Ally Card", "Queen Iseult", "Textures/Ally/queenIseult", 0, 2, iseult, "None", "Sir Tristan", 4, 0, new BuffOnCardInPlayEffect()));
        allyCards.Add(new AllyCard("Ally Card", "Sir Lancelot", "Textures/Ally/sirLancelot", 15, 0, lance, "Defend the Queen's Honor", "None", 0, 25, new BuffOnQuestEffect()));
        allyCards.Add(new AllyCard("Ally Card", "Sir Galahad", "Textures/Ally/sirGalahad", 15, 0, "", "None", "None", 0, 0, new NoBuff()));

        return allyCards;
    }

    public List<Card> generateAmourCards()
    {
        List<Card> amourCards = new List<Card>();
        amourCards.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
        amourCards.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
        amourCards.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
        amourCards.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
        amourCards.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
        amourCards.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
        amourCards.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
        amourCards.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));

        return amourCards;
    }

    public List<Card> generateTestCards()
    {
        List<Card> testCards = new List<Card>();
        testCards.Add(new TestCard("Test Card", "Test of the Questing Beast", "Textures/tests/testOfTheQuestingBeast", 4));
        testCards.Add(new TestCard("Test Card", "Test of the Questing Beast", "Textures/tests/testOfTheQuestingBeast", 4));
        testCards.Add(new TestCard("Test Card", "Test of Temptation", "Textures/tests/testOfTemptation", 3));
        testCards.Add(new TestCard("Test Card", "Test of Temptation", "Textures/tests/testOfTemptation", 3));
        testCards.Add(new TestCard("Test Card", "Test of Valor", "Textures/tests/testOfValor", 3));
        testCards.Add(new TestCard("Test Card", "Test of Valor", "Textures/tests/testOfValor", 3));
        testCards.Add(new TestCard("Test Card", "Test of Morgan Le Fey", "Textures/tests/testOfMorganLeFey", 3));
        testCards.Add(new TestCard("Test Card", "Test of Morgan Le Fey", "Textures/tests/testOfMorganLeFey", 3));
        return testCards;
    }
}
