using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))] //requires hitbox
public class Generator : MonoBehaviour, IShockable {

	[Tooltip("Objects that are toggled when the generator is shocked. They should have IActivable components")]
	public List<GameObject> attachedObjects;
	private List<IActivable> attachedActivables;

	void Start() {
		attachedActivables = new List<IActivable>();
		foreach(GameObject element in attachedObjects) {
			IActivable activableComponent = element.GetComponent<IActivable>();
			attachedActivables.Add(activableComponent);
		}
	}

	public void ReceiveShock(float shockStrength) {
		foreach(IActivable activable in attachedActivables) {
			activable.Toggle();
		}
	}
}
