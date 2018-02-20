using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour {

	private static PickupManager instance;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public static PickupManager Instance {
		get { return instance; }
	}

	public void ObtainWord(Word word) {
		DictionaryManager.Instance.AddWord(word);
		PickupUIManager.Instance.DisplayWithWord(word);
	}
}
