using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Handles the part of the UI that appears when a pickup is obtained.
/// </summary>
public class PickupUIManager : MonoBehaviour {

	[Header("Controller Settings")]
	public PickupUIController pickupUIController;

	#region UI Settings
	[Header("UI Settings")]

	[Tooltip("Canvas group all elements in the pickup UI belong to")]
	public CanvasGroup pickupCanvasGroup;

	public Image backgroundImage;

	public Text newPickupText;

	public Text pickupNameText;

	public Text pickupDescriptionText;
	#endregion

	#region Animation Settings variable declarations
	[Header("Animation Settings")]

	[Tooltip("Defines behavior of the animation of the background")]
	public FadeConfig backgroundConfig;

	[Tooltip("Defines behavior of the animation of the text that says the type of new pickup acquired")]
	public FadeConfig newPickupConfig;

	[Tooltip("Defines behavior of the animation of the text with the name of what was picked up")]
	public FadeConfig pickupNameConfig;

	public FadeConfig pickupDescriptionConfig;

	[Tooltip("Defines behavior of animation of the whole Pickup UI fading out when dismissed by the player")]
	public FadeConfig dismissConfig;

	#endregion

	#region Word Pickup Settings
	[Header("Word Pickup Settings")]

	[Tooltip("Message displayed when a new word is picked up")]
	public string newWordMessage;
	#endregion

	private static PickupUIManager instance;
	private bool isDismissing = false;
	private bool visible = false;

	void Awake() {
		instance = this;
	}

	void Start() {
		ResetState();
	}

	public static PickupUIManager Instance {
		get { return instance; }
	}

	private void AnnounceSystemBegin() {
		SystemManager.Instance.RegisterActiveSystem(GameSystem.Type.PickupUI);
		visible = true;
	}

	private void AnnounceSystemDismissal() {
		SystemManager.Instance.UnregisterActiveSystem(GameSystem.Type.PickupUI);
		visible = false;
	}

	public void DisplayWithWord(Word word) {
		AnnounceSystemBegin();
		newPickupText.text = newWordMessage;
		pickupNameText.text = word.WordName;
		pickupDescriptionText.text = word.PickupDescription;
		StartCoroutine(AnimatePickupUI());
	}

	public void DismissWindow() {
		if(isDismissing || !visible) {
			return;
		}
		pickupUIController.Disable();
		StartCoroutine(FadeOutWindow());
	}

	private IEnumerator AnimatePickupUI() {
		yield return new WaitForSecondsRealtime(backgroundConfig.startDelay);
		backgroundImage.gameObject.SetActive(true); //for performance reasons, background is disabled at the start of the animation
		UIUtils.Instance.FadeGraphic(backgroundImage, backgroundConfig.fadeTime, backgroundConfig.finalAlpha, backgroundConfig.preFadeSound);
		yield return new WaitForSecondsRealtime(backgroundConfig.fadeTime + newPickupConfig.startDelay);

		//time to show text that says "New word acquired!" or something alike. Code could change an
		//animation controller's state, but right now it just fades in.
		newPickupText.gameObject.SetActive(true);
		UIUtils.Instance.FadeGraphic(newPickupText, newPickupConfig.fadeTime, newPickupConfig.finalAlpha, newPickupConfig.preFadeSound);
		yield return new WaitForSecondsRealtime(newPickupConfig.fadeTime + pickupNameConfig.startDelay);

		pickupNameText.gameObject.SetActive(true);
		UIUtils.Instance.FadeGraphic(pickupNameText, pickupNameConfig.fadeTime, pickupNameConfig.finalAlpha, pickupNameConfig.preFadeSound);
		yield return new WaitForSecondsRealtime(pickupNameConfig.fadeTime + pickupDescriptionConfig.startDelay);

		pickupDescriptionText.gameObject.SetActive(true);
		UIUtils.Instance.FadeGraphic(pickupDescriptionText, pickupDescriptionConfig.fadeTime, pickupDescriptionConfig.finalAlpha, pickupDescriptionConfig.preFadeSound);

		pickupUIController.Enable();
	}

	private IEnumerator FadeOutWindow() {
		isDismissing = true;
		UIUtils.Instance.FadeCanvasGroup(pickupCanvasGroup, dismissConfig.fadeTime, 0, dismissConfig.preFadeSound);
		yield return new WaitForSecondsRealtime(dismissConfig.fadeTime + 0.05f);
		//deactivates UI components to avoid Unity redrawing them when pickup UI is inactive -- source: https://www.youtube.com/watch?v=_wxitgdx-UI
		ResetState();
		AnnounceSystemDismissal();
		isDismissing = false;
	}

	/// <summary>
	/// Deactivates everything and sets up state for the next DisplaySomething() call
	/// </summary>
	private void ResetState() {
		backgroundImage.gameObject.SetActive(false);
		newPickupText.gameObject.SetActive(false);
		pickupNameText.gameObject.SetActive(false);
		pickupDescriptionText.gameObject.SetActive(false);
		UIUtils.Instance.MakeGraphicInvisible(backgroundImage);
		UIUtils.Instance.MakeGraphicInvisible(newPickupText);
		UIUtils.Instance.MakeGraphicInvisible(pickupNameText);
		UIUtils.Instance.MakeGraphicInvisible(pickupDescriptionText);
	}
}

[System.Serializable]
public class FadeConfig {
	[Tooltip("Delay in seconds before the animation of the element starts")]
	public float startDelay;

	[Tooltip("Time in seconds the graphic will take to fade in/out")]
	public float fadeTime;

	[Tooltip("Alpha the graphic will have after it finishes fading in/out")]
	public float finalAlpha = 1f;

	[Tooltip("Sound that will play as the fading starts. If null, no sound will be played")]
	public AudioSource preFadeSound;
}
