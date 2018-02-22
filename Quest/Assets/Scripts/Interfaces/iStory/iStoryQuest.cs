using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStages
{
    public static string questStage = "Sponsoring";
    public static int sponsorIndex = 0;
}

public class iStoryQuest : iStory
{
    public void execute(List<Player> players, Card storyCard, GameController game)
    {
        // implement Quest logic
        //QuestStage.questStage == "Sponsoring";
        //check for sponsors (if we have a sponsor continue else end this quest)
        //check for participants (if we don't have participants end quest)
        //Have sponsor setup the quest
    }
}
