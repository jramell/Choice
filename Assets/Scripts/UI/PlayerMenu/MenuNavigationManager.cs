using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles navigating in around the player's menu that contains the Dictionary. Activates and deactivates the controller of the
/// current window appropriately. Does not listen for input, as that is done by controllers and UI elements. Does not handle 
/// triggering animations for changing windows, as that is handled by controllers and UI elements.
/// </summary>
public class MenuNavigationManager : MonoBehaviour {

	private static MenuNavigationManager instance;
	private MenuController playerMenuController;
	private DictionaryWindowController dictionaryController;

	private IController currentWindowController;

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

	public void OpenPlayerMenu() {
		FocusDictionary();
		SystemManager.Instance.RegisterActiveSystem(GameSystem.Type.PlayerMenu);
	}

	/// <summary>
	/// Called when the Dictionary Menu becomes the focus of the game.
	/// </summary>
	public void FocusDictionary() {
		if(playerMenuController == null) {
			playerMenuController = GameObject.FindGameObjectWithTag("Player").GetComponent<MenuController>();
		}
		if(dictionaryController == null) {
			dictionaryController = GameObject.FindGameObjectWithTag("Player").GetComponent<DictionaryWindowController>();
		}
		playerMenuController.Disable();
		dictionaryController.Enable();
		currentWindowController = dictionaryController;
	}

	public void ExitPlayerMenu() {
		currentWindowController.Disable();
		SystemManager.Instance.UnregisterActiveSystem(GameSystem.Type.PlayerMenu);
	}

	public void DisablePlayerMenuController() {
		InitializePlayerMenuController();
		playerMenuController.Disable();
	}

	public void EnablePlayerMenuController() {
		InitializePlayerMenuController();
		playerMenuController.Enable();
	}

	private void InitializePlayerMenuController() {
		if(playerMenuController == null) {
			playerMenuController = GameObject.FindGameObjectWithTag("Player").GetComponent<MenuController>();
		}
	}
}
