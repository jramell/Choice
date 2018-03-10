/// <summary>
/// Entities that want to be notified when the Pickup UI is dismissed implement this interface
/// </summary>
public interface IPickupUIDismissedListener {

	/// <summary>
	/// Executed every time the pickup UI is dismissed
	/// </summary>
	void OnPickupUIDismissed();
}