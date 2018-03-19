using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryKingsRecognition : iStory
{
    public iStoryKingsRecognition() { }
    public void execute(List<Player> players, Card storyCard, Controller game)
    {
        // implement King's Recongition
        if (players != null)
        {
            // Implement by affecting bool value in gameController that iStoryQuest will check for before awarding shields
            Debug.Log("The Next player(s) to complete a Quest will receive 2 extra shields");
            game.kingsRecognition = true;
        }
        game.isDoneStoryEvent = true;
    }
}
