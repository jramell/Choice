using System.Collections;
using UnityEngine;

public class DoorPanel : MonoBehaviour, IOpenable {

	public AudioSource panelOpenSFX;

	public float defaultOpenDelay = 0.8f;
	public DoorConfig[] attachedDoorsConfig;


	public bool reusable = true;
	public bool useDefaultOpenDelay = true;

	private int timesUsed = 0;
	private bool beingUsed = false;

	void Start() {
		if(useDefaultOpenDelay) {
			foreach(DoorConfig doorConfig in attachedDoorsConfig) {
				doorConfig.openDelay = defaultOpenDelay;
			}
		}
	}
	public void OnOpened() {
		if(beingUsed) {
			return;
		}
		if(!reusable && timesUsed > 0) {
			return;
		}
		StartCoroutine(ToggleAttachedDoors());
		++timesUsed;
		//other code that happens when a door is open, maybe tell camera to enqueue interactions
	}

	private IEnumerator ToggleAttachedDoors() {
		beingUsed = true;
		foreach (DoorConfig doorConfig in attachedDoorsConfig) {
			if (panelOpenSFX != null) {
				panelOpenSFX.Play();
			}
			yield return new WaitForSeconds(doorConfig.openDelay);
			doorConfig.doorAnimator.Toggle();
		}
		beingUsed = false;
	}
}

[System.Serializable]
public class DoorConfig {
	public DoorAnimator doorAnimator;
	public float openDelay;
}
