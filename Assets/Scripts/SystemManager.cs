﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles transition between systems. Checks if a system can activate in a certain circumstance. If it can't, it will disable it until it can again.
/// For example, when the player is talking, the System Manager disables the PauseController until the conversation is over. Does not handle
/// interactions within a system, only amongst systems. If the player pauses while about to interact, tells the active interaction controller
/// to stop listening for input. Once the player unpauses, tells InteractionManager to keep going.
/// 
/// Handles which transitions should pause or unpause the game, so not every controller has to.
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
			TransitionToInteraction ();
		} else if (systemType == GameSystem.Type.PlayerMenu) {
			TransitionToPlayerMenu();
		}
	}

	public void UnregisterActiveSystem(GameSystem.Type systemType) {
		if (systemType == GameSystem.Type.PauseMenu) {
			TransitionFromPauseMenu();
		} else if (systemType == GameSystem.Type.Interaction) {
			TransitionFromInteraction();
		} else if (systemType == GameSystem.Type.PlayerMenu) {
			TransitionFromPlayerMenu();
		}
	}


	#region Transition methods definitions

	/// <summary>
	/// Called when pause menu is accessed. Suspends current available interactions and 
	/// </summary>
	private void TransitionToPauseMenu() {
		SuspendInteraction();
		DisablePlayerMenu();
		PauseGame();
		//can't do anything else while game is paused
	}

	private void TransitionFromPauseMenu() {
		AwakeInteraction();
		EnablePlayerMenu();
		UnpauseGame();
	}

	/// <summary>
	/// Executed when an interaction is ocurring. Disables the ability to pause and access the dictionary menu.
	/// </summary>
	private void TransitionToInteraction() {
		DisablePauseMenu();
		DisablePlayerMenu();
	}

	/// <summary>
	/// Executed when an interaction finishes happening. Enables the ability to open the pause and the dictionary menus.
	/// </summary>
	private void TransitionFromInteraction() {
		EnablePauseMenu();
		EnablePlayerMenu();
	}

	private void TransitionToPlayerMenu() {
		SuspendInteraction();
		DisablePauseMenu();
		PauseGame();
	}

	private void TransitionFromPlayerMenu() {
		AwakeInteraction();
		EnablePauseMenu();
		EnablePlayerMenu();
		UnpauseGame();
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

	private void EnablePlayerMenu() {
		//DictionaryWindowManager.Instance.DisableDictionaryWindowController();
		MenuNavigationManager.Instance.EnablePlayerMenuController();
	}

	private void DisablePlayerMenu() {
		MenuNavigationManager.Instance.DisablePlayerMenuController();
	}

	private void SuspendInteraction() {
		InteractionManager.Instance.SuspendInteraction();
	}

	private void AwakeInteraction() {
		InteractionManager.Instance.RestablishInteraction();
	}

	private void PauseGame() {
		
		PauseManager.Instance.Pause();
	}

	private void UnpauseGame() {
		PauseManager.Instance.Unpause();
	}

	#endregion
}
