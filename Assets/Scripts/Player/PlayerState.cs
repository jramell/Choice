using UnityEngine;

/// <summary>
/// Holds variables related to player state within the world. Should only be modified by controllers through PlayerManager to
/// avoid debugging nightmares. Does not update UI.
/// </summary>
public class PlayerState {

	private float currentHealth;

	private Vector2 playerVelocity = Vector2.zero;

	/// <summary>
	/// 1 if the player is looking to the right, -1 if they're looking to the left.
	/// </summary>
	private float playerOrientation;

	IPlayerStateChangedListener playerStateListeners;

	/// <summary>
	/// Player's current health. Should only be updated through the PlayerManager to avoid debugging problems.
	/// </summary>
	public float Health {
		get { return currentHealth; }
		set {
			currentHealth = value;
			//call "on health changed listeners"
		}
	}

	/// <summary>
	/// 1 if the player is looking to the right, -1 if they're looking to the left. 
	/// </summary>
	public float PlayerOrientation {
		get { return playerOrientation; }
		set { playerOrientation = value; }
	}

	public Vector2 PlayerVelocity {
		get { return playerVelocity; }
		set { playerVelocity = value; }
	}
}