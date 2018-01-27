using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation : MonoBehaviour {
	[SerializeField]
	[Tooltip("Dialogs that belong to this conversation. They'll be shown in the order they are in the editor.")]
	private Dialog[] dialogs;

	private int currentConversationIndex = 0;

	void Start() {
		if (dialogs.Length == 0) {
			Debug.LogError("There can't be an empty dialog.");
		}
	}

	/// <summary>
	/// Returns the next dialog in the conversation if there is one. Otherwise, returns null and restarts the conversation.
	/// This means that the first call to nextDialog() after it returns null will return the first dialog in the conversation.
	/// </summary>
	/// <returns>Dialog object that comes next in the conversation if there is one, null otherwise. Once it returns null,
	/// 			the conversation will restart and the next call to nextDialog() will return its first dialog.</returns>
	public Dialog nextDialog() {
		if(IsOver()) {
			RestartConversation();
			return null;
		}
		return dialogs[currentConversationIndex++];
	}

	public Dialog currentDialog() {
		return dialogs[currentConversationIndex];
	}
		
	/// <returns><c>true</c>, if over was ised, <c>false</c> otherwise.</returns>
	public bool IsOver() {
		return currentConversationIndex == dialogs.Length;
	}

	void RestartConversation() {
		currentConversationIndex = 0;
	}
}
