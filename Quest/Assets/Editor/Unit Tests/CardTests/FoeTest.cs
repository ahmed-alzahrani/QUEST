using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class FoeTest{

  [Test]
	public void FoeCard_CreatedWithGiven_WillHaveTheVariables() {

		var foe = new FoeCard("Foe Card", "Robber Knight", " ", 15, 20);
		foe.display();
		Assert.AreEqual("Foe Card", foe.type);
		Assert.AreEqual("Robber Knight", foe.name);
		Assert.AreEqual(" ", foe.texturePath);
		Assert.AreEqual(15, foe.minBP);
		Assert.AreEqual(20, foe.maxBP);

		// Use the Assert class to test conditions.
	}
}
