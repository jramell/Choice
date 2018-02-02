using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Equippable {

	protected override void OnUsed() {
		Debug.Log("Shield was used!");
	}

	public override void OnBreak() {
		Debug.Log ("Shield broke!");
	}

	public override void OnCrafted() {
		Debug.Log("Shield was crafted!");
	}
}