using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class EventTest{

  [Test]
	public void EventCard_CreatedWithGiven_WillHaveTheVariables() {

		var eventTest = new EventCard("Event", "Pox", "Some Event called Pox", " ", new iStoryPox());
		eventTest.display();
		Assert.AreEqual("Event", eventTest.type);
		Assert.AreEqual("Pox", eventTest.name);
    Assert.AreEqual("Some Event called Pox", eventTest.description);
		Assert.AreEqual(" ", eventTest.texturePath);
    Assert.IsNotNull(eventTest.effect);
	//	Assert.AreEqual(new iStoryPox(), eventTest.effect);

		// Use the Assert class to test conditions.
	}
}
