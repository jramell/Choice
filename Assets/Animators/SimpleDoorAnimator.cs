using System.Collections;
using UnityEngine;

/// <summary>
/// Handles a simple door's animations, i.e., Open(), Close() for doors that only open upwards
/// </summary>
public class SimpleDoorAnimator : DoorAnimator {

	/// <summary>
	/// How much does the door raise 
	/// </summary>
	public float raiseAmount;

	/// <summary>
	/// How long does the door take to raise in seconds
	/// </summary>
	public float raiseTime;

	private float raiseStepPerSecond;
	private float currentRaise = 0;

	private Coroutine openAnimation;
	private Coroutine closeAnimation;

	void Start() {
		raiseStepPerSecond = raiseAmount / raiseTime;
	}

	public override void Open() {
		if(closeAnimation != null) {
			return;
		}
		openAnimation = StartCoroutine(PlayOpenAnimation());
	}

	public override void Close() {
		if (openAnimation != null) {
			return;
		}
		closeAnimation = StartCoroutine(PlayCloseAnimation());
	}

	IEnumerator PlayOpenAnimation() {
		while(currentRaise < raiseAmount) {
			transform.position = new Vector2(transform.position.x, transform.position.y + raiseStepPerSecond * 0.015f);
			yield return new WaitForSeconds(0.015f);
			currentRaise += raiseStepPerSecond * 0.015f;
		}
		currentRaise = 0;
		open = true;
		openAnimation = null;
	}

	IEnumerator PlayCloseAnimation() {
		while (currentRaise < raiseAmount) {
			transform.position = new Vector2(transform.position.x, transform.position.y - raiseStepPerSecond * 0.015f);
			yield return new WaitForSeconds(0.015f);
			currentRaise += raiseStepPerSecond * 0.015f;
		}
		currentRaise = 0;
		open = false;
		closeAnimation = null;
	}
}
