using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour {

	void Update() {
		if(PlayerWantsToTogglePauseMenu()) {
			if (PauseManager.Instance.IsPaused) {
				//UI code that deactivates pause menu, its controller, and plays unpausing animation
				SystemManager.Instance.UnregisterActiveSystem(GameSystem.Type.PauseMenu);
			} else {
				//UI code that activates pause menu and plays pausing animation
				SystemManager.Instance.RegisterActiveSystem(GameSystem.Type.PauseMenu);
			}
		}
	}

	private bool PlayerWantsToTogglePauseMenu() {
		return Input.GetKeyDown(KeyCode.Escape);
	}

	public void Disable() {
		enabled = false;
	}

	public void Enable() {
		enabled = true;
	}
}
