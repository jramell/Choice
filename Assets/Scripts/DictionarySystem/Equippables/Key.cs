using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Equippable {

	public override void Use() {
		Debug.Log("Key was used!");
	}

	public override void Break() {
		throw new System.NotImplementedException();
	}

	public override void OnCrafted() {
		Debug.Log("Key was crafted!");
	}
}