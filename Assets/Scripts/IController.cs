/// <summary>
/// Implemented by generic controllers that listen to input, so they can be enabled and disabled.
/// </summary>
public interface IController {

	/// <summary>
	/// Tell controller to start listening for player input
	/// </summary>
	void Enable();

	/// <summary>
	/// Tell controller to stop listening for player input.
	/// </summary>
	void Disable();
}