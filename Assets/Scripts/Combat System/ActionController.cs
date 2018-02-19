﻿using System.Collections;
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
		return Input.GetAxis("Fire1") > 0;
	}

	private bool PlayerWantsToStopUsingEquippedObject() {
		return Input.GetAxis("Fire1") <= 0;
	}

	public void Enable() {
		enabled = true;
	}

	public void Disable() {
		enabled = false;
	}
}
