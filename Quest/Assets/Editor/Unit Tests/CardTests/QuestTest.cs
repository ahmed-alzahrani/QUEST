using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class QuestTest{

  [Test]
	public void QuestCard_CreatedWithGiven_WillHaveTheVariables() {

		var quest = new QuestCard("Quest Card", "Journey through the Enchanted Forest", " ", 3, "Evil Knight");
		quest.display();
		Assert.AreEqual("Quest Card", quest.type);
		Assert.AreEqual("Journey through the Enchanted Forest", quest.name);
		Assert.AreEqual(" ", quest.texturePath);
		Assert.AreEqual(3, quest.stages);
		Assert.AreEqual("Evil Knight", quest.foe);
    Assert.IsNotNull(quest.quest);

		// Use the Assert class to test conditions.
	}
}
