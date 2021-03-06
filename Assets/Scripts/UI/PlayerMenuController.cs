﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Receives input regarding opening the player's menu. Currently, that's only the Dictionary. Note this controller doesn't handle
/// exiting from the menu, since the controller of each menu window is responsible for that.
/// </summary>
public class PlayerMenuController : MonoBehaviour {

	void Update() {
		if(PlayerWantsToOpenDictionary()) {
			OpenDictionaryMenu();
		}
	}

	private bool PlayerWantsToOpenDictionary() {
		return Input.GetKeyDown(KeyCode.Tab);
	}

	private void OpenDictionaryMenu() {
		//window.setbool(open)
		PlayerMenuNavigationManager.Instance.OpenPlayerMenu();
	}

	public void Enable() {
		enabled = true;
	}

	public void Disable() {
		enabled = false;
	}
}
