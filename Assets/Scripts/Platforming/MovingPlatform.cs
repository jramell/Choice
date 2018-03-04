using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))] //needs to collide with player
public class MovingPlatform : MonoBehaviour {
	[Tooltip("Path the moving platform will follow. Once it reaches the end, it will go back to the beginning. It will follow it in order")]
	public List<PlatformPathPoint> path;

	#region Speed Settings
	[Header("Speed Settings")]

	public float maxSpeed;

	[Range(0.0001f, 100)]
	[Tooltip("Time the platform will take to accelerate up to max speed")]
	public float timeToMaxSpeed;
	#endregion

	#region Stop Settings
	[Header("Stop Settings")]

	[Tooltip("Should the platform's stop time override its path point's?")]
	public bool overrideStopTime;

	[Tooltip("If overrideStopTime is checked, time the platform will take to stop when arriving at a point it must stop on. Else, does nothing")]
	public float stopTimeOverride;
	#endregion

	private int currentPathIndex = 0;
	private bool turnedOn = true;

	private float accelerationAmount = 0;

	private float velocity;
	/// <summary>
	/// In range [0, 1], where 0 means no distance from last origin and 1 means Vector2.Distance(lastOrigin.position, target.position)
	/// </summary>
	private float distanceFromLastOrigin = 0; 

	/// <summary>
	/// Last point the Moving Platform was at
	/// </summary>
	private Vector2 lastOrigin;

	/// <summary>
	/// True if the platform is decelerating, false otherwise
	/// </summary>
	bool decelerating = false;
	
	void Start() {
		foreach(PlatformPathPoint point in path) {
			if(point.isLocalPosition) {
				point.position.x += transform.position.x;
				point.position.y += transform.position.y;
			}
			if(point.preserveX) {
				point.position.x = transform.position.x;
			}
			if(point.preserveY) {
				point.position.y = transform.position.y;
			}
		}
		SetNextTarget();
	}

	void Update() {
		if(turnedOn) {
			bool reachedTarget = distanceFromLastOrigin >= 0.99f || distanceFromLastOrigin < 0f;
			if(reachedTarget) {
				StartCoroutine(WaitInCurrentPlatform());
			} else {
				AccelerateTowardsTarget();
			}
		}
	}

	private void AccelerateTowardsTarget() {
		float stopTime = overrideStopTime? stopTimeOverride : path[currentPathIndex].stopTime;
		//deceleration that needs to be applied to the platform so it stops in stopTime seconds. Comes from:
		//Vf = Vo + a*t -- Vf = 0, so equation becomes a = -Vo/t
		float deceleration = -velocity / stopTime;
		//the platform must start decelerating at this distance if it wants to stop just as it's reaching its target. Comes from:
		//s = s_o + Vo*t + (a*t^2)/2 -- since we're interested in distance relative to the platform, s_o = 0
		float decelerateThreshold = velocity * stopTime + 0.5f * deceleration * stopTime * stopTime;
		float remainingDist = 1 - distanceFromLastOrigin;
		if(remainingDist <= decelerateThreshold && !decelerating) {
			accelerationAmount = deceleration;
			decelerating = true;
		}
		velocity += accelerationAmount * Time.deltaTime;
		float maxSpeedScaled = maxSpeed / Vector2.Distance(lastOrigin, path[currentPathIndex].position);
		velocity = Mathf.Clamp(velocity, -maxSpeedScaled, maxSpeedScaled);
		distanceFromLastOrigin += velocity * Time.deltaTime;
		transform.position = Vector2.MoveTowards(lastOrigin, path[currentPathIndex].position, 
						distanceFromLastOrigin * Vector2.Distance(lastOrigin, path[currentPathIndex].position));
	}

	private void SetNextTarget() {
		accelerationAmount = maxSpeed / timeToMaxSpeed;
		lastOrigin = transform.position;
		accelerationAmount /= Vector2.Distance(lastOrigin, path[currentPathIndex].position);
	}

	private void UpdateMovementState() {
		decelerating = false;
		distanceFromLastOrigin = 0;
		velocity = 0;
		++currentPathIndex;
		if(currentPathIndex == path.Count) {
			currentPathIndex = 0;
		}
		SetNextTarget();
	}

	private IEnumerator WaitInCurrentPlatform() {
		enabled = false;
		yield return new WaitForSeconds(path[currentPathIndex].stayTime);
		UpdateMovementState();
		if(turnedOn) {
			enabled = true;
		}
	}

	public void TurnOn() {
		turnedOn = true;
		enabled = true;
	}

	public void TurnOff() {
		turnedOn = false;
		enabled = false;
	}

	void OnCollisionEnter2D(Collision2D col) {
		if(col.gameObject.tag == "Player") {
			col.transform.SetParent(transform);
		}
	}

	void OnCollisionExit2D(Collision2D col) {
		if (col.gameObject.tag == "Player") {
			col.transform.SetParent(null);
		}
	}
}

[System.Serializable]
public class PlatformPathPoint {
	public Vector3 position;

	public bool isLocalPosition; 

	public bool preserveX;

	public bool preserveY;

	[Tooltip("How long in seconds the platform should take to stop completely when arriving at this point. Can be overwritten by the platform's global configurations")]
	public float stopTime;

	[Tooltip("How long in seconds the platform should stay at this point once it's arrived")]
	public float stayTime;
}