using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles navigating in around the player's menu that contains the Dictionary. Activates and deactivates the controller of the
/// current window appropriately.
/// </summary>
public class MenuNavigationManager : MonoBehaviour {

	private static MenuNavigationManager instance;
	private MenuController menuController;
	private DictionaryWindowController dictionaryController;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public static MenuNavigationManager Instance {
		get { return instance; }
	}

	/// <summary>
	/// Called when the Dictionary Menu becomes the focus of the game.
	/// </summary>
	public void FocusDictionary() {
		if(menuController == null) {
			menuController = GameObject.FindGameObjectWithTag("Player").GetComponent<MenuController>();
		}
		if(dictionaryController == null) {
			dictionaryController = GameObject.FindGameObjectWithTag("Player").GetComponent<DictionaryWindowController>();
		}
		menuController.Disable();
		dictionaryController.Enable();
	}

	public void ExitPlayerMenu() {
		menuController.Enable();
		dictionaryController.Disable();
	}
}
