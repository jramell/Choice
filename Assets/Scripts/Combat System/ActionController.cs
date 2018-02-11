using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Listens for input related to using an equipped object or an equipped buff.
/// </summary>
public class ActionController : MonoBehaviour {

	void Update() {
		if(PlayerWantsToUseEquippedObject()) {
			EquipmentManager.Instance.UseEquipment();
		} else if(PlayerWantsToStopUsingEquippedObject()) {
			EquipmentManager.Instance.StopUsingEquipment();
		}
	}

	private bool PlayerWantsToUseEquippedObject() {
		return Input.GetKey(KeyCode.X);
	}

	private bool PlayerWantsToStopUsingEquippedObject() {
		return Input.GetKeyUp(KeyCode.X);
	}

	public void Enable() {
		enabled = true;
	}

	public void Disable() {
		enabled = false;
	}
}
