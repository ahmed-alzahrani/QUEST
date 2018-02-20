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
    public string shieldPath;
    public GameController gameController;

    //member functions
    public Player(string playerName, List<Card> startingHand, iStrategy strat, string shieldPth = "")
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

    public int CalculateBP()
    {
        int total = 0;
        for (int i = 0; i < activeAllies.Count; i++)
        {
            total += activeAllies[i].battlePoints;
        }

        return total + rankCard.battlePoints;
    }

    // this is where the logic should be added for ranking up a player
    public void addShields(int shields)
    {
        rankUpCheck(score, shields);
        score += shields;
    }

    public void rankUpCheck(int score, int shields)
    {
        // Can rank up from Squire to Knight
        if (score < 5 && (score + shields) >= 5)
        {
            rankChangeToKnight();
        }
        // Can rank up from Knight to Champion Knight
        if (score < 7 && (score + shields) >= 7)
        {
            rankChangeToChampionKnight();
        }
        // Can become a Knight of the Round Table
        if (score < 10 && (score + shields) > 10)
        {
            Debug.Log("We have a winner!");
        }
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
        if (score >= 7 && (score - shields) < 7)
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

    public List<AllyCard> courtCalled()
    {
        List<AllyCard> allies = new List<AllyCard>();
        for (var i = 0; i < activeAllies.Count; i++)
        {
            allies.Add(activeAllies[i]);
        }
        for (var i = 0; i < allies.Count; i++)
        {
            activeAllies.Remove(allies[i]);
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

    public void handCheck()
    {
        if (hand.Count > 12) {
            Debug.Log("You have more than 12 cards you need to fix your hand");
            // fix your hand
        }
    }

    /*
    public void drawFromAdvenutreDeck(int count)
    {
      List<Card> cardsToDraw = game.drawFromAdvenutreDeck(count);
      if(hand.Count + cardsToDraw.Count <= 12)
      {
        for(int i = 0; i < cardsToDraw.Count; i++)
        {
          hand.Add(cardsToDraw[i]);
        }
      } else{
        // resolveHandIssue(cardsToDraw, hand);
      }

    }
*/


    /*
      public void resolveHandIssue(List<Card> extra, List<Card> hand)
      {
        for(int i = 0; i < extra.Count; i++)
        {
          if (extra[i].type == "Ally")
          {
            activeAllies.Add(extra[i]);
          }
        }

        for(int i = 0; i < extra.Count; i++)
        {
          if (extra[i].type == "Ally")
          {
            activeAllies.Add(extra[i]);
          }
        }

        if ((extra.Count + hand.Count) > 12)
        {
          List<Card> cardsToDiscard = strategy.getDiscards(extra, hand);
        }

        for(int i = 0; i < extra.Count; i++)
        {
          hand.Add(extra[i]);
        }
      }
      */

    public List<Card> kingsCall()
    {
        if (hasWeapon())
        {
            List<Card> discardCards = strategy.discardWeapon(hand);
            discard(discardCards);
            return discardCards;
        }
        if (hasFoe())
        {
            List<Card> discardCards = strategy.discardFoesForKing(hand);
            discard(discardCards);
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

    public bool hasFoe()
    {
        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].type == "Foe Card")
            {
                return true;
            }
        }
        return false;
    }

    // public void drawCard(deckToDrawFrom){}

    //public Card playCard(){}
}
