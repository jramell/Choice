using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central entity that checks whether a system can activate in a certain circumstance. If it can't, it will disable it until it can again.
/// For example, when the player is talking, the System Manager disables the PauseController until the conversation is over. Does not handle
/// interactions within a system, only amongst systems. If the player pauses while about to interact, tells the active interaction controller
/// to stop listening for input. Once the player unpauses, tells InteractionManager to keep going.
/// </summary>
public class SystemManager : MonoBehaviour {

	private static SystemManager instance;
	private PauseMenuController pauseMenuController;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public static SystemManager Instance {
		get { return instance; }
	}

	public void RegisterActiveSystem(GameSystem.Type systemType) {
		if (systemType == GameSystem.Type.PauseMenu) {
			TransitionToPauseMenu();
		} else if (systemType == GameSystem.Type.Interaction) {
			TransitionToPauseMenu();
		} else if (systemType == GameSystem.Type.DictionaryMenu) {
			TransitionToDictionaryMenu();
		}
	}

	public void UnregisterActiveSystem(GameSystem.Type systemType) {
		if (systemType == GameSystem.Type.PauseMenu) {
			TransitionFromPauseMenu();
		} else if (systemType == GameSystem.Type.Interaction) {
			TransitionFromPauseMenu();
		} else if (systemType == GameSystem.Type.DictionaryMenu) {
			TransitionFromDictionaryMenu();
		}
	}


	#region Transition methods definitions

	/// <summary>
	/// Called when pause menu is accessed. Suspends current available interactions and 
	/// </summary>
	private void TransitionToPauseMenu() {
		SuspendInteraction();
		DisableDictionaryMenu();
		//can't do anything else while game is paused
	}

	private void TransitionFromPauseMenu() {
		AwakeInteraction();
		EnableDictionaryMenu();
	}

	/// <summary>
	/// Executed when an interaction is ocurring. Disables the ability to pause and access the dictionary menu.
	/// </summary>
	private void TransitionToInteraction() {
		DisablePauseMenu();
		DisableDictionaryMenu();
	}

	/// <summary>
	/// Executed when an interaction finishes happening. Enables the ability to open the pause and the dictionary menus.
	/// </summary>
	private void TransitionFromInteraction() {
		EnablePauseMenu();
		EnableDictionaryMenu();
	}

	private void TransitionToDictionaryMenu() {
		SuspendInteraction();
		DisablePauseMenu();
	}

	private void TransitionFromDictionaryMenu() {
		AwakeInteraction();
		EnablePauseMenu();
	}
	#endregion

	#region Helper methods definition

	private void DisablePauseMenu() {
		if(pauseMenuController == null) {
			pauseMenuController = GameObject.FindGameObjectWithTag("Player").GetComponent<PauseMenuController>();
		}
		pauseMenuController.Disable();
	}

	private void EnablePauseMenu() {
		pauseMenuController.Enable();
	}

	private void DisableDictionaryMenu() {
		//once entity exists
	}

	private void EnableDictionaryMenu() {
		//once entity exists
	}

	private void SuspendInteraction() {
		InteractionManager.Instance.SuspendInteraction();
	}

	private void AwakeInteraction() {
		InteractionManager.Instance.RestablishInteraction();
	}

	#endregion
}
