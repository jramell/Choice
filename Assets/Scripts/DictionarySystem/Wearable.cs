using UnityEngine;

public abstract class Wearable : MonoBehaviour {

	#region Wearable Settings variable declarations
	[Header("Wearable Settings")]

	///<summary>
	/// Method through which the Buff wears off
	///</summary>
	[SerializeField]
	[Tooltip("Method through which the object wears of")]
	private Duration.Type durationType;

	[SerializeField]
	[Tooltip("How many wear events the object can handle before wearing out completely. Wear events' nature depends on the Duration Type." +
		" A duration type of seconds means the object wears down in time, while a duration type of uses means it wears down every time it's used")]
	protected float duration;
	#endregion

	protected float remainingDuration;

	protected virtual void Start() {
		remainingDuration = duration;
	}

	public Duration.Type DurationType {
		get { return durationType; }
	}

	public float RemainingDuration {
		get { return remainingDuration; }
	}
}
