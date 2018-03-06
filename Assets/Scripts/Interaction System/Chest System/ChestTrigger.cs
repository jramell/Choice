using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))] //requires a hitbox
public class ChestTrigger : MonoBehaviour, IInteractionTrigger {

	[Tooltip("Chest object this trigger allows to interact with")]
	public Chest chest;

	private ChestController chestController;

	void OnTriggerEnter2D(Collider2D collider2d) {
		if(collider2d.tag == "Player" && !chest.IsOpen) {
			if (chestController == null) {
				chestController = collider2d.gameObject.GetComponent<ChestController>();
			}
			InteractionManager.Instance.RegisterTrigger(this, Interaction.Type.Inspect);
		}
	}

	void OnTriggerExit2D(Collider2D collider2d) {
		if(collider2d.tag == "Player" && !chest.IsOpen) {
			InteractionManager.Instance.UnregisterTrigger(this);
		}
	}

	public void EnableInteraction() {
		if(chest.IsOpen) {
			InteractionManager.Instance.UnregisterTrigger(this);
			return;
		}
		chestController.Enable(chest);
	}

	public void DisableInteraction() {
		chestController.Disable();
	}

	public Interaction.Type InteractionType() {
		return Interaction.Type.Inspect;
	}
}
