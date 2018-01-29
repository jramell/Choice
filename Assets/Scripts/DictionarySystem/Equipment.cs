public abstract class Equipment : Wearable {

	/// <summary>
	/// Executed when the player wants to use the equipment. Handles moving a hitbox and checking for certain interactables
	/// within it, in order to interact with them. Animation and other transitions are handled by the EquipmentController,
	/// since it listens for player input.
	/// </summary>
	public abstract void Use();

	/// <summary>
	/// Executed when the equipment the player is currently using breaks. Handles any on-breaking effects the Equipment
	/// may have. Note that other effects such as animation and sound effects on-breaking are handled by the EquipmentManager,
	/// since it handles the Equipment's lifetime.
	/// </summary>
	public abstract void Break();
}
