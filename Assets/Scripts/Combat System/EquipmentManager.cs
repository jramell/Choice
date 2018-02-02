using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {

	private static EquipmentManager instance;
	private Equippable currentEquipment;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public static EquipmentManager Instance {
		get { return instance; }
	}

	/// <summary>
	/// Handles equipping 
	/// </summary>
	/// <param name="equippable">Equippable.</param>
	public void Equip(Equippable equippable) {
		//depending on animation strctur, window.setWeapon("none") before setting next weapon
		//window.setweapon("new equippable") ... let the animator handle the rest
		currentEquipment = equippable;
		if(equippable.DurationType == Duration.Type.Seconds) {
			//start coroutine that counts that
		}
	}

	public void UseEquipment() {
		if(currentEquipment == null) {
			return;
		}
		//window.set("using thing"), or other instruction related to animation
		currentEquipment.Use();
		if(currentEquipment.DurationType == Duration.Type.Uses) {
			currentEquipment.remainingDuration--;
			if(currentEquipment.remainingDuration == 0) {
				BreakCurrentEquipment();
			}
		}
	}

	private void BreakCurrentEquipment() {
		currentEquipment.OnBreak(); //sfx and things
		Unequip();
	}

	private void Unequip() {
		currentEquipment = null;
	}
}
