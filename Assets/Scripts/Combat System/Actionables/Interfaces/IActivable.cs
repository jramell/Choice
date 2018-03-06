/// <summary>
/// Interface implemented by objets that can be activated and deactivated
/// </summary>
public interface IActivable {
	void Activate();
	void Deactivate();
	
	/// <summary>
	/// Activates the object if it's activated, deactivates it otherwise
	/// </summary>
	void Toggle();
}