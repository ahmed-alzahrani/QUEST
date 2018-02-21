using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class iStoryKingsRecognitionTest{
  [Test]
  public void KingsRecognition_changes_gameControllerBool_toTrue(){
    iStoryKingsRecognition recognition = new iStoryKingsRecognition();
    List<Player> players = new List<Player>();
    RankCard storyCard = new RankCard("Story Card", "Test Card", "texture", 5);
    GameController game = new GameController();

    // King's recognition should be false in GC before the event executes
    Assert.AreEqual(false, game.kingsRecognition);

    recognition.execute(players, storyCard, game);

    // King's Recognition should now be true that the event executed
    Assert.AreEqual(true, game.kingsRecognition);

  }
}
