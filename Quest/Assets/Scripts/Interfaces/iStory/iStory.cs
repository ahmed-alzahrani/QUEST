using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iStory {
  bool execute(List<Player> players, Card storyCard, Deck adventure);
}
