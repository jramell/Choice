using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles that only one interaction with the game world is available to the player at any given moment. The last registered 
/// possible interaction will override the current one. Handles that only one input manager of a certain interaction system is listening 
/// for input. This includes the talking system and the movement system, but not actions that are not related directly to the world
/// like accessing menus.
/// 
/// Note: Assumes no new triggers will register while an interaction is happening. 
/// </summary>
public class InteractionManager : MonoBehaviour {

	public PlayerController playerController;
	public Text actionAvailablePrompt;

	private static InteractionManager instance;

	//TODO: Only one trigger at a time is supported, the last one encountered. To support more than one, a list should
	//be created and maintained, where triggers register, but only the last one is active.
	private IInteractionTrigger currentTrigger;

	/// <summary>
	/// Type of the interaction happening currently. Note that an active trigger does not mean an active interaction, just the
	/// possibility for the player to start it.
	/// </summary>
	private Interaction.Type activeInteractionType;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public static InteractionManager Instance {
		get { return instance; }
	}

	/// <summary>
	/// Registers trigger as ready for interaction. Will override the current active trigger, if any.
	/// the current one.
	/// </summary>
	/// <param name="trigger">Trigger to be marked as ready for interaction with the player</param>
	/// <param name="interactionType">Type of interaction the current trigger allows</param> 
	public void RegisterTrigger(IInteractionTrigger trigger, Interaction.Type interactionType) {
		if(currentTrigger != null) {
			currentTrigger.DisableInteraction();
		}
		currentTrigger = trigger;
		currentTrigger.EnableInteraction();
		ShowActionAvailablePrompt();
	}

	/// <summary>
	/// If the current active interaction trigger is the one passed as a parameter, stops considering it active.
	/// Otherwise, doesn't do anything, since it means that the trigger trying to unregister was already overwritten by
	/// another and subsequently disabled.
	/// </summary>
	/// <param name="trigger">Trigger that wants to unregister. If it's the currently active trigger, interaction with
	/// it is disabled and the current trigger is nullified. Otherwise, doesn't do anything.</param>
	public void UnregisterTrigger(IInteractionTrigger trigger) {
		if(currentTrigger == trigger) {
			currentTrigger.DisableInteraction();
			currentTrigger = null;
			HideActionAvailablePrompt();
		}
	}

	public void RegisterActiveInteraction(Interaction.Type interactionType) {
		activeInteractionType = interactionType;
		HideActionAvailablePrompt();
		playerController.Disable();
		SystemManager.Instance.RegisterActiveSystem(GameSystem.Type.Interaction);
	}

	public void UnregisterActiveInteraction() {
		ShowActionAvailablePrompt();
		activeInteractionType = Interaction.Type.None;
		playerController.Enable();
		SystemManager.Instance.UnregisterActiveSystem(GameSystem.Type.Interaction);
	}

	private void HideActionAvailablePrompt() {
		actionAvailablePrompt.text = "";
		actionAvailablePrompt.gameObject.SetActive(false);
	}

	private void ShowActionAvailablePrompt() {
		actionAvailablePrompt.text = "[F] " + currentTrigger.InteractionType();
		actionAvailablePrompt.gameObject.SetActive(true);
	}

	/// <summary>
	/// If an interaction trigger is listening for input, this suspends such listening. Calling RestablishInteraction
	/// would start listening for input again.
	/// </summary>
	public void SuspendInteraction() {
		if(currentTrigger != null) {
			currentTrigger.DisableInteraction();
		}
	}

	/// <summary>
	/// If the interaction that was listening for input was suspended, this will restart listening for input.
	/// </summary>
	public void RestablishInteraction() {
		if(currentTrigger != null) {
			currentTrigger.EnableInteraction();
		}
	}
}
