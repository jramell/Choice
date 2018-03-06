using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Contains utility functions for UI purposes
/// </summary>
public class UIUtils : MonoBehaviour {

	private static UIUtils instance;

	void Awake() {
		instance = this;
	}

	public static UIUtils Instance {
		get { return instance; }
	}

	/// <summary>
	/// Fades in an image from its original color to the same color but with the endAlpha passed as a parameter. 
	/// </summary>
	/// <param name="image">Image to fade in</param>
	/// <param name="timeToFadeIn">Time the image will take to get to the endAlpha passed as a parameter</param>
	/// <param name="endAlpha">Alpha the image will have once it has completely faded in</param>
	/// <param name="fadeStepLength">Amount of time between each alpha change for the image. The lower, the more performance cost</param>
	public void FadeGraphic(MaskableGraphic graphic, float timeToFade, float endAlpha=1, AudioSource preFadeSound=null, float fadeStepLength=0.01f) {
		if(preFadeSound != null) {
			preFadeSound.Play();
		}
		StartCoroutine(_FadeGraphic(graphic, timeToFade, endAlpha, fadeStepLength));
	}

	public void FadeCanvasGroup(CanvasGroup canvasGroup, float timeToFade, float endAlpha=1, AudioSource preFadeSound = null, float fadeStepLength = 0.01f) {
		if (preFadeSound != null) {
			preFadeSound.Play();
		}
		StartCoroutine(_FadeCanvasGroup(canvasGroup, timeToFade, endAlpha, fadeStepLength));
	}

	public void MakeGraphicInvisible(MaskableGraphic graphic) {
		graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, 0);
	}

	public void MakeGraphicOpaque(MaskableGraphic graphic) {
		graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, 1);
	}

	private static IEnumerator _FadeGraphic(MaskableGraphic graphic, float time, float finalAlpha, float fadeStepLength) {
		if(Mathf.Approximately(time, 0) || time < 0f) {
			instance.MakeGraphicOpaque(graphic);
		} else {
			Color fadedInColor = new Color(graphic.color.r, graphic.color.g, graphic.color.b, finalAlpha);
			Color originalColor = graphic.color;
			float transitionStep = fadeStepLength / time;
			float transitionAmount = transitionStep;
			while (transitionAmount < 1) {
				graphic.color = Color.Lerp(originalColor, fadedInColor, transitionAmount);
				transitionAmount += transitionStep;
				yield return new WaitForSecondsRealtime(fadeStepLength);
			}
		}

	}

	private static IEnumerator _FadeCanvasGroup(CanvasGroup canvasGroup, float time, float finalAlpha, float fadeStepLength) {
		float originalAlpha = canvasGroup.alpha;
		float transitionStep = fadeStepLength / time;
		float transitionAmount = transitionStep;
		while (transitionAmount < 1) {
			canvasGroup.alpha = Mathf.SmoothStep(originalAlpha, finalAlpha, transitionAmount);
			transitionAmount += transitionStep;
			yield return new WaitForSecondsRealtime(fadeStepLength);
		}
	}
}
