﻿using UnityEngine;

public class ElectricGun : Equippable {

	#region Projectile Spawn Settings variable declarations
	[Header("Projectile Spawn Settings")]
	public Transform player;

	[Tooltip("Prefab of the electric projectile the gun shoots")]
	public GameObject projectilePrefab;

	public Transform projectileSpawn;

	[Header("Sound Settings")]
	[SerializeField]
	private AudioSource shootSoundEffect;

	[Tooltip("Horizontal distance from the player's center the spawn point for the projectile will have")]
	private float shootSpawnHorizontalDistance;

	[Tooltip("Vertical distance from the player's center the spawn point for the projectile will have")]
	private float shootSpawnVerticalDistance;

	#endregion

	#region Projectile Settings variable declarations
	[Header("Projectile Settings")]
	[Tooltip("How strong the shock caused by projectiles is")]
	public float shockStrength;

	[Tooltip("The speed in units per second the gun's projectiles should have")]
	public float projectileSpeed;

	#endregion

	protected override void Start() {
		base.Start();
		shootSpawnHorizontalDistance = Mathf.Abs(projectileSpawn.transform.position.x - player.position.x);
		shootSpawnVerticalDistance = Mathf.Abs(projectileSpawn.transform.position.y - player.position.y);
		ElectricProjectile projectileComponent = projectilePrefab.GetComponent<ElectricProjectile>();
		projectileComponent.shockStrength = shockStrength;
		projectileComponent.speed = projectileSpeed;
		if(player == null) {
			player = GameObject.FindGameObjectWithTag("Player").transform;
		}
	}

	void Update() {
		UpdateSpawnPosAccordingToPlayerOrientation();
		UpdatePlayerAnimatorAccordingToPlayerOrientation();
	}

	public override void OnEquipped() {
		base.OnEquipped();
		enabled = true;
		gameObject.SetActive(true);
		//appear gun sprite / change animator state
	}

	public override void OnBreak() {
		base.OnBreak();
		enabled = false;
		gameObject.SetActive(false);
		//disappear gun sprite / change animator state
	}

	public override void OnStopUsing() {
		//nothing, really
	}

	protected override void OnUsed() {
		//shoot projectile... which is pooled, eventually
		SpawnElectricProjectile();
		if(shootSoundEffect != null) {
			shootSoundEffect.Play();
		}
		remainingDuration--;
	}

	public override void Unequip() {
		enabled = false;
		gameObject.SetActive(false);
	}

	private void SpawnElectricProjectile() {
		Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
	}

	private void UpdateSpawnPosAccordingToPlayerOrientation() {
		float playerOrientation = PlayerManager.Instance.PlayerState.PlayerOrientation;
		projectileSpawn.position = new Vector2(transform.position.x + shootSpawnHorizontalDistance*playerOrientation,
											   transform.position.y + shootSpawnVerticalDistance*playerOrientation);
		bool playerLookingLeft = playerOrientation < 0f;
		projectileSpawn.rotation = playerLookingLeft ? Quaternion.Euler(0, 180, 0) : Quaternion.identity; 
	}

	private void UpdatePlayerAnimatorAccordingToPlayerOrientation() {
		//change animation state if necessary
		transform.rotation = projectileSpawn.rotation; //temporal, to show gun direction
	}
}
