﻿using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Key : Equippable {

	#region Key Settings variable declarations
	[Header("Key Settings")]
	[SerializeField]
	[Tooltip("Height of the hitbox relative to the center of the player")]
	private float hitboxHeight;

	[SerializeField]
	[Tooltip("Horizontal distance the hitbox will be when used, calculated from hitboxHeight towards the direction the player is looking to.")]
	private float hitboxDistance;
	#endregion
	
	/// <summary>
	/// Distance the player wants the Key's hitbox to be on the current frame
	/// </summary>
	private float targetDistance = 0f;

	private BoxCollider2D hitbox;

	protected override void Start() {
		base.Start();
		hitbox.enabled = false;
	}

	void Update() {
		UpdateHitboxPosAccordingToPlayerOrientation();
		UpdatePlayerAnimatorAccordingToPlayerOrientation();
	}

	protected override void OnUsed() {
		targetDistance = hitboxDistance;
		hitbox.enabled = true;
		//hitbox.enable interaction with stuff, as in... receiving onTriggerEvents from player. Could also be just bool in this script
	}

	public override void OnStopUsing() {
		targetDistance = 0f;
		hitbox.enabled = false;
		ResetPosition();
		SetAnimatorToStandby();
	}

	public override void OnBreak() {
		base.OnBreak();
		ResetAnimator();
		gameObject.SetActive(false);
		enabled = false;
	}

	public override void OnEquipped() {
		base.OnEquipped();
		if(hitbox == null) {
			hitbox = GetComponent<BoxCollider2D>();
			hitbox.isTrigger = true;
		}
		enabled = true;
		gameObject.SetActive(true);
		//handle player animator's state -- UI code that handles that the key is seen for the first time
	}

	public void UpdateHitboxPosAccordingToPlayerOrientation() {
		transform.localPosition = new Vector2(PlayerManager.Instance.PlayerState.PlayerOrientation * targetDistance, hitboxHeight);
		transform.localScale = new Vector3(PlayerManager.Instance.PlayerState.PlayerOrientation, 
											transform.localScale.y, transform.localScale.z);
	}

	/// <summary>
	/// Updadates player animator's state so key is pointing to the correct side
	/// </summary>
	public void UpdatePlayerAnimatorAccordingToPlayerOrientation() {
		//updadate player animator's state so key is pointing to the correct side
	}

	private void ResetPosition() {
		transform.localPosition = Vector2.zero;
	}

	/// <summary>
	/// Updates player animator so that the key is on the "standby" pose
	/// </summary>
	private void SetAnimatorToStandby() {
		//Update player animator so that the key is on the "standby" pose
	}

	/// <summary>
	/// Puts animator in the state it was before obtaining the key
	/// </summary>
	private void ResetAnimator() {

	}

	void OnTriggerEnter2D(Collider2D col) {
		IOpenable openableCollidedWith = col.gameObject.GetComponent<IOpenable>();
		if (openableCollidedWith != null) {
			openableCollidedWith.OnOpened();
			remainingDuration--;
		}
	}

	public override void Unequip() {
		ResetAnimator();
		gameObject.SetActive(false);
		enabled = false;
	}
}