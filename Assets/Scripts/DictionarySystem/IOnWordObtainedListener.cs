/// <summary>
/// Entities that want to perform an action in response to a word being obtained by the player implement this interface
/// </summary>
public interface IOnWordObtainedListener {
	/// <summary>
	/// Executed when the player obtains a new word
	/// </summary>
	void OnWordObtained();
}