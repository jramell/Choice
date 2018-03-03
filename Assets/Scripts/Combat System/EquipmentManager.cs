using UnityEngine;

public class EquipmentManager : MonoBehaviour {

	private static EquipmentManager instance;
	private Equippable currentEquipment;

	void Awake() {
		if (instance == null) {
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
		if(currentEquipment != null) {
			currentEquipment.Unequip();
		}
		currentEquipment = equippable;
		currentEquipment.OnEquipped();
		if (equippable.DurationType == Duration.Type.Seconds) {
			//start coroutine that counts that
		}
	}

	public void UseEquipment() {
		HandleEquipmentRemainingDuration();
		if (currentEquipment == null) {
			return;
		}
		//window.set("using thing"), or other instruction related to animation
		currentEquipment.Use();
	}

	public void StopUsingEquipment() {
		HandleEquipmentRemainingDuration();
		if (currentEquipment == null) {
			return;
		}
		currentEquipment.OnStopUsing();
	}

	private void BreakCurrentEquipment() {
		currentEquipment.OnStopUsing();
		currentEquipment.OnBreak(); //sfx and things
		Unequip();
	}

	private void Unequip() {
		currentEquipment = null;
	}

	private void HandleEquipmentRemainingDuration() {
		if (currentEquipment == null) {
			return;
		}
		if (currentEquipment.DurationType == Duration.Type.Uses) {
			//does not reduce item duration, since that's done by the item itself if its duration is uses
			if (currentEquipment.RemainingDuration <= 0) {
				BreakCurrentEquipment();
			}
		}
	}
}
