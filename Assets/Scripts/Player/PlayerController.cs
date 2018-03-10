using UnityEngine;

//Note: For this to work similarly to a normal 2D controller, check this video: https://www.youtube.com/watch?v=lRGZKZ-qypA
//Improvements: http://error454.com/2013/10/23/platformer-physics-101-and-the-3-fundamental-equations-of-platformers/
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

	#region Walk Settings definition
	[Header("Walk settings")]
	public float speed = 10f;

	public float accelerationTimeGrounded = 0.1f;

	public float accelerationTimeAirborne = 0.2f;
	#endregion

	#region Jump Settings definition
	[Header("Jump Settings")]

	//Time to apex in seconds
	public float timeToApex = 0.5f;

	//Max jump height in meters/Unity Units
	public float maxJumpHeight = 10f;

	//Min jump height in meters/Unity Units
	public float minJumpHeight = 5f; //In case of early jump termination

	///<summary>
	///Layers in this LayerMask are considered ground. If the player is colliding with them, they'll be considered grounded.
	///</summary>
	[Tooltip("Layers in this LayerMask are considered ground. If the player is colliding with them, they'll be considered grounded")]
	public LayerMask WhatIsGround;

	/// <summary>
	/// If a layer within WhatIsMask is close enough to this position, the player will be considered grounded. Would normally
	/// correspond to the player's feet.
	/// </summary>
	[SerializeField]
	[Tooltip("If object within WhatIsMask is closer than groundCheckDistance to any position in this list, the player will be considered grounded. There would normally be groundChecks in the player's feet")]
	private Transform[] groundChecks;

	/// <summary>
	/// If groundCheck is at this distance or less from an object in a layer within WhatIsMask, the player will be considered grounded
	/// </summary>
	[SerializeField]
	[Tooltip("If groundCheck is at this distance or less from an object in a layer within WhatIsMask, the player will be considered grounded")]
	private float groundCheckDistance = 0.15f;

	[SerializeField]
	[Tooltip("The player won't fall faster than this. Be warned this could interfere with the jump feeling.")]
	private float maxFallSpeed = 45f;
	#endregion

	#region Wall Sliding Settins definition
	[Header("Wall Sliding Settings")]

	///<summary>
	/// If any object in the slidable LayerMask is at slidingDistance or less to the right of any transform in this list, and the player is
	/// moving left and not grounded, the player is considered to be Wall Sliding
	/// </summary>
	[SerializeField]
	[Tooltip("If any object in the slidable LayerMask is at slidingDistance or less to the left of any transform in this list, and the player is"
		+ " moving left and not grounded, the player is considered to be Wall Sliding")]
	private Transform[] leftSlideChecks;

	///<summary>
	/// If any object in the slidable LayerMask is at slidingDistance or less to the right of any transform in this list, and the player is
	/// moving right and not grounded, the player is considered to be Wall Sliding
	/// </summary>
	[SerializeField]
	[Tooltip("If any object in the slidable LayerMask is at slidingDistance or less to the right of any transform in this list, and the player is"
		+ " moving right and not grounded, the player is considered to be Wall Sliding")]
	private Transform[] rightSlideChecks;

	/// <summary>
	/// Layers the player can wall slide on
	/// </summary>
	[Tooltip("Layers the player can wall slide on")]
	public LayerMask slidable;

	/// <summary>
	/// Distance a slidable surface must be to a slideCheck in the direction the player is moving for the player to be considered Wall Sliding
	/// </summary>
	[Tooltip("Distance a slidable surface must be to a slideCheck in the direction the player is moving for the player to be considered Wall Sliding")]
	public float slideCheckDistance = 0.1f;

	[Tooltip("Speed that player falls when wall sliding. If it's negative, the player will slide upwards")]
	public float maxSlidingSpeed = 5f;

	[Tooltip("Time the player needs to want to move away from a wall to actually move away from a wall. Does not have to do with jumping away from a wall")]
	public float wallStickTime = 0.25f;

	/// <summary>
	/// When the player is Wall Sliding and wants to jump towards the direction of the wall they're sliding on, this impulse is applied.
	/// </summary>
	public Vector2 wallJumpClimbImpulse;

	/// <summary>
	/// When the player is Wall Sliding and wants to jump off the wall they're sliding on, but not in the opposite direction, this impulse is applied.
	/// </summary>
	public Vector2 wallJumpOffImpulse;

	/// <summary>
	/// When the player is Wall Sliding and wants to jump to the opposite direction, this impulse is applied.
	/// </summary>
	public Vector2 wallJumpOppositeImpulse;

	#endregion

	[Header("Sound Effect Settings")]
	public AudioSource jumpSFX;

	public AudioSource wallSlidingSFX;

	#region Debug Settings definition
	[Header("Debug Settings")]

	[SerializeField]
	private bool debugJump = true;

	[SerializeField]
	private bool debugWallSliding = true;
	#endregion

	#region Internal variables definition

	/// <summary>
	/// Direction the player wants to move towards in the X axis in this frame. 1 if it's to the right, -1 if to the left, 0 if none.
	/// Note this isn't necessarily the direction the player is currently moving towards.
	/// </summary>
	private float inputX;

	private float velocityXSmoothing = 0f;

	/// <summary>
	/// Y-axis velocity of the player when they jump. Calculated in Start() according to maxJumpHeight and timeToApex.
	/// </summary>
	private float jumpVelocity = 0f;

	private bool isGrounded = false;

	/// <summary>
	/// Y-axis velocity the player needs to reach 
	/// </summary>
	private float earlyJumpTerminationVelocity = 0f;

	private bool isWallSliding = false;

	/// <summary>
	/// 0 when not wall sliding, 1 when wall sliding to the right, -1 when wall sliding to the left
	/// </summary>
	private int wallSlidingDirection = 0;

	/// <summary>
	/// Time the player needs to want to move away from a wall to actually move away from a wall. Doesn't have to do with jumping away from a wall.
	/// </summary>
	private float timeToWallUnstick;

	/// <summary>
	/// Target velocity of the current frame. Is modified according to the actions the player wants to take.
	/// </summary>
	private Vector2 targetVelocity;

	/// <summary>
	/// Reference to the player's rigidbody2D.
	/// </summary>
	private Rigidbody2D rigidbody2D;

	#endregion

	void Start() {
		rigidbody2D = GetComponent<Rigidbody2D>();	
		timeToApex *= 10; //change unit of timeToApex so the following equations work correctly
		maxJumpHeight *= 10; //change unit of maxJumpHeight so the following equations work correctly
		minJumpHeight *= 10; //change unit of minJumpHeight so the following equations work correctly
		rigidbody2D.gravityScale = (2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
		jumpVelocity = Mathf.Sqrt(2 * rigidbody2D.gravityScale * maxJumpHeight);
		earlyJumpTerminationVelocity = Mathf.Sqrt (
			Mathf.Pow(jumpVelocity, 2) + -2 * rigidbody2D.gravityScale * (maxJumpHeight - minJumpHeight) );
		maxFallSpeed *= -1;
		maxSlidingSpeed *= -1;
		timeToWallUnstick = wallStickTime;
	}

	void Update() { //the order the state of things is updated and differents part of the input processed is important

		//TODO: should probably check input state the frame the player unpauses the game. If its non-zero, keep the current
		//controller internal state. Else, reset velocity effects related to input (other forces should still act on the player).
		//That might get rid of the player "sliding" after unpausing.

		targetVelocity = rigidbody2D.velocity; 
		UpdateIsGrounded();
		ProcessHorizontalMovementInput();
		ProcessJumpInput();
		//------ things processed until now are jump and horizontal movement, both of which affect Wall Sliding. If
		//the player is grounded, he can't wall slide, and if he isn't trying to move horizontally, he won't wall slide regardless
		//of grounded state, hence why Wall Sliding is processed after these.
		UpdateWallSlidingState();
		ProcessWallSlidingInput();
		LimitFallSpeed();
		if(float.IsNaN(targetVelocity.x)) {
			targetVelocity.x = 0;
		}
		if(inputX != 0) {
			PlayerManager.Instance.UpdatePlayerOrientation(Mathf.Sign(targetVelocity.x));
		}
		if (wallSlidingDirection != 0) {
			PlayerManager.Instance.UpdatePlayerOrientation(-wallSlidingDirection);
		}
		HandlePlayingSoundEffects();
		rigidbody2D.velocity = targetVelocity;
	}

	private void HandlePlayingSoundEffects() {
		HandleWallSlidingSFX();
	}

	private void HandleWallSlidingSFX() {
		if(wallSlidingSFX == null) {
			return;
		}
		if(wallSlidingDirection == 0) {
			wallSlidingSFX.Stop();
		} else if(!wallSlidingSFX.isPlaying) {
			wallSlidingSFX.Play();
		}
	}

	private void LimitFallSpeed() {
		targetVelocity.y = Mathf.Max(targetVelocity.y, maxFallSpeed);
	}

	private void ProcessHorizontalMovementInput() {
		inputX = Input.GetAxisRaw("Horizontal");
		float accelerationTime = isGrounded ? accelerationTimeGrounded : accelerationTimeAirborne;
		//Mathf.SmoothDamp handles acceleration, basically
		targetVelocity.x = Mathf.SmoothDamp(rigidbody2D.velocity.x, inputX * speed, ref velocityXSmoothing, accelerationTime);
	}

	private void UpdateWallSlidingState() {
		if (isGrounded) {
			isWallSliding = false;
			return;
		}
		bool currentDirection = wallSlidingDirection == 1;
		CheckIfWallSlidingToDirection(currentDirection); //check current wall sliding direction first
		if (!isWallSliding) {
			CheckIfWallSlidingToDirection(!currentDirection);
		}
	}

	/// <summary>
	/// Checks if wall sliding to the direction passed as a parameter. True means right direction. False means left direction.
	/// </summary>
	/// <param name="rightDirection">If set to <c>true</c> right direction.</param>
	private void CheckIfWallSlidingToDirection(bool rightDirection) {
		Transform[] slideChecks = rightDirection ? rightSlideChecks : leftSlideChecks;
		Vector2 directionVector = rightDirection ? Vector2.right : Vector2.left;
		bool wasWallSliding = isWallSliding;
		isWallSliding = false;
		for(int i = 0; i < slideChecks.Length && !isWallSliding; i++) {
			isWallSliding = Physics2D.Raycast(slideChecks[i].position, directionVector, slideCheckDistance, slidable);
			if (debugWallSliding) {
				Debug.DrawRay(slideChecks[i].position, directionVector * slideCheckDistance, Color.yellow);
			}
		}
		if(isWallSliding) {
			wallSlidingDirection = rightDirection ? 1 : -1;
			if(!wasWallSliding) {
				ResetTimeToWallUnstick();
			}
		} else {
			wallSlidingDirection = 0;
		}
	}

	private void ProcessWallSlidingInput() {
		if(!isWallSliding) {
			return;
		}
		if(targetVelocity.y < maxSlidingSpeed) { //limit wall sliding speed
			targetVelocity.y = maxSlidingSpeed;
		}
		if (timeToWallUnstick > 0) {
			HandleWallSlideUnstick();
		}
		if(PlayerWantsToJump()) {
			inputX = Input.GetAxisRaw("Horizontal"); //shouldn't change from before, but it's an attempt to try to process stuff faster
			bool playerWantsToClimbWall = inputX == wallSlidingDirection;
			bool playerWantsToJumpOffWall = inputX == 0;
			timeToWallUnstick = -1; //so time to wall unstick doesn't mess with X impulse on jump

			if (playerWantsToClimbWall) {
				targetVelocity.x = -wallSlidingDirection * wallJumpClimbImpulse.x;
				targetVelocity.y = wallJumpClimbImpulse.y;
			} else if (playerWantsToJumpOffWall) {
				targetVelocity.x = -wallSlidingDirection * wallJumpOffImpulse.x;
				targetVelocity.y = wallJumpOffImpulse.y;
			} else { //player wants to leap towards opposite wall
				targetVelocity.x = -wallSlidingDirection * wallJumpOppositeImpulse.x;
				targetVelocity.y = wallJumpOppositeImpulse.y;
			}
		}
	}

	private void HandleWallSlideUnstick() {
		velocityXSmoothing = 0;
		targetVelocity.x = 0;
		bool playerWantsToMoveAwayFromWall = inputX != wallSlidingDirection && inputX != 0;
		if (playerWantsToMoveAwayFromWall) {
			timeToWallUnstick -= Time.deltaTime;
		} else {
			ResetTimeToWallUnstick();
		}
	}

	private void ResetTimeToWallUnstick() {
		timeToWallUnstick = wallStickTime;
	}

	/// <summary>
	/// Updates isGrounded according to the player position in the current frame
	/// </summary>
	private void UpdateIsGrounded() {
		isGrounded = false;
		for(int i = 0; i < groundChecks.Length && !isGrounded; i++) { 
			isGrounded = Physics2D.Raycast(groundChecks[i].position, Vector2.down, groundCheckDistance, WhatIsGround);
			if (debugJump) {
				Debug.DrawRay(groundChecks[i].position, Vector2.down * groundCheckDistance, Color.red);
			}
		}
	}

	/// <summary>
	/// Checks if the player can jump and wants to jump 
	/// </summary>
	void ProcessJumpInput() {
		if (CanJump() && PlayerWantsToJump()) {
			Jump();
		}
		if (PlayerWantsToInterruptJump()) {
			targetVelocity.y = Mathf.Min(targetVelocity.y, earlyJumpTerminationVelocity);
		}
	}

	bool CanJump() {
		return isGrounded;
	}

	bool PlayerWantsToJump() {
		return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z);
	}

	bool PlayerWantsToInterruptJump() {
		return Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Z);
	}

	void Jump() {
		targetVelocity.y = jumpVelocity;
		if(jumpSFX != null) {
			jumpSFX.Play();
		}
	}

	/// <summary>
	/// When executed, the PlayerController starts receiving input from the player. If it's already
	/// enabled, does nothing.
	/// </summary>
	public void Enable() {
		enabled = true;
	}

	/// <summary>
	/// When executed, the PlayerController stops receiving input from the player. If it's already
	/// disabled, does nothing.
	/// </summary>
	public void Disable() {
		enabled = false;
		CancelMovement();
	}

	void CancelMovement() {
		rigidbody2D.velocity = Vector2.zero;
	}
}
