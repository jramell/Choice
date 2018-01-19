using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cain : Talker {

	public Conversation intro;

	public override Dialog Talk () {
		return intro.nextDialog();
	}
}
