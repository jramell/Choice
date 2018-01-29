using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryManager : MonoBehaviour {

	private static DictionaryManager instance;
	private Dictionary<string, Word> knownWords;

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
			knownWords = new Dictionary<string, Word>();
		} 
		knownWords.Add(word.WordName, word);
		DictionaryWindowManager.Instance.AddWord(word);
	}

	/// <summary>
	/// Returns true if the dictionary currently contains the word passed as a parameter, false otherwise
	/// </summary>
	public bool Contains(string word) {
		if(knownWords == null) {
			return false;
		}
		return knownWords.ContainsKey(word);
	}
		
	/// <returns>Word with name wordName if it's found in the player's dictionary. Null otherwise</returns>
	public Word GetWordWithName(string wordName) {
		Word answer = null;
		knownWords.TryGetValue(wordName, out answer);
		return answer;
	}
}
