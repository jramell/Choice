using System;
using UnityEngine;

/// <summary>
/// Receives stimuli about how the player's combat-like interactions with the world. This includes getting
/// electrocuted, or simply impacted by some damaging projectile.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PlayerCombatController : MonoBehaviour, IShockable {

	public void ReceiveShock(float shockStrength) {
		//what should the player do when they receive a shock? lose health, and what else?
	}
}
