using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles access to the dialog manager and ensures no two conversations can exist at the same time, 
/// </summary>
public class DialogManager : MonoBehaviour {

	/// <summary>
	/// Time between each character of a dialog getting drawn.
	/// </summary>
	[Tooltip("Time between each character of a dialog getting drawn")]
	public float drawSpeed = 0.05f;

	/// <summary>
	/// Text element where dialog is drawn on screen.
	/// </summary>
	[Tooltip("Text element where dialog is drawn on screen")]
	public Text dialogText;

	[Tooltip("Animator of the UI part of the dialog")]
	public Animator dialogBoxAnimator;

	[Tooltip("Prompt that appears when a dialog is available")]
	public GameObject dialogAvailablePrompt;

	private static DialogManager instance;
	private Dialog currentDialog;
	private bool isDrawing = false;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public static DialogManager Instance {
		get { return instance; }
	}

	public bool IsDrawing {
		get { return isDrawing; }
	}

	public void SetDialogAvailable(bool available) {
		dialogAvailablePrompt.SetActive(available);
	}

	/// <summary>
	/// Starts and displays the conversation passed as a parameter. This conversation will override the current conversation
	/// being displayed, if any. After calling this, 
	/// </summary>
	/// <param name="conversation">Conversation.</param>
	public void StartDialog(Dialog dialog) {
		if (dialog == null) {
			FinishConversation();
			return;
		}
		if (!IsInConversation()) {
			dialogBoxAnimator.SetBool("Open", true);
			if(dialog.useNPCVoiceSFX) {
				dialog.Interlocutor.voiceSFX.Play(); //SFX is NPC's voice
			} else {
				dialog.soundEffect.Play(); //SFX is the custom one defined in the dialog
			}
		}
		currentDialog = dialog;
		DisplayCurrentDialog();
		SetDialogAvailable(false);
	}

	private void DisplayCurrentDialog()  {
		StartCoroutine(DrawDialogCharacters());
	}

	public void SkipCurrentDialog() {
		AbortDrawingCharacters();
		dialogText.text = currentDialog.Sentence;
	}

	private void AbortDrawingCharacters() {
		StopCoroutine(DrawDialogCharacters());
		isDrawing = false;
	}

	/// <summary>
	/// Writes the dialog characters of the current conversation's current dialog.
	/// </summary>
	/// <returns>The dialog characters.</returns>
	private IEnumerator DrawDialogCharacters() {
		isDrawing = true;
		dialogText.text = ""; //just to be safe, in case DialogManager does not do this in other methods
		foreach (char character in currentDialog.Sentence) {
			dialogText.text += character;
			yield return new WaitForSeconds(drawSpeed);
		}
		isDrawing = false;
	}

	private void FinishConversation() {
		Debug.Log ("Conversation finished");
		dialogText.text = "";
		dialogBoxAnimator.SetBool("Open", false);
		currentDialog = null;
		SetDialogAvailable(true); //assumes player can't move while in a conversation
	}

	public bool IsInConversation() {
		return currentDialog != null;
	}
}
