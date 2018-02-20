using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the part of the UI that appears when a pickup is obtained.
/// </summary>
public class PickupUIManager : MonoBehaviour {

	public Image background;


	private static PickupUIManager instance;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public static PickupUIManager Instance {
		get { return instance; }
	}

	private void AnnounceSystemTransition() {
		//SystemManager.Instance.RegisterActiveSystem(GameSystem.Type.PickupUI);
	}

	public void DisplayWithWord(Word word) {
		Debug.Log("word picked up and pretty UI is being shown right now");
	}
}
