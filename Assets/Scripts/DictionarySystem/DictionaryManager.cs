using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maintains a list of words that have been acquired by the player
/// </summary>
public class DictionaryManager : MonoBehaviour {

	private static DictionaryManager instance;
	private Dictionary<string, Word> knownWords;
	private List<IOnWordObtainedListener> listeners;

	void Awake() {
		instance = this;
		listeners = new List<IOnWordObtainedListener>();
		knownWords = new Dictionary<string, Word>();
	}

	public static DictionaryManager Instance {
		get { return instance; }
	}

	public void AddWord(Word word) {
		if (word == null) {
			return;
		}
		knownWords.Add(word.WordName.ToLower(), word);
		DictionaryWindowManager.Instance.AddWord(word);
		foreach (IOnWordObtainedListener listener in listeners) {
			listener.OnWordObtained();
		}
	}

	/// <summary>
	/// Returns true if the dictionary currently contains the word passed as a parameter, false otherwise
	/// </summary>
	public bool Contains(string wordName) {
		if(knownWords == null) {
			return false;
		}
		return knownWords.ContainsKey(wordName.ToLower());
	}
		
	/// <returns>Word with name wordName if it's found in the player's dictionary. Null otherwise</returns>
	public Word GetWordWithName(string wordName) {
		Word answer = null;
		if(knownWords != null) {
			knownWords.TryGetValue(wordName.ToLower(), out answer);
		}
		return answer;
	}

	public void AddWordObtainedListener(IOnWordObtainedListener listener) {
		listeners.Add(listener);
	}
}
