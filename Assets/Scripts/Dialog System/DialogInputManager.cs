﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Listens for player input related to starting, skipping, advancing, and finishing dialog. Is enabled and disabled
/// by DialogTrigger, so it's not listening every frame. Is attached to the player.
/// 
/// If working on a mobile version, it should be enough to make it a listener of the UI element that would 
/// </summary>
[RequireComponent(typeof(Collider2D))]	
public class DialogInputManager : MonoBehaviour {

	/// <summary>
	/// The talking entity that the player wants to interact with when the DialogInputManager is active.
	/// </summary>
	private Talker currentTalker;

	/// <summary>
	/// List of DialogAvailableListeners that are called 
	/// </summary>
	private IOnDialogAvailableListener listeners;

	//TODO: it's for PC version, mobile version could simply listen for clicks in the interface.
	void Update() {
		if (!DialogManager.Instance.IsDrawing && ButtonToTalkPressed()) {
			StartConversation();
		} 
	}

	/// <summary>
	/// Starts listening for player dialog related input. On PC, this means listening for key presses, while on mobile
	/// it means to just listen for clicks on dialog interface.
	/// </summary>
	public void Enable(Talker talker) {
		currentTalker = talker;
		enabled = true;
		DialogManager.Instance.SetDialogAvailable(true);
	}

	/// <summary>
	/// Stops listening for player dialog related input. On PC, this means listening for key presses, while on mobile
	/// it means to just listen for clicks on dialog interface.
	/// </summary>
	public void Disable() {
		enabled = false;
		DialogManager.Instance.SetDialogAvailable(false);
	}

	private bool ButtonToTalkPressed() {
		return Input.GetKeyDown(KeyCode.F);
	}

	private void StartConversation() {
		DialogManager.Instance.StartDialog(currentTalker.Talk());
	}
}
