using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class AmourTest{

  [Test]
	public void AmourCard_CreatedWithGiven_WillHaveTheVariables() {

		var amour = new AmourCard("Amour Card", "Amour", " ", 1, 10);
		amour.display();
		Assert.AreEqual("Amour Card", amour.type);
		Assert.AreEqual("Amour", amour.name);
		Assert.AreEqual(" ", amour.texturePath);
		Assert.AreEqual(1, amour.bid);
		Assert.AreEqual(10, amour.battlePoints);

		// Use the Assert class to test conditions.
	}
}
