using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Note: For this to work similarly to a normal 2D controller, check this video: https://www.youtube.com/watch?v=lRGZKZ-qypA
//Improvements: http://error454.com/2013/10/23/platformer-physics-101-and-the-3-fundamental-equations-of-platformers/
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

	#region Walk Settings definition
	[Header("Walk settings")]
	public float speed = 10f;
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
	#endregion

	#region Debug Settings definition
	[SerializeField]
	[Header("Debug Settings")]
	private bool debugJump = true;
	#endregion

	#region Internal variables definition
	/// <summary>
	/// Y-axis velocity of the player when they jump. Calculated in Start() according to maxJumpHeight and timeToApex.
	/// </summary>
	private float jumpVelocity = 0f;

	private bool isGrounded = false;

	/// <summary>
	/// Y-axis velocity the player needs to reach 
	/// </summary>
	private float earlyJumpTerminationVelocity = 0f;

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
	}

	void Update() {
		UpdateInternalState();
		ProcessInput();
		rigidbody2D.velocity = targetVelocity;

	}

	/// <summary>
	/// Resets targetVelocity this frame and checks if the player is grounded. 
	/// </summary>
	private void UpdateInternalState() {
		targetVelocity = rigidbody2D.velocity;
		UpdateIsGrounded();
	}

	/// <summary>
	/// Resets targetVelocity this frame and checks if the player is grounded. 
	/// </summary>
	private void ProcessInput() {
		targetVelocity.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
		ProcessJumpInput();
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
		} else if (!isGrounded && PlayerWantsToInterruptJump()) {
			targetVelocity.y = Mathf.Min(targetVelocity.y, earlyJumpTerminationVelocity);
		}
	}

	bool CanJump() {
		return isGrounded;
	}

	bool PlayerWantsToJump() {
		return Input.GetKeyDown(KeyCode.Space);
	}

	bool PlayerWantsToInterruptJump() {
		return Input.GetKeyUp(KeyCode.Space);
	}

	void Jump() {
		targetVelocity.y = jumpVelocity;
	}
}
