/// <summary>
/// Entities that can be shocked by electricity implement this interface.
/// </summary>
public interface IShockable {
	/// <summary>
	/// The entity receives a shock with the strength passed as a parameter. It's up to the entity how
	/// to react to this; it could receive damage, or do something else, like activate a mechanism.
	/// </summary>
	/// <param name="shockStrength">Strength of the electricity shock received.</param>
	void ReceiveShock(float shockStrength);
}