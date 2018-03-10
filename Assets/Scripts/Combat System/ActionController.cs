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
		if (EquipmentManager.Instance.CurrentEquipment == null) {
			return false;
		}
		if (EquipmentManager.Instance.CurrentEquipment.UseInputType == EquipmentInput.Type.KeyPressed) {
			return Input.GetAxis("Fire1") > 0;
		} else {
			return Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0);
		}
	}

	private bool PlayerWantsToStopUsingEquippedObject() {
		if (EquipmentManager.Instance.CurrentEquipment == null) {
			return false;
		}
		if (EquipmentManager.Instance.CurrentEquipment.UseInputType == EquipmentInput.Type.KeyPressed) {
			return Input.GetAxis("Fire1") <= 0;
		} else {
			return Input.GetKeyUp(KeyCode.X) || Input.GetMouseButtonUp(0);
		}
	}

	public void Enable() {
		enabled = true;
	}

	public void Disable() {
		enabled = false;
	}
}
