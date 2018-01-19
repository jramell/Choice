using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates and deactivates the DialogInputManager when the player enters its trigger, and does the same for dialog input
/// options (for example, buttons of 'Talk' in mobile devices).
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class DialogTrigger : MonoBehaviour {

	[Tooltip("The talking entity whose talk option is enabled when the player enters this script's associated trigger")]
	public Talker talker;

	/// <summary>
	/// The player's dialog input manager, cached on the first OnTriggerEnter2D event for performance.
	/// </summary>
	private DialogInputManager dialogInputManager;

	void OnTriggerEnter2D(Collider2D collider2d) {
		Debug.Log ("TriggerEnter. Collider2d.tag = " + collider2d.tag	);
		if(collider2d.tag == "Player") {
			Debug.Log ("Tag is player");
			if (dialogInputManager == null) {
				dialogInputManager = collider2d.gameObject.GetComponent<DialogInputManager>();
			}
			dialogInputManager.Enable(talker);
		}
	}

	void OnTriggerExit2D(Collider2D collider2d) {
		if(collider2d.tag == "Player") {
			dialogInputManager.Disable();
		}
	}
}
