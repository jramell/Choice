using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles pausing and micropausing.
/// </summary>
public class PauseManager : MonoBehaviour {

	private static PauseManager instance;
	private bool isPaused = false;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public static PauseManager Instance {
		get { return instance; }
	}

	public bool IsPaused {
		get { return isPaused; }
	}

	public void Pause() {
		Time.timeScale = 0;
		isPaused = true;
	}

	public void Unpause() {
		Time.timeScale = 1;
		isPaused = false;
	}

	/// <summary>
	/// Pauses immediately and unpauses once the number of seconds passed as a parameter have elapsed.
	/// </summary>
	/// <param name="seconds">number of seconds the pause should last</param>
	public void PauseFor(float seconds) {
		Pause();
		WaitAndUnpause(seconds);
	}

	IEnumerator WaitAndUnpause(float waitTime) {
		yield return new WaitForSecondsRealtime(waitTime);
		Unpause();
	}
}
