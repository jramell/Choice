using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform player;
	public float maxSpeed;
	public float maxHeight;

	private void LateUpdate() {
		//Mathf.SmoothStep(transform.position.x, player.position.x, )
		float targetX = player.position.x;
		float targetY = player.position.y < maxHeight ? player.position.y : transform.position.y;
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
}
