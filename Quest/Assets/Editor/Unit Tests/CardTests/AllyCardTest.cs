using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class AllyCardTest{
	[Test]
	public void AllyCard_CreatedWithSpecial_ReturnsTrue(){
		var ally = new AllyCard("Ally Card", "Ayrton", " ", 10, "Special");
		Assert.IsTrue(ally.hasSpecial());

	}

	[Test]
	public void AllyCard_CreatedWithoutSpecial_ReturnsFalse(){
		var ally = new AllyCard("Ally Card", "Ayrton", " ", 10, "");
		Assert.IsFalse(ally.hasSpecial());
	}

	[Test]
	public void AllyCard_CreatedWithGiven_WillHaveTheVariables() {

		var ally = new AllyCard("Ally Card", "Ayrton", " ", 10, "None");
		ally.display();
		Assert.AreEqual("Ally Card", ally.type);
		Assert.AreEqual("Ayrton", ally.name);
		Assert.AreEqual(" ", ally.texturePath);
		Assert.AreEqual(10, ally.battlePoints);
		Assert.AreEqual("None", ally.special);

		// Use the Assert class to test conditions.
	}
	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode

	/*
	[UnityTest]
	public IEnumerator NewEditModeTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame

	 	yield return null;
	}
*/



}
