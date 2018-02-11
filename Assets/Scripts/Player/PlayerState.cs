/// <summary>
/// Holds variables related to player state within the world. Should only be modified by controllers through PlayerManager to
/// avoid debugging nightmares. Does not update UI.
/// </summary>
public class PlayerState {

	private float currentHealth;

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
}