public interface IOnDialogAvailableListener {
	/// <summary>
	/// Executed by the DialogInputManager when any dialog becomes available
	/// </summary>
	void OnDialogAvailable();

	/// <summary>
	/// Executed by the DialogInputManager when any dialog becomes available
	/// </summary>
	void OnDialogUnavailable();
}