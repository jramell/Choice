using UnityEngine;

public abstract class Equippable : Wearable {
	
	[Header("Equippable Settings")]
	public AudioSource equipSoundEffect;
	public AudioSource breakSoundEffect;

	/// <summary>
	/// Time that must pass between 
	/// </summary>
	public float cooldown = 0f;

	private float lastTimeUsed = 0f;

	public void Use() {
		if(Time.time - lastTimeUsed >= cooldown) {
			lastTimeUsed = Time.time;
			OnUsed();
		}
	}

	/// <summary>
	/// Executed when player is able to use the equippable. Handles moving a hitbox and checking for certain interactables
	/// within it, in order to interact with them. Animation and other transitions are handled by the EquipmentController,
	/// since it listens for player input. Cooldown calculations are handled automatically by Equipment base class.
	/// </summary>
	protected abstract void OnUsed();

	/// <summary>
	/// Executed when the equipment was being used, and stops being used by the player.
	/// </summary>
	public abstract void OnStopUsing();

	/// <summary>
	/// Executed when the equippable the player is currently using breaks. Handles any on-breaking effects the Equipment
	/// may have. Note that other effects such as animation and sound effects on-breaking are handled by the EquipmentManager,
	/// since it handles the Equipment's lifetime.
	/// </summary>
	public virtual void OnBreak() {
		if(breakSoundEffect != null) {
			breakSoundEffect.Play();
		}
	}

	/// <summary>
	/// Executed when the equippable is equipped by the player
	/// </summary>
	public virtual void OnEquipped() {
		if(equipSoundEffect != null) {
			equipSoundEffect.Play();
		}
		remainingDuration = duration;
	}

	public abstract void Unequip();
}
