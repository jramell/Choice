using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates and deactivates the DialogInputManager when the player enters its trigger, and does the same for dialog input
/// options (for example, buttons of 'Talk' in mobile devices).
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class DialogTrigger : MonoBehaviour, IInteractionTrigger {

	[Tooltip("The talking entity whose talk option is enabled when the player enters this script's associated trigger")]
	public Talker talker;

	/// <summary>
	/// The player's dialog input manager, cached on the first OnTriggerEnter2D event for performance.
	/// </summary>
	private DialogInputManager dialogInputManager;

	void OnTriggerEnter2D(Collider2D collider2d) {
		if(collider2d.tag == "Player") {
			if (dialogInputManager == null) {
				dialogInputManager = collider2d.gameObject.GetComponent<DialogInputManager>();
			}
			InteractionManager.Instance.RegisterTrigger(this, Interaction.Type.Talk);
		}
	}

	void OnTriggerExit2D(Collider2D collider2d) {
		if(collider2d.tag == "Player") {
			InteractionManager.Instance.UnregisterTrigger(this);
		}
	}

	public void EnableInteraction() {
		dialogInputManager.Enable(talker);
	}

	public void DisableInteraction() {
		dialogInputManager.Disable();
	}

	public Interaction.Type InteractionType() {
		return Interaction.Type.Talk;
	}
}
