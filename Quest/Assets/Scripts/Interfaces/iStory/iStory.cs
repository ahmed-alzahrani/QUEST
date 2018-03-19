using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iStory
{
    void execute(List<Player> players, Card storyCard, Controller game);
}
