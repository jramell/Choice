using UnityEngine;
using UnityEngine.UI;

public class PickupButton : MonoBehaviour {

	[Tooltip("What kind of pickup does pressing this button give you?")]
	public Pickup.Type pickupType;

	[Tooltip("Word that is picked up if pickupType is word. Otherwise, does nothing.")]
	public Word wordPickup;

	private bool consumed = false;

	void Start() {
		GetComponent<Button>().onClick.AddListener(() => OnClick());
	}

	private void OnClick() {
		if(!consumed) {
			consumed = true;
			ObtainPickup();
		} //else, something could be done, e.g. play a sound effect
	}

	private void ObtainPickup() {
		if (pickupType == Pickup.Type.Word) {
			PickupManager.Instance.ObtainWord(wordPickup);
		}
	}
}
