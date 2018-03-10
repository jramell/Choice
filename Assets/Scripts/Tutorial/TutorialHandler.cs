using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handlers are objects that handle both a controller's job (listening for world input) and a manager's (maintaining and updating
/// world state). The Tutorial Handler does that for the tutorial part of the game.
/// </summary>
public class TutorialHandler : MonoBehaviour {

	private enum TutorialState {
		Movement, //teaching the player how to move
		Jump, //teaching the player how to jump
		Click, //telling the player to click on orange words
		CraftMode, //telling the player to push the button in the interface
		WriteWord, //telling the player to write a word into the crafting input field
		Craft //
	}
}
