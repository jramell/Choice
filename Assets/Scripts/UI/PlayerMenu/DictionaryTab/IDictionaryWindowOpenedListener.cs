/// <summary>
/// Entities that want to do something in response to the dictionary window being opened implement this interface
/// </summary>
public interface IDictionaryWindowOpenedListener {
	/// <summary>
	/// Executed when the dictionary window is opened
	/// </summary>
	void OnDictionaryWindowOpened();
}