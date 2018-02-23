using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface AllyEffect{
  int BuffOnQuest(string quest, string currentQuest, int buff);
  int BuffOnCardInPlay(List<Player> players, string Ally, int buff);
}
