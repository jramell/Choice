using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles navigating in around the player's menu that contains the Dictionary. Activates and deactivates the controller of the
/// current window appropriately. Does not listen for input, as that is done by controllers and UI elements. Does not handle 
/// triggering animations for changing windows, as that is handled by controllers and UI elements.
/// </summary>
public class PlayerMenuNavigationManager : MonoBehaviour {

	private static PlayerMenuNavigationManager instance;
	private PlayerMenuController playerMenuController;
	private DictionaryWindowController dictionaryController;

	private IController currentWindowController;
	private List<IDictionaryWindowOpenedListener> dictionaryOpenedListeners;

	void Awake() {
		instance = this;
		dictionaryOpenedListeners = new List<IDictionaryWindowOpenedListener>();
	}

	public static PlayerMenuNavigationManager Instance {
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
			playerMenuController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMenuController>();
		}
		if(dictionaryController == null) {
			dictionaryController = GameObject.FindGameObjectWithTag("Player").GetComponent<DictionaryWindowController>();
		}
		playerMenuController.Disable();
		dictionaryController.Enable();
		currentWindowController = dictionaryController;
		foreach(IDictionaryWindowOpenedListener listener in dictionaryOpenedListeners) {
			listener.OnDictionaryWindowOpened();
		}
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

	public void AddDictionaryWindowOpenedListener(IDictionaryWindowOpenedListener listener) {
		dictionaryOpenedListeners.Add(listener);
	}

	private void InitializePlayerMenuController() {
		if(playerMenuController == null) {
			playerMenuController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMenuController>();
		}
	}
}
