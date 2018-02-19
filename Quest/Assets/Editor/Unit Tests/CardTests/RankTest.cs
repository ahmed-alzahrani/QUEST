using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class RankTest{

  [Test]
	public void RankCard_CreatedWithGiven_WillHaveTheVariables() {

		var rank = new RankCard("Rank Card", "Squire", " ", 5);
		rank.display();
		Assert.AreEqual("Rank Card", rank.type);
		Assert.AreEqual("Squire", rank.name);
		Assert.AreEqual(" ", rank.texturePath);
		Assert.AreEqual(5, rank.battlePoints);

		// Use the Assert class to test conditions.
	}
}
