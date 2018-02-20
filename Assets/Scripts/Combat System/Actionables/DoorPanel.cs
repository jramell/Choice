using System;
using UnityEngine;

public class DoorPanel : MonoBehaviour, IOpenable {

	public AudioSource panelOpenSFX;

	public DoorAnimator[] doorsToOpen;

	public bool reusable = true;

	private int timesUsed = 0;

	public void OnOpened() {
		if(!reusable && timesUsed > 0) {
			return;
		}
		++timesUsed;
		foreach(DoorAnimator doorAnimator in doorsToOpen) {
			doorAnimator.Toggle();
		}
		if(panelOpenSFX != null) {
			panelOpenSFX.Play();
		}
		//other code that happens when a door is open, maybe tell camera to enqueue interactions
	}
}
