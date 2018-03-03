using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Projectile component that always moves towards its local X axis
/// </summary>
[RequireComponent(typeof(Collider2D))] //requires a hitbox
public class Projectile : MonoBehaviour {

	public float speed;

	[Tooltip("Should the projectile change configuration of its Collider2D and Rigidbody2D on Start?")]
	public bool autoSetup;

	private Vector3 translationVector;
	private Rigidbody2D r2d;

	protected virtual void Start() {
		r2d = GetComponent<Rigidbody2D>();
		if (autoSetup) {
			GetComponent<Collider2D>().isTrigger = true;
		}
		translationVector = transform.right * speed;
		StartMoving();
	}

	private void StartMoving() {
		r2d.velocity = translationVector;
	}

	/// <summary>
	/// Makes projectile move in the direction it's aligned with
	/// </summary>
	public void AlignWith(Vector2 direction) {
		transform.rotation = Quaternion.LookRotation(direction, Vector2.up);
	}

	public void SetRotation(Vector3 rotation) { //refence just in case: https://answers.unity.com/questions/654222/make-sprite-look-at-vector2-in-unity-2d-1.html
		transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
	}
}
