using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Note: For this to work similarly to a normal 2D controller, check this video: https://www.youtube.com/watch?v=lRGZKZ-qypA
//Improvements: http://error454.com/2013/10/23/platformer-physics-101-and-the-3-fundamental-equations-of-platformers/
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

	[Header("Walk settings")]
	public float speed = 10f;

	[Header("Jump Settings")]

	//Time to apex in seconds
	public float timeToApex = 0.5f;

	//Jump height in meters/Unity Units
	public float jumpHeight = 10f;

	///<summary>
	///Layers in this LayerMask are considered ground. If the player is colliding with them, they'll be considered grounded.
	///</summary>
	[Tooltip("Layers in this LayerMask are considered ground. If the player is colliding with them, they'll be considered grounded")]
	public LayerMask WhatIsGround;

	/// <summary>
	/// If a layer within WhatIsMask is close enough to this position, the player will be considered grounded. Would normally
	/// correspond to the player's feet.
	/// </summary>
	[Tooltip("If an object in a layer within WhatIsMask is close enough to this position, the player will be considered grounded. Would normally orrespond to the player's feet")]
	public Transform groundCheck; 

	/// <summary>
	/// If groundCheck is at this distance or less from an object in a layer within WhatIsMask, the player will be considered grounded
	/// </summary>
	[Tooltip("If groundCheck is at this distance or less from an object in a layer within WhatIsMask, the player will be considered grounded")]
	public float groundCheckRadius = 0.15f;


	//Private


	/// <summary>
	/// Y-axis velocity of the player when they jump
	/// </summary>
	private float jumpVelocity = 250f;

	private bool isGrounded = false;

	/// <summary>
	/// Target velocity of the current frame. Is modified according to the actions the player wants to take.
	/// </summary>
	private Vector2 targetVelocity;

	/// <summary>
	/// Reference to the player's rigidbody2D.
	/// </summary>
	private Rigidbody2D rigidbody2D;

	void Start() {
		rigidbody2D = GetComponent<Rigidbody2D>();	
		timeToApex *= 10; //convert timeToApex
		jumpHeight *= 10;
		rigidbody2D.gravityScale = (2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
		jumpVelocity = Mathf.Sqrt(2 * rigidbody2D.gravityScale * jumpHeight);
	}

	void Update () {
		UpdateInternalState();
		ProcessInput();
		rigidbody2D.velocity = targetVelocity;
	}

	//Resets targetVelocity this frame and checks if the player is grounded. 
	private void UpdateInternalState() {
		targetVelocity = rigidbody2D.velocity;
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, WhatIsGround);
	}

	//Updates horizontal movement and jumps according to input.
	private void ProcessInput() {
		targetVelocity.x = Input.GetAxis ("Horizontal") * speed * Time.deltaTime;
		if (CanJump() && PlayerWantsToJump()) {
			Jump();
		}
	}

	bool CanJump() {
		return isGrounded;
	}

	bool PlayerWantsToJump() {
		return Input.GetKeyDown(KeyCode.Space);
	}

	void Jump() {
		targetVelocity.y = jumpVelocity;
	}
}
