using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages input while in the dictionary UI menu, including player clicks.
/// </summary>
public class DictionaryUIController : MonoBehaviour {

	/// <summary>
	/// True if the crafting options in the Dictionary Menu are visible and interactable for the player, false otherwise.
	/// </summary>
	private bool craftWindowOpen;

	void Update () {
		if(PlayerWantsToReturnInWindow()) {
			ReturnInWindow();
		} else if(PlayerWantsToExitMenu()) {
			ExitDictionaryMenu();
		}
	}

	private void ReturnInWindow() {
		if(craftWindowOpen) {
			CloseCraftWindow();
		} else {
			ExitDictionaryMenu();
		}
	}

	private void CloseCraftWindow() {
		//make craft window invisible, definitions window visible
	}

	private void ExitDictionaryMenu() {
		CloseCraftWindow();
		//window.setbool(close) ...
	}

	private bool PlayerWantsToReturnInWindow() {
		return Input.GetKeyDown(KeyCode.Escape);
	}

	private bool PlayerWantsToExitMenu() {
		return Input.GetKeyDown(KeyCode.Tab);
	}
}
