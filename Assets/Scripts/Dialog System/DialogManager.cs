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

	private static DialogManager instance;
	private Dialog currentDialog;
	private bool isDrawing = false;
	private Coroutine drawingCoroutine;

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

	/// <summary>
	/// If no conversation is active, displays the dialog passed as a parameter in a new conversation. If a conversation is already
	/// active and the dialog is not null, displays the dialog as part of the conversation. If the dialog is null, finishes the current
	/// conversation or does nothing when no conversation is active.
	/// </summary>
	/// <param name="conversation">Conversation.</param>
	public void AdvanceDialog(Dialog dialog) {
		if (dialog == null) {
			FinishConversation();
			return;
		}
		if (!IsInConversation()) {
			dialogBoxAnimator.SetBool("Open", true);
			dialogText.gameObject.SetActive(true); //for performance, as noted in 
		}
		if(dialog.useNPCVoiceSFX) {
			dialog.Interlocutor.voiceSFX.Play(); //SFX is NPC's voice
		} else {
			dialog.soundEffect.Play(); //SFX is the custom one defined in the dialog
		}
		InteractionManager.Instance.RegisterActiveInteraction(Interaction.Type.Talk);
		currentDialog = dialog;
		DisplayCurrentDialog();
	}

	private void DisplayCurrentDialog()  {
		drawingCoroutine = StartCoroutine(DrawDialogCharacters());
	}

	public void SkipCurrentDialog() {
		AbortDrawingCharacters();
		dialogText.text = currentDialog.Sentence;
	}

	private void AbortDrawingCharacters() {
		StopCoroutine(drawingCoroutine);
		drawingCoroutine = null;
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
			yield return new WaitForSeconds(drawSpeed);
			dialogText.text += character;
		}
		isDrawing = false;
	}

	private void FinishConversation() {
		dialogText.gameObject.SetActive(false); //avoids re-drawing costs when emptying the dialogText's text.
		dialogText.text = "";
		dialogBoxAnimator.SetBool("Open", false);
		currentDialog = null;
		InteractionManager.Instance.UnregisterActiveInteraction();
	}

	public bool IsInConversation() {
		return currentDialog != null;
	}
}
