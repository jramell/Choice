using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles game-wide variables like the player, as well as its state and the general state of the world
/// </summary>
public class GameStateManager : MonoBehaviour {

	private static GameStateManager instance;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public static GameStateManager Instance {
		get { return instance; }
	}
}
