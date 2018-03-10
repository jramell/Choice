using System.Collections;
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

	public DialogController dialogController;

	private static SystemManager instance;
	private PauseMenuController pauseMenuController;
	private ActionController actionController;

//	private class SystemState {
//		bool pauseMenuControllerEnabled;
//		bool actionControllerEnabled;
//		GameSystem.Type activeSystem;
//	}

	void Awake() {
		instance = this;
	}

	public static SystemManager Instance {
		get { return instance; }
	}

	public void RegisterActiveSystem(GameSystem.Type systemType) {
		if(systemType == GameSystem.Type.PauseMenu) {
			TransitionToPauseMenu();
		} else if(systemType == GameSystem.Type.Interaction) {
			TransitionToInteraction();
		} else if(systemType == GameSystem.Type.PlayerMenu) {
			TransitionToPlayerMenu();
		} else if(systemType == GameSystem.Type.PickupUI) {
			TransitionToPickupUI();
		}
	}

	public void UnregisterActiveSystem(GameSystem.Type systemType) {
		if (systemType == GameSystem.Type.PauseMenu) {
			TransitionFromPauseMenu();
		} else if (systemType == GameSystem.Type.Interaction) {
			TransitionFromInteraction();
		} else if (systemType == GameSystem.Type.PlayerMenu) {
			TransitionFromPlayerMenu();
		} else if (systemType == GameSystem.Type.PickupUI) {
			TransitionFromPickupUI();
		}
	}


	#region Transition methods definitions

	/// <summary>
	/// Called when pause menu is accessed. Suspends current available interactions and 
	/// </summary>
	private void TransitionToPauseMenu() {
		SuspendInteraction();
		DisablePlayerMenu();
		DisableActionInput();
		PauseGame();
		//can't do anything else while game is paused
	}

	private void TransitionFromPauseMenu() {
		AwakeInteraction();
		EnablePlayerMenu();
		EnableActionInput();
		UnpauseGame();
	}

	/// <summary>
	/// Executed when an interaction is ocurring. Disables the ability to pause and access the dictionary menu.
	/// </summary>
	private void TransitionToInteraction() {
		DisablePauseMenu();
		DisablePlayerMenu();
		DisableActionInput();
	}

	/// <summary>
	/// Executed when an interaction finishes happening. Enables the ability to open the pause and the dictionary menus.
	/// </summary>
	private void TransitionFromInteraction() {
		EnablePauseMenu();
		EnablePlayerMenu();
		EnableActionInput();
	}

	private void TransitionToPlayerMenu() {
		SuspendInteraction();
		DisablePauseMenu();
		PauseGame();
		DisableActionInput();
	}

	private void TransitionFromPlayerMenu() {
		AwakeInteraction();
		EnablePauseMenu();
		EnablePlayerMenu();
		UnpauseGame();
		EnableActionInput();
	}

	private void TransitionToPickupUI() { //this is the bug!
		SuspendInteraction();
		DisablePauseMenu();
		DisableActionInput();
		PauseGame(); 
		DisablePlayerMenu();
	}

	private void TransitionFromPickupUI() {
		AwakeInteraction();
		UnpauseGame();
		//assumes that the pickup UI is returning towards a conversation. If this stops being the case, code must
		//be changed
		if (InteractionManager.Instance.ActiveInteractionType == Interaction.Type.Talk) {
			dialogController.AdvanceConversation(); 
		} else {
			EnablePlayerMenu();
			EnableActionInput();
		}
	}

	#endregion

	#region Helper methods definition

	private void DisablePauseMenu() {
		if(pauseMenuController == null) {
			pauseMenuController = PlayerManager.Instance.PlayerGameObject.GetComponent<PauseMenuController>();
		}
		pauseMenuController.Disable();
	}

	private void EnablePauseMenu() {
		pauseMenuController.Enable();
	}

	private void EnablePlayerMenu() {
		PlayerMenuNavigationManager.Instance.EnablePlayerMenuController();
	}

	private void DisablePlayerMenu() {
		PlayerMenuNavigationManager.Instance.DisablePlayerMenuController();
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

	private void DisableActionInput() {
		if(actionController == null) {
			actionController = PlayerManager.Instance.PlayerGameObject.GetComponent<ActionController>();
		}
		actionController.Disable();
	}

	private void EnableActionInput() {
		actionController.Enable();
	}

	#endregion
}