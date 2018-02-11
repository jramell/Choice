using System;
using UnityEngine;

/// <summary>
/// Handles cannon behavior
/// </summary>
public class CannonAI : MonoBehaviour, IHealthChangedListener {
	[SerializeField]
	private float shootCooldown;

	[SerializeField]
	private GameObject shotPrefab;

	private Cannon controlledCannon;

	void Start() {
		controlledCannon = GetComponent<Cannon>();
		controlledCannon.RegisterHealthChangedListener(this);
		//instantiate your shot prefab, deactivate it and pool it somewhere. Tell it you're its creator.
		//if more than one shot prefab can be unpooled, instantiate more. Alternatively, wait until you need
		//to fire a new one, and instantiate another (that will be pooled once it becomes inactive).
	}

	void Update() {
		//set your shot prefab's position to be that of your spawn point, activate it so it moves. 
		//Once it impacts the player, it'll tell you it did, so you can deactivate it and pool it again.
	}

	public void OnHealthChanged() {
		//destroy all pooled objects and disable self. Other script will handle visual feedback.
	}
}
