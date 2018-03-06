using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chest : MonoBehaviour {

	[Tooltip("Delay between the chest being opened and it triggering an item collection sequence")]
	public float collectDelay = 0.2f;

	[Tooltip("Sound effect that plays when this chest is opened")]
	public AudioSource openSFX;

	private bool open = false;

	/// <summary>
	/// If the chest hasn't been opened, triggers the chest getting opened, and the sequence displayed after that. Otherwise, does nothing.
	/// </summary>
	public void Open() {
		if(open) {
			return;
		}
		open = true;
		if(openSFX != null) {
			openSFX.Play();
		}
		OnOpened();
		//animation stuff
		StartCoroutine(DelayCollectionSequence());
	}

	public bool IsOpen {
		get { return open; }
	}

	protected virtual void OnOpened() {

	}

	protected abstract void StartCollectionSequence();

	private IEnumerator DelayCollectionSequence() {
		yield return new WaitForSeconds(collectDelay);
		StartCollectionSequence();
	}
}
