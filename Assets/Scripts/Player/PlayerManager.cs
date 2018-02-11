using UnityEngine;

/// <summary>
/// Handles player's state within the world
/// </summary>
public class PlayerManager : MonoBehaviour {

	///<summary>
	/// The player's Max Health
	///</summary>
	[SerializeField]
	[Tooltip("The player's Max Health")]
	private float maxHealth = 100;

	#region Internal variables definition
	private static PlayerManager instance;
	private PlayerState playerState;
	private GameObject playerGameObject;
	private Animator playerAnimator;
	#endregion

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
		playerState = new PlayerState();
		playerState.Health = maxHealth;
	}

	public static PlayerManager Instance {
		get { return instance; }
	}

	public PlayerState PlayerState {
		get { return playerState; }
	}

	public void UpdatePlayerOrientation(float orientation) {
		playerState.PlayerOrientation = orientation;
	}

	public GameObject PlayerGameObject {
		get {
			InitializePlayerGameObject();
			return playerGameObject;
		}
	}

	public Animator PlayerAnimator {
		get {
			InitializePlayerGameObject();
			return playerAnimator;
		}
	}

	private void InitializePlayerGameObject() {
		if (playerGameObject == null) {
			playerGameObject = GameObject.FindGameObjectWithTag("Player");
		}
		playerAnimator = playerGameObject.GetComponent<Animator>();
	}

	public void TakeDamage(int damage) {

	}

	public void Heal(int healAmount) {
		playerState.Health = Mathf.Min(playerState.Health + healAmount, maxHealth);
	}

	public void ReplenishHealth() {
		playerState.Health = maxHealth;
	}
}
