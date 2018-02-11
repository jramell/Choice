using UnityEngine;

public class RespawnManager : MonoBehaviour {

	private GameObject lastCheckpoint;
	private GameObject respawnPosition;

	private static RespawnManager instance;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public static RespawnManager Instance {
		get { return instance; }
	}

	public GameObject LastCheckpoint {
		get { return lastCheckpoint; }
		set { lastCheckpoint = value; }
	}

	/// <summary>
	/// Sets the player position to that of the last checkpoint it touched. Does not reset the state of the
	/// world.
	/// </summary>
	public void ReturnToLastCheckpoint() {
		//handle any code that needs to run when the player respawns
		PlayerManager.Instance.PlayerGameObject.transform.position = lastCheckpoint.transform.position;
	}

	/// <summary>
	/// Sets the player position to the current respawn position, resets the state of the world according
	/// to respawn design.
	/// </summary>
	public void Respawn() {

	}
}
