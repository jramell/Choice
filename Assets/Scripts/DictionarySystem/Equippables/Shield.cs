using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Equippable {

	public override void Use() {
		Debug.Log("Shield was used!");
	}

	public override void Break() {
		throw new System.NotImplementedException();
	}

	public override void OnCrafted() {
		Debug.Log("Shield was crafted!");
	}
}