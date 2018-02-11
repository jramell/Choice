using System;
using UnityEngine;

public class DoorPanel : MonoBehaviour, IOpenable {

	public AudioSource panelOpenSFX;

	public DoorAnimator[] doorsToOpen;

	void Start() {

	}

	public void OnOpened() {
		foreach(DoorAnimator doorAnimator in doorsToOpen) {
			doorAnimator.Toggle();
		}
		if(panelOpenSFX != null) {
			panelOpenSFX.Play();
		}
		//other code that happens when a door is open, maybe tell camera to enqueue interactions
	}
}
