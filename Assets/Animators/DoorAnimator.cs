using UnityEngine;

public abstract class DoorAnimator : MonoBehaviour {

	protected bool open = false;

	/// <summary>
	/// Triggers the door's opening animation and opens it
	/// </summary>
	public abstract void Open();

	/// <summary>
	/// Triggers the door's closing animation and closes it
	/// </summary>
	public abstract void Close();

	/// <summary>
	/// Opens door if closed, closes it if opened
	/// </summary>
	public void Toggle() {
		if(open) {
			Close();
		} else {
			Open();
		}
	}
}