/// <summary>
/// I interaction trigger.
/// </summary>
public interface IInteractionTrigger {

	/// <summary>
	/// Called by the InteractionManager when the trigger is allowed to move on with its interaction, this is,
	/// when it is the one the player can do right now.
	/// </summary>
	void EnableInteraction();

	/// <summary>
	/// Disable the trigger interaction, so it tells the input listener to stop listening for input. Is called by 
	/// the InteractionManager when a new trigger wants to register as interactable.
	/// </summary>
	void DisableInteraction();

	/// <returns>Type of interaction the trigger possibilitates (i.e. allows the player to perform)</returns>
	Interaction.Type InteractionType();
}
