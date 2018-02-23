using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AllyCard : Card
{
    // member variables

    public int battlePoints { get; set; }
    public int freeBid { get; set; }
    public string special{ get; set; }
    public string specialQuest { get; set; }
    public string specialAlly  { get; set;}
    public int bidBuff {get; set;}
    public int bpBuff {get; set;}
    public AllyEffect allyEffect{ get; set; }

    // member functions
    public AllyCard(string cardType, string cardName, string texture, int bp, int bid, string specialSkill, string quest, string ally, int bidPlus, int bpPlus, AllyEffect aEffect)
    {
        type = cardType;
        name = cardName;
        texturePath = texture;
        battlePoints = bp;
        freeBid = bid;
        special = specialSkill;
        specialQuest = quest;
        specialAlly = ally;
        bidBuff = bidPlus;
        bpBuff = bpPlus;
        allyEffect = aEffect;

    }

    public int getBattlePoints(string questName, List<Player> players)
    {
      if (bpBuff == 0)
      {
        return battlePoints;
      }
      if (specialQuest != "")
      {
        return battlePoints + allyEffect.BuffOnQuest(specialQuest, questName, bpBuff);
      }
      if (specialAlly != "")
      {
        return battlePoints + allyEffect.BuffOnCardInPlay(players, specialAlly, bidBuff);
      }
      return battlePoints;
    }

    public int getFreeBid(string questName, List<Player> players)
    {
      if (bidBuff == 0)
      {
        return freeBid;
      }
      if (specialQuest != "")
      {
        return freeBid + allyEffect.BuffOnQuest(specialQuest, questName, bidBuff);
      }
      if (specialAlly != "")
      {
        return freeBid + allyEffect.BuffOnCardInPlay(players, specialAlly, bidBuff);
      }
      return battlePoints;
    }

    public bool hasSpecial()
    {
        return (this.special != "");
    }

    // static void executeSpecial() {} execute a specific card's given special ability
}
