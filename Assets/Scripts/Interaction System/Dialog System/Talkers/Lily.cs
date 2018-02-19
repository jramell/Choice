using System;
using UnityEngine;

public class Lily : Talker { //should listen to crafting system	

	public Conversation firstMeet;

	public Conversation afterKeyCrafted;

	private bool keyCrafted = false;

	public override Dialog Talk() {
		if(keyCrafted) {
			return afterKeyCrafted.nextDialog();
		}
			
		return firstMeet.nextDialog();
	}

}
