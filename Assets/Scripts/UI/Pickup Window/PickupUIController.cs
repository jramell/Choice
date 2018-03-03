using System;
using UnityEngine;

public class PickupUIController : MonoBehaviour, IController {

	void Update() {
		if(PlayerWantsToDismissPickupUI()) {
			PickupUIManager.Instance.DismissWindow();
		}
	}

	private bool PlayerWantsToDismissPickupUI() {
		return Input.anyKeyDown;
	}
	public void Disable() {
		enabled = false;
	}

	public void Enable() {
		enabled = true;
	}

}
