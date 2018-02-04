/// <summary>
/// Interface that world classes that can be opened through some means implement
/// </summary>
public interface IOpenable {
	/// <summary>
	///	Executed when the object is opened through some mean. Should manage UI representation of opening, if
	///	appropriate.
	/// </summary>
	void OnOpened();
}
