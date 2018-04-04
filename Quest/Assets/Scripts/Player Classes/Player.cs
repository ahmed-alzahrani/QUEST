using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player
{
    //member variables
    public string name { get; set; }
    public int score { get; set; }
    public RankCard rankCard { get; set; }
    public List<RankCard> rankCards { get; set; }
    public List<Card> hand { get; set; }
    public List<AllyCard> activeAllies { get; set; }
    public iStrategy strategy { get; set; }
    public int knightScore;
    public int champKnightScore;
    public int kotrkScore;
    public bool participating = false;
    public bool sponsoring = false;
    public string shieldPath;
    public Controller gameController;
    public UnityEngine.Networking.NetworkIdentity connection;

    //member functions
    public Player(string playerName, List<Card> startingHand, iStrategy strat, string shieldPth = "", int knight = 5, int champKnight = 12, int knightOfTheRoundTable = 22, UnityEngine.Networking.NetworkIdentity connect = null)
    {
        name = playerName;
        score = 0;
        hand = startingHand;
        activeAllies = new List<AllyCard>();
        rankCard = new RankCard("Rank Card", "Squire", "Textures/Ranks/squire", 5);
        List<RankCard> ranks = new List<RankCard>();
        RankCard KnightCard = new RankCard("Rank Card", "Knight", "Textures/Ranks/knight ", 10);
        RankCard champKnightCard = new RankCard("Rank Card", "Champion Knight", "Textures/Ranks/championKnight", 20);
        ranks.Add(KnightCard);
        ranks.Add(champKnightCard);
        rankCards = ranks;
        strategy = strat;
        shieldPath = shieldPth;
        knightScore = knight;
        champKnightScore = champKnight;
        kotrkScore = knightOfTheRoundTable;
        connection = connect;
    }

    public Player() { }

    public void discard(List<Card> discard)
    {
        for (int i = 0; i < discard.Count; i++)
        {
            hand.Remove(discard[i]);
            // GameController.AdventureDeck.discard(discard);
        }
    }

    public int calculateBid(string questName, List<Player> players)
    {
      int total = 0;
      for (int i = 0; i < activeAllies.Count; i++)
      {
        total += activeAllies[i].getFreeBid(questName, players);
      }
      return total;
    }

    public int CalculateBP(string questName, List<Player> players)
    {
        int total = 0;
        for (int i = 0; i < activeAllies.Count; i++)
        {
            total += activeAllies[i].getBattlePoints(questName, players);
        }

        return total + rankCard.battlePoints;
    }

    // this is where the logic should be added for ranking up a player
    public void addShields(int shields)
    {
        rankUpCheck(score, shields);
        score += shields;
    }

    public bool rankUpCheck(int score, int shields)
    {
        // Can rank up from Squire to Knight
        if (score < knightScore && (score + shields) >= knightScore)
        {
            rankChangeToKnight();
        }
        // Can rank up from Knight to Champion Knight
        if (score < champKnightScore && (score + shields) >= champKnightScore)
        {
            rankChangeToChampionKnight();
        }
        // Can become a Knight of the Round Table
        if (score < kotrkScore && (score + shields) > kotrkScore)
        {
            Debug.Log("We have a winner!");
            return true;
        }
        return false;
    }

    public void rankChangeToChampionKnight()
    {
        for (int i = 0; i < rankCards.Count; i++)
        {
            if (rankCards[i].name == "Champion Knight")
            {
                RankCard temp = rankCards[i];
                rankCards[i] = rankCard;
                rankCard = temp;
            }
        }
    }

    public void rankChangeToKnight()
    {
        for (int i = 0; i < rankCards.Count; i++)
        {
            if (rankCards[i].name == "Knight")
            {
                RankCard temp = rankCards[i];
                rankCards[i] = rankCard;
                rankCard = temp;
            }
        }
    }

    public void rankChangeToSquire()
    {
        for (int i = 0; i < rankCards.Count; i++)
        {
            if (rankCards[i].name == "Squire")
            {
                RankCard temp = rankCards[i];
                rankCards[i] = rankCard;
                rankCard = temp;
            }
        }
    }

    public void rankDownCheck(int score, int shields)
    {
        // rank down to Knight
        if (score >= champKnightScore && (score - shields) < champKnightScore)
        {
            rankChangeToKnight();
        }
        // rank down to Squire
        if (score >= 5 && (score - shields) < 5)
        {
            rankChangeToSquire();
        }
    }
    // similiarly, the logic for implementing ranking down needs to be added here
    public void removeShields(int shields)
    {
        rankDownCheck(score, shields);
        score -= shields;
        if (score < 0)
        {
            score = 0;
        }
    }

    public List<Card> courtCalled()
    {
        List<Card> allies = new List<Card>();
        for (var i = 0; i < activeAllies.Count; i++)
        {
            allies.Add(activeAllies[i]);
        }
        for (var i = 0; i < allies.Count; i++)
        {
            AllyCard ally = (AllyCard)allies[i];
            activeAllies.Remove(ally);
        }

        return allies;
    }

    public void drawCards(List<Card> cards) {
        for (int i = 0; i < cards.Count; i++)
        {
            hand.Add(cards[i]);
        }
        handCheck();
    }

    public void discardCards(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            hand.Remove(cards[i]);
        }
    }

    public bool handCheck()
    {
        return hand.Count > 12;
    }

    public List<Card> kingsCall()
    {
        if (hasWeapon())
        {
            List<Card> discardCards = strategy.discardWeapon(hand);
            return discardCards;
        }
        else if (hasFoes())
        {
            List<Card> discardCards = strategy.discardFoesForKing(hand);
            return discardCards;
        }
        else
        {
            return new List<Card>();
        }
    }

    public bool hasWeapon()
    {
        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].type == "Weapon Card")
            {
                return true;
            }
        }
        return false;
    }

    public bool hasFoes()
    {
        int numFoes = 0;

        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].type == "Foe Card")
            {
                numFoes++;
            }
        }

        return numFoes > 1;
    }

    public bool hasAlly(string Ally)
    {
        for (int i = 0; i < activeAllies.Count; i++)
        {
            if (activeAllies[i].name == Ally)
            {
                return true;
            }
        }
        return false;
    }

    public void display()
    {
        Debug.Log(name);

        if (connection == null)
        {
            Debug.Log("The connection is null, this is a CPU player.");
        }
        else
        {
          if (connection.isServer){
            Debug.Log(connection.connectionToClient.connectionId);
          } else {
            Debug.Log(connection.connectionToServer.connectionId);
          }
        }
    }
}
