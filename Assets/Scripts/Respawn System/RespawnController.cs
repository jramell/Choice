using UnityEngine;

/// <summary>
/// Registers collisions with checkpoints, as well as collisions that would cause the player to return to the last checkpoint.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class RespawnController : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D collider2d) {
		if(collider2d.gameObject.tag == "Checkpoint") {
			RespawnManager.Instance.LastCheckpoint = collider2d.gameObject;
		} else if(collider2d.gameObject.tag == "Spikes") {
			RespawnManager.Instance.ReturnToLastCheckpoint();
		}
	}
}
