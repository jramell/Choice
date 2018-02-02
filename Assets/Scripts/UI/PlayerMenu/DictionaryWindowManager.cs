using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct MatrixPosition {
	public int row;
	public int col;
	public MatrixPosition(int row, int col) {
		this.row = row;
		this.col = col;
	}
}

/// <summary>
/// Handles adding a new word to display in the dictionary in the appropriate WordSlot, and handles which word is selected in the
/// dictionary window. 
/// </summary>
public class DictionaryWindowManager : MonoBehaviour {

	/// <summary>
	/// Image that displays the icon of the currently selected word.
	/// </summary>
	[Tooltip("Image that displays the icon of the currently selected word")]
	public Image wordIconHolder;

	/// <summary>
	/// Text the displays the definition of the currently selected word.
	/// </summary>
	[Tooltip("Text the displays the definition of the currently selected word")]
	public Text wordDefinitionText;

	private static DictionaryWindowManager instance;

	/// <summary>
	/// Matrix position of the word last word slot that has a word on it. "Last" from left to right, top to bottom.
	/// </summary>
	private MatrixPosition lastOccupiedWordSlot;

	/// <summary>
	/// All the word slots in the dictionary window.
	/// </summary>
	private WordSlot[,] wordSlots;

	private WordSlot currentlySelectedWordSlot;

	private DictionaryWindowController dictionaryWindowController;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public static DictionaryWindowManager Instance {
		get { return instance; }
	}

	public void InitializeWordSlots(GameObject[,] slots) {
		lastOccupiedWordSlot = new MatrixPosition(0, 0);
		wordSlots = new WordSlot[slots.GetLength(0), slots.GetLength(1)];
		for (int i = 0; i < slots.GetLength(0); i++) {
			for (int j = 0; j < slots.GetLength(1); j++) {
				wordSlots [i, j] = slots[i, j].GetComponent<WordSlot>();
			}
		}
	}

	public void AddWord(Word word) {
		if(lastOccupiedWordSlot.row == wordSlots.GetLength(0)) {
			return; //no more available word slots	
		}
		wordSlots[lastOccupiedWordSlot.row, lastOccupiedWordSlot.col].Word = word;
		++lastOccupiedWordSlot.col;
		if(lastOccupiedWordSlot.col == wordSlots.GetLength(1)) {
			lastOccupiedWordSlot.col = 0;
			++lastOccupiedWordSlot.row;
		}
	}

	public void RegisterHoveredWordSlot(WordSlot wordSlot) {
		if(currentlySelectedWordSlot != null) {
			currentlySelectedWordSlot.Deselect();
		}
		wordSlot.Select();
		currentlySelectedWordSlot = wordSlot;
		UpdateDisplayedWordInformation();
	}

	private void UpdateDisplayedWordInformation() {
		if(currentlySelectedWordSlot.Word == null) {
			return;
		}
		wordIconHolder.sprite = currentlySelectedWordSlot.Word.Icon;
		wordDefinitionText.text = currentlySelectedWordSlot.Word.Definition;
	}

	public void EnableDictionaryWindowController() {
		if(dictionaryWindowController == null) {
			dictionaryWindowController = GameObject.FindGameObjectWithTag("Player").GetComponent<DictionaryWindowController>();
		}
		dictionaryWindowController.Enable();
	}

	public void DisableDictionaryWindowController() {
		if(dictionaryWindowController == null) {
			dictionaryWindowController = GameObject.FindGameObjectWithTag("Player").GetComponent<DictionaryWindowController>();
		}
		dictionaryWindowController.Disable();
	}
}

