using System;
using UnityEngine;

public class Door : MonoBehaviour, IOpenable {

	public AudioSource doorOpenSFX;
	private Animator doorAnimator;

	void Start() {
		doorAnimator = GetComponent<Animator>();
	}
	public void OnOpened() {
		doorAnimator.SetBool("Open", true);
		if(doorOpenSFX != null) {
			doorOpenSFX.Play();
		}
		//other code that happens when a door is open
	}
}
