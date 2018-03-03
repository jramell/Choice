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

	private Vector2 currentDeceleration;
	private float accelerationAmount;

	/// <summary>
	/// How much acceleration per second the platform needs in order to reach maxSpeed in timeToMaxSpeed seconds
	/// </summary>
	private Rigidbody2D rigidbody2D;
	private bool needToDecelerateInX = false;
	private bool needToDecelerateInY = false;
	
	void Start() {
		rigidbody2D = GetComponent<Rigidbody2D>();
		accelerationAmount = maxSpeed / timeToMaxSpeed;
		currentDeceleration = new Vector2();
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
	}

	void Update() {
		if(turnedOn) {
			bool reachedTarget = Mathf.Approximately(Vector2.Distance(transform.position, path[currentPathIndex].position), 0f);
			//Debug.Log("reachedTarget = " + reachedTarget + " distance = " + Vector2.Distance(transform.position, path[currentPathIndex].position) + ", vel = " + rigidbody2D.velocity);
			//if (reachedTarget) {
			//	Debug.Log("initiating wait in current platform");
			//	StartCoroutine(WaitInCurrentPlatform());
			//} else {
				AccelerateTowardsTarget();
			//}
		}
	}

	private void AccelerateTowardsTarget() {
		Vector3 distanceVector = path[currentPathIndex].position - transform.position;
		distanceVector.z = 0; //so z position doesn't interfere with calculations
		Vector2 directionVector = Vector3.Normalize(distanceVector);
		Vector2 distanceNeededToDecelerate = Vector2.zero;
		//Debug.Log("needToDecelerateY = " + needToDecelerateInY + ", decelerationY = " + currentDeceleration.y);
		if (!needToDecelerateInX) {
			currentDeceleration.x = -rigidbody2D.velocity.x / path[currentPathIndex].stopTime; //from Vf = at + Vo
			distanceNeededToDecelerate.x = DistanceNeededToDecelerateToFullStop(rigidbody2D.velocity.x, currentDeceleration.x, path[currentPathIndex].stopTime);
			needToDecelerateInX = !Mathf.Approximately(rigidbody2D.velocity.x, 0) && distanceVector.x <= distanceNeededToDecelerate.x;
		}
		if(!needToDecelerateInY) {
			currentDeceleration.y = -rigidbody2D.velocity.y / path[currentPathIndex].stopTime;
			Debug.Log("current Y velocity = " + rigidbody2D.velocity.y + ", currentDecelerationY = " + currentDeceleration.y);
			distanceNeededToDecelerate.y = DistanceNeededToDecelerateToFullStop(rigidbody2D.velocity.y, currentDeceleration.y, path[currentPathIndex].stopTime);
			needToDecelerateInY = !Mathf.Approximately(rigidbody2D.velocity.y, 0) && distanceVector.y <= distanceNeededToDecelerate.y;
		}
		float effectiveXAcceleration = needToDecelerateInX ? currentDeceleration.x : accelerationAmount;
		float effectiveYAcceleration = needToDecelerateInY ? currentDeceleration.y : accelerationAmount;
		AccelerateTowards(directionVector, effectiveXAcceleration, effectiveYAcceleration);
	}

	private void AccelerateTowards(Vector2 direction, float accelerationX, float accelerationY) {
		direction *= Time.deltaTime;
		rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x + direction.x * accelerationX,
			rigidbody2D.velocity.y + direction.y * accelerationY);
		rigidbody2D.velocity = new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, -maxSpeed, maxSpeed),
										   Mathf.Clamp(rigidbody2D.velocity.y, -maxSpeed, maxSpeed));
	}

	private float DistanceNeededToDecelerateToFullStop(float currentVelocity, float deceleration, float stopTime) {
		//x = x_0 + t*Vo + (a*t^2)/2 -- x_0 is 0, since we're calculating distance relative to local position
		float distance = (currentDeceleration.x * stopTime * stopTime) / 2;
		distance += currentVelocity * stopTime;
		return distance;
	}

	private IEnumerator WaitInCurrentPlatform() {
		enabled = false;
		yield return new WaitForSeconds(path[currentPathIndex].stayTime);
		++currentPathIndex;
		if(currentPathIndex == path.Count) {
			currentPathIndex = 0;
		}
		if(turnedOn) {
			enabled = true;
		}
		needToDecelerateInY = false;
		needToDecelerateInX = false;
	}

	public void TurnOn() {
		turnedOn = true;
		enabled = true;
	}

	public void TurnOff() {
		turnedOn = false;
		enabled = false;
	}
	//void OnCollisionEnter2D()
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