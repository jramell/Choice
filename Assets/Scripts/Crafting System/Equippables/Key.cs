using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Key : Equippable {

	private BoxCollider2D hitbox;

	protected override void Start() {
		base.Start();
		hitbox = GetComponent<BoxCollider2D>();
	}

	protected override void OnUsed() {
		
		Debug.Log("Key was used!");

	}

	public override void OnBreak() {
		Debug.Log("Key broke");
	}

	public override void OnCrafted() {
		Debug.Log("Key was crafted!");
	}
}