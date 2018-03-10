using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewWordPromptDisplayer : MonoBehaviour, IOnWordObtainedListener, IDictionaryWindowOpenedListener, IPickupUIDismissedListener {

	[Tooltip("Object that displays the message that a new word is available")]
	public Text newWordAvailablePrompt;

	[Tooltip("Canvas group the new word available prompt belongs to")]
	public CanvasGroup newWordAvailableCanvasGroup;

	[Tooltip("Time in seconds after the pickup UI is dismissed that the prompt waits to start to fade in")]
	public float displayDelay = 0.2f;

	[Tooltip("Time in seconds for the prompt to fully fade in")]
	public float fadeInTime = 0.2f;

	private bool newWordAvailable = false;

	void Start() {
		DictionaryManager.Instance.AddWordObtainedListener(this);
		PlayerMenuNavigationManager.Instance.AddDictionaryWindowOpenedListener(this);
		PickupUIManager.Instance.AddDismissListener(this);
		HideNewWordAvailablePrompt();
	}

	public void OnWordObtained() {
		newWordAvailable = true;
	}

	public void OnDictionaryWindowOpened() {
		HideNewWordAvailablePrompt();
		newWordAvailable = false;
	}

	public void OnPickupUIDismissed() {
		if(newWordAvailable) {
			DisplayNewWordAvailablePrompt();
		}
	}

	private void DisplayNewWordAvailablePrompt() {
		newWordAvailablePrompt.gameObject.SetActive(true);
		UIUtils.Instance.FadeCanvasGroup(newWordAvailableCanvasGroup, fadeInTime, 1, null, 0.01f, displayDelay);
	}

	private void HideNewWordAvailablePrompt() {
		newWordAvailablePrompt.gameObject.SetActive(false);
		newWordAvailableCanvasGroup.alpha = 0;
	}
}
