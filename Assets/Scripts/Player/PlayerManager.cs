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

	public Animator playerAnimator;
	public Transform playerSprite;

	#region Internal variables definition
	private static PlayerManager instance;
	private PlayerState playerState;
	private GameObject playerGameObject;
	#endregion

	void Awake() {
		instance = this;
		playerState = new PlayerState();
		playerState.Health = maxHealth;
	}

	private void Start() {
		InitializePlayerGameObject();
	}

	public static PlayerManager Instance {
		get { return instance; }
	}

	public PlayerState PlayerState {
		get { return playerState; }
	}

	public void UpdatePlayerOrientation(float orientation) {
		if(orientation == 0) {
			return;
		}
		playerState.PlayerOrientation = orientation;
		playerSprite.localScale = new Vector3(orientation, playerSprite.localScale.y, 
			playerSprite.localScale.z);
	}

	public void UpdatePlayerGroundedState(bool isGrounded) {
		playerAnimator.SetBool("IsGrounded", isGrounded);
	}

	public void UpdatePlayerVelocity(Vector2 velocity) {
		playerState.PlayerVelocity = velocity;
		playerAnimator.SetFloat("HorizontalSpeed", Mathf.Abs(playerState.PlayerVelocity.x));
		playerAnimator.SetFloat("VerticalSpeed", playerState.PlayerVelocity.y);
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
		if(playerGameObject == null) {
			playerGameObject = GameObject.FindGameObjectWithTag("Player");
		}
		if(playerAnimator == null) {
			playerAnimator = playerGameObject.GetComponent<Animator>();
		}
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
