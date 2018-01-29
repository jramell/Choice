using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages input while in the dictionary UI menu, including button presses and word "highlighting".
/// </summary>
public class DictionaryWindowController : MonoBehaviour {

	/// <summary>
	/// True if the crafting options in the Dictionary Menu are visible and interactable for the player, false otherwise.
	/// </summary>
	private bool craftWindowOpen;

	void Update () {
		if(PlayerWantsToReturnInWindow()) {
			ReturnInWindow();
		} else if(PlayerWantsToExitMenu()) {
			ExitPlayerMenu();
		}
	}

	private void ReturnInWindow() {
		if(craftWindowOpen) {
			CloseCraftWindow();
		} else {
			ExitPlayerMenu();
		}
	}

	private void CloseCraftWindow() {
		//make craft window invisible, definitions window visible
	}

	/// <summary>
	/// Exits menu where the player's dictionary and (future) other options are, resuming
	/// normal game.
	/// </summary>
	private void ExitPlayerMenu() {
		CloseCraftWindow();
		MenuNavigationManager.Instance.ExitPlayerMenu();
		//window.setbool(close) ...
	}

	private bool PlayerWantsToReturnInWindow() {
		return Input.GetKeyDown(KeyCode.Escape);
	}

	private bool PlayerWantsToExitMenu() {
		return Input.GetKeyDown(KeyCode.Tab);
	}

	public void Enable() {
		enabled = true;
	}

	public void Disable() {
		enabled = false;
	}
}
