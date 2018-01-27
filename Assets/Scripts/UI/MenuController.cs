using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Receives input regarding opening the player's menu. Currently, that's only the Dictionary. Note this controller doesn't handle
/// exiting from the menu, since the controller of each menu window is responsible for that.
/// </summary>
public class MenuController : MonoBehaviour {

	void Update() {
		if(PlayerWantsToOpenDictionary()) {
			OpenDictionaryMenu();
		}
	}

	private bool PlayerWantsToOpenDictionary() {
		return Input.GetKeyDown(KeyCode.Tab);
	}

	private void OpenDictionaryMenu() {
		PauseManager.Instance.Pause();
		MenuNavigationManager.Instance.FocusDictionary();
		SystemManager.Instance.RegisterActiveSystem(GameSystem.Type.DictionaryMenu);
	}

	public void Enable() {
		enabled = true;
	}

	public void Disable() {
		enabled = false;
	}
}
