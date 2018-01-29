using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magueo : MonoBehaviour {

	public Word word1;
	public Word word2;
	public Word word3;

	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			DictionaryManager.Instance.AddWord(word1);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			DictionaryManager.Instance.AddWord(word2);
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			DictionaryManager.Instance.AddWord(word3);
		}
	}
}
