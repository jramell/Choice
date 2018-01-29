using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Receives input regarding opening the player's menu. Currently, that's only the Dictionary. Note this controller doesn't handle
/// exiting from the menu, since the controller of each menu window is responsible for that.
/// </summary>
public class MenuController : MonoBehaviour {

	/// <summary>
	/// Game Object that contains the Player Menu's UI.
	/// </summary>
	[Tooltip("Object that contains the Player Menu's UI")]
	public GameObject playerMenu;

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
		playerMenu.SetActive(true);
		MenuNavigationManager.Instance.OpenPlayerMenu();
	}

	public void Enable() {
		enabled = true;
	}

	public void Disable() {
		enabled = false;
	}
}
