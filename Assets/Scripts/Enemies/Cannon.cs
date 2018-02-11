using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds information about the Cannon enemy and handles reacting to stimuli
/// </summary>
public class Cannon : MonoBehaviour {
	[SerializeField]
	private int maxHealth;

	private int currentHealth;
	private List<IHealthChangedListener> healthChangedListeners;

	void Start() {
		currentHealth = maxHealth;
	}

	public void RegisterHealthChangedListener(IHealthChangedListener listener) {
		if(healthChangedListeners == null) {
			healthChangedListeners = new List<IHealthChangedListener>(1);
		}
		healthChangedListeners.Add(listener);
	}

	private int CurrentHealth {
		set {
			currentHealth = value;
			if(healthChangedListeners != null) {
				foreach(IHealthChangedListener listener in healthChangedListeners) {
					listener.OnHealthChanged();
				}
			}
		}
	}

	private void TakeDamage(int damage) {
		currentHealth -= damage;
		HandleDamageTakenFeedback(damage);
	}

	private void HandleDamageTakenFeedback(int damage) {
		if(currentHealth == 0) {
			//handle death animation/feedback. Alternatively, dedicate a component specifically for that.
		}
	}
}
