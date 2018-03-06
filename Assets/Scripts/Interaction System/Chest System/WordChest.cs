using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordChest : Chest {

	[Tooltip("Word collected by inspecting this chest")]
	public Word word;

	protected override void StartCollectionSequence() {
		PickupManager.Instance.ObtainWord(word);
	}
}
