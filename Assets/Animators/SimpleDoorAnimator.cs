using System.Collections;
using UnityEngine;

/// <summary>
/// Handles a simple door's animations, i.e., Open(), Close() for doors that only open upwards
/// </summary>
public class SimpleDoorAnimator : DoorAnimator {

	public enum Direction {
		Up, Down, Left, Right
	}
	/// <summary>
	/// How much does the door move 
	/// </summary>
	public float moveAmount;

	/// <summary>
	/// How long does the door take to move in seconds
	/// </summary>
	public float moveTime;

	public Direction moveDirection;

	private float moveStepPerSecond;
	private float currentMove = 0;
	private float animationStepLength = 0.015f;
	private float xChangePerStep = 0;
	private float yChangePerStep = 0;

	private Coroutine openAnimation;
	private Coroutine closeAnimation;

	void Start() {
		moveStepPerSecond = moveAmount / moveTime;
		if(moveDirection == Direction.Up || moveDirection == Direction.Down) {
			float verticalDirectionModifier = moveDirection == Direction.Up ? 1 : -1;
			yChangePerStep = moveStepPerSecond * animationStepLength * verticalDirectionModifier;
		} else {
			float horizontalDirectionModifier = moveDirection == Direction.Right ? 1 : -1;
			xChangePerStep = moveStepPerSecond * animationStepLength * horizontalDirectionModifier;
		}
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
		while(currentMove < moveAmount) {
			transform.position = new Vector2(transform.position.x + xChangePerStep, transform.position.y + yChangePerStep);
			yield return new WaitForSeconds(animationStepLength);
			currentMove += moveStepPerSecond * animationStepLength;
		}
		currentMove = 0;
		open = true;
		openAnimation = null;
	}

	IEnumerator PlayCloseAnimation() {
		while (currentMove < moveAmount) {
			transform.position = new Vector2(transform.position.x - xChangePerStep, transform.position.y - yChangePerStep);
			yield return new WaitForSeconds(animationStepLength);
			currentMove += moveStepPerSecond * animationStepLength;
		}
		currentMove = 0;
		open = false;
		closeAnimation = null;
	}
}
