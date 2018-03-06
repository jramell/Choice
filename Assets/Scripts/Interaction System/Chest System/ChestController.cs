using UnityEngine;

public class ChestController : MonoBehaviour {

	/// <summary>
	/// Chest the player is interacting with currently
	/// </summary>
	private Chest currentChest;

	void Start() {
		enabled = false;
	}

	void Update() {
		if(PlayerWantsToOpenChest()) {
			currentChest.Open();
		}
	}

	public void Enable(Chest chest) {
		currentChest = chest;
		enabled = true;
	}

	public void Disable() {
		enabled = false;
	}

	private bool PlayerWantsToOpenChest() {
		return Input.GetKeyDown (KeyCode.F) || Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow);
	}
}
