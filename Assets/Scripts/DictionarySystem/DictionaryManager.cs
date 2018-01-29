using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryManager : MonoBehaviour {

	private static DictionaryManager instance;
	private List<Word> knownWords;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public static DictionaryManager Instance {
		get { return instance; }
	}

	public void AddWord(Word word) {
		if (word == null) {
			return;
		}
		if(knownWords == null) {
			knownWords = new List<Word>();
		} 
		knownWords.Add(word);
		DictionaryWindowManager.Instance.AddWord(word);
	}
}
