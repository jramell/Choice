using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reads Dialog text, which uses Unity tag system, and parses it to return the next piece of string that should be added
/// to the DialogBox, taking into account that one visible character is added to the DialogBox at each step. For example, when
/// processing a color tag around the word "loa", it will return the letter 'l' surrounded by the same color tag, then the 'o' and
/// the 'a' surrounded by the same tags. This way, it's possible to use Unity's Rich Text system without breaking the dialog
/// implementation. Also, each tag's type should not have spaces after opening the bracket.
/// </summary>
public class CharacterParser {

	private class TagTransformation {
		public string openingTag;
		public string closingTag;
		public TagTransformation(string openingTag, string closingTag) {
			this.openingTag = openingTag;
			this.closingTag = closingTag;
		}
	}

	private string currentSentence = "";
	private int currentCharIndex = 0;
	private LinkedList<TagTransformation> currentTransformations;

	public CharacterParser() {
		currentTransformations = new LinkedList<TagTransformation>();
	}

	/// <summary>
	/// Prepares the character parser to parse a certain string that uses Unity's Rich Text system. If sentence is empty or null,
	/// does nothing. After loading, the sentence.Length successive calls to NextDialog() will return the next character that should
	/// be added to the DialogBox. This character will be correctly formatted according to Unity's Rich Text.
	/// </summary>
	/// <param name="sentence">sentence that will be parsed with successive calls to NextDialog()</param>
	public void LoadDialog(string sentence) {
		if(string.IsNullOrEmpty(sentence)) {
			return;
		}
		currentSentence = sentence;
	}

	/// <summary>
	/// Returns the next character that should be added to the DialogBox surrounded by all the appropriate transformations
	/// defined in the currently loaded dialog. Can be called once per non-tag defining character in the currently loaded dialog.
	/// After that many calls, successive calls before loading a new dialog would return an empty string.
	/// </summary>
	/// 
	/// <returns></returns>
	public string NextString() {
		if(currentCharIndex >= currentSentence.Length) {
			return "";
		}
		char nextCharacter = NextCharacter();
		bool nextCharacterDefinesTag = nextCharacter == '<';
		while (nextCharacterDefinesTag) {
			string tagBeingRead = nextCharacter.ToString();
			nextCharacter = NextCharacter();
			while(nextCharacter != '>') {
				tagBeingRead += nextCharacter;
				nextCharacter = NextCharacter();
			}
			tagBeingRead += '>';
			nextCharacter = NextCharacter();
			bool isClosingTag = tagBeingRead[1] == '/';
			if(isClosingTag) {
				RemoveTransformationWithClosingTag(tagBeingRead);
			} else {
				string closingTag = "</";
				for (int i = 1; i < tagBeingRead.Length && IsEnglishCharacter(tagBeingRead[i]); i++) {
					closingTag += tagBeingRead[i];
				}
				closingTag += ">";
				currentTransformations.AddLast(new TagTransformation(tagBeingRead, closingTag));
			}
			nextCharacterDefinesTag = nextCharacter == '<';
		}
		if (currentCharIndex == currentSentence.Length) {
			ResetState();
		}
		return ApplyCurrentTransformations(nextCharacter);
	}

	private void ResetState() {
		currentCharIndex = 0;
		currentSentence = "";
	}

	private string ApplyCurrentTransformations(char character) {
		string answer = character.ToString();
		foreach(TagTransformation transformation in currentTransformations) {
			answer = transformation.openingTag + answer;
			answer += transformation.closingTag;
		}
		return answer;
	}

	private void PrintTransformations() {
		foreach(TagTransformation transformation in currentTransformations) {
			Debug.Log(transformation.openingTag);
		}
	}

	private void RemoveTransformationWithClosingTag(string openingTag) {
		var currentNode = currentTransformations.First;
		LinkedListNode<TagTransformation> nextNode;
		while(currentNode != null) {
			nextNode = currentNode.Next;
			if(currentNode.Value.closingTag == openingTag) {
				currentTransformations.Remove(currentNode);
				break;
			}
			currentNode = nextNode;
		}
	}

	private bool IsEnglishCharacter(char character) {
		return character >= 'A' && character <= 'z';
	}

	///<summary>
	/// Returns first character that hasn't been read in the current dialog and advances pointer to next
	/// unread character
	///</summary>
	/// <returns>First character in the current dialog that hasn't been read</returns>
	private char NextCharacter() {
		return currentSentence[currentCharIndex++];
	}
}