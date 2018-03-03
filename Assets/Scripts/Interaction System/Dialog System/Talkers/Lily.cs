using System;
using UnityEngine;

public class Lily : Talker, IOnCraftedListener { //should listen to crafting system	

	public Conversation firstMeet;

	public Conversation afterKeyCrafted;

	private bool keyCrafted = false;

	void Start() {
		CraftingManager.Instance.AddCraftingListener(this);
	}

	public override Dialog Talk() {
		if(keyCrafted) {
			return afterKeyCrafted.nextDialog();
		}
		return firstMeet.nextDialog();
	}

	public void OnWordCrafted() {
		keyCrafted = true;
		CraftingManager.Instance.RemoveCraftingListener(this);
	}
}
