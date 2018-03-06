using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles access to the dialog manager and ensures no two conversations can exist at the same time, 
/// </summary>
public class DialogManager : MonoBehaviour {

	/// <summary>
	/// Time before drawing the first character after dialog start
	/// </summary>
	[Tooltip("Time before drawing the first character after dialog start")]
	public float startDelay = 0.05f;

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
	private Coroutine drawingCoroutine;
	private CharacterParser characterParser;

	void Awake() {
		instance = this;
	}

	void Start() {
		characterParser = new CharacterParser();
	}

	public static DialogManager Instance {
		get { return instance; }
	}

	public bool IsDrawing {
		get { return drawingCoroutine != null; }
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
			dialogText.gameObject.SetActive(true); //for performance, as noted in https://www.youtube.com/watch?v=_wxitgdx-UI
		}
		dialog.PlaySoundEffect();
		InteractionManager.Instance.RegisterActiveInteraction(Interaction.Type.Talk);
		currentDialog = dialog;
		DisplayCurrentDialog();
		HandleCurrentDialogCustomActionOpen();
	}

	private void DisplayCurrentDialog()  {
		drawingCoroutine = StartCoroutine(DrawDialogCharacters());
	}

	private void HandleCurrentDialogCustomActionOpen() {
		if(currentDialog.customActionType == CustomAction.Type.EnableButton) {
			SetDialogButtonEnabled(true);
		}
	}

	private void HandleCurrentDialogCustomActionClose() {
		if (currentDialog.customActionType == CustomAction.Type.EnableButton) {
			SetDialogButtonEnabled(false);
		}
	}

	public void SkipCurrentDialog() {
		AbortDrawingCharacters();
		dialogText.text = InterlocutorNameInBold() + currentDialog.Sentence;
	}

	private void AbortDrawingCharacters() {
		StopCoroutine(drawingCoroutine);
		drawingCoroutine = null;
	}

	/// <summary>
	/// Writes the dialog characters of the current conversation's current dialog.
	/// </summary>
	/// <returns>The dialog characters.</returns>
	private IEnumerator DrawDialogCharacters() {
		//TODO: Take into account that the interlocutor may not have a name
		dialogText.text = InterlocutorNameInBold();
		characterParser.LoadDialog(currentDialog.Sentence);
		string nextString = "dummy";
		yield return new WaitForSeconds(startDelay);
		while (nextString != "") {
			nextString = characterParser.NextString();
			dialogText.text += nextString;
			yield return new WaitForSeconds(drawSpeed);
		}
		drawingCoroutine = null;
	}

	private void FinishConversation() {
		dialogBoxAnimator.SetBool("Open", false);
		HandleCurrentDialogCustomActionClose();
		currentDialog = null;
		InteractionManager.Instance.UnregisterActiveInteraction();
	}

	private void SetDialogButtonEnabled(bool enabled) {
		currentDialog.buttonToEnable.gameObject.SetActive(enabled);
		currentDialog.buttonToEnable.interactable = enabled; //TODO: Could improve performance by sticking to a standard in Editor,
														  //i.e., button are always interactable 
	}

	public bool IsInConversation() {
		return currentDialog != null;
	}

	private string InterlocutorNameInBold() {
		return "<b>" + currentDialog.Interlocutor.Name + ":</b> ";
	}
}
